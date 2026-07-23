using CarRental.Attachments.Forms;
using CarRental.Customers.CustomersList.Controls;
using CarRental.Customers.CustomersList.Forms;
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Services.Customers;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CarRental.Vehicles.VehiclesList.Controls.ctrlVehicles;

namespace CarRental.Customers.CustomersList.Controls
{
    public partial class ctrlCustomers : UserControl,IRefreshable
    {
        private static class Columns
        {
            public const string CustomerID          = "CustomerID";
            public const string FullName			= "FullName";
            public const string Age					= "Age";
            public const string Gender				= "Gender";
            public const string Email				= "Email";
            public const string Phone				= "Phone";
            public const string Address				= "Address";
            public const string DriverLicenseNumber	= "DriverLicenseNumber";
            public const string DriverLicenseExpiry	= "DriverLicenseExpiry";
            public const string IsDeleted			= "IsDeleted";
            public const string CreatedDate			= "CreatedDate";
            public const string CreatedByUserID		= "CreatedByUserID";
            public const string EditedDate			= "EditedDate";
            public const string EditedByUserID      = "EditedByUserID";
            public const string PersonID            = "PersonID";
        }

        private enum enFilter
        {
            CustomerID, FullName, Age,
            Gender, Email, Phone, Address, DriverLicenseNumber,
            CreatedByUserID, EditedByUserID
        }

        public enum enMode { Management, Selection }
        private enMode _mode;

        public event Action<int> CustomerSelectedId;
        public event Action<DateTime> DriverLicenseExpiry;

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsCustomerService _CustomerService;

        private int _currentPage = 1;
        private int _totalPages = 0;

  
        public event Action DataRefreshed;

        public ctrlCustomers(frmMain frmMain , enMode mode)
        {
            InitializeComponent();
            _CustomerService = new clsCustomerService();
            _mode = mode;
            _frmMain = frmMain;
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlCustomers_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                _InitializeUserControl();
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.ctrlCustomers_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditCustomer frm = new frmAddEditCustomer(_CustomerService))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        await RefreshDataAsync();
                        DataRefreshed?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedCustomerId(out int CustomerId))
                return;

            try
            {
                using (frmAddEditCustomer frm = new frmAddEditCustomer(_CustomerService,CustomerId))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        await RefreshDataAsync();
                        DataRefreshed?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedCustomerId(out int CustomerId))
                return;

            try
            {
                if (!await _CheckDatabaseConnection())
                {
                    _ShowServerErrorState();
                    return;
                }

                if (!clsMessages.ShowDeleteDialog())
                    return;

                var result = await _CustomerService.DeleteAsync(CustomerId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف العميل الذي يحمل الرقم التعريفي '{CustomerId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف العميل");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlCustomers.btnDelete_Click", ex);
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            _ExportToExcel();
        }
        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblSearch.Visible = string.IsNullOrEmpty(txtSearch.Text);
                int currentRequest = ++_searchRequestId;

                await _debouncer.DebounceAsync(async () =>
                {
                    if (currentRequest != _searchRequestId)
                        return;

                    _currentPage = 1;
                    await _ReloadDataAsync();
                });
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.txtSearch_TextChanged", ex);
                clsMessages.ShowError();
            }
        }
        private async void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    _currentPage = 1;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.txtSearch_KeyPress", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnRefresh_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _ReloadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
        private async void cbPageNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (_isUpdatingPageCombo)
                    return;

                if (cbPageNumber.SelectedIndex >= 0)
                {
                    _currentPage = cbPageNumber.SelectedIndex + 1;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.cbPageNumber_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnPrevious_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentPage < _totalPages)
                {
                    _currentPage++;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemPersonInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.PersonID, out int personID))
                return;

            using (frmPersonCardInfo  frm = new frmPersonCardInfo(personID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemSMS_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
        }
        private void toolStripMenuItemEmail_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
        }
        private void dgvListCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetSelectedCustomerId(out int customerId))
                return;

            if (!_TryGetCellValue<DateTime>(row, Columns.DriverLicenseExpiry, out DateTime driverLicenseExpiry))
                return;

            if (_mode == enMode.Selection)
            {
                CustomerSelectedId?.Invoke(customerId);
                DriverLicenseExpiry?.Invoke(driverLicenseExpiry);
            }
        }
        private void toolStripMenuItemAttach_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetSelectedCustomerId(out int customerId))
                return;

            if (!_TryGetCellValue<string>(row, Columns.FullName, out string customerName))
                return;

            using (frmRelatedAttachments frm = new frmRelatedAttachments("Customers", customerId, customerName))
                frm.ShowDialog();
        }

        // ==================  METHODS ===================

        private void _InitializeUserControl()
        {
            bool isManagement = _mode == enMode.Management;

            btnEdit.Visible = isManagement;
            btnDelete.Visible = isManagement;
            btnExport.Visible = isManagement;
            toolStripSeparator1.Visible = isManagement;
            toolStripMenuItemSMS.Visible = isManagement;
            toolStripMenuItemEmail.Visible = isManagement;
        }
        private async Task _LoadDataAsync()
        {

            using (var loading = new frmLoading())
            {
                try
                {
                    loading.Show();
                    loading.Refresh();
                    if (!await _CheckDatabaseConnection())
                    {
                        _ShowServerErrorState();
                        return;
                    }

                    await _ReloadDataAsync();
                }
                catch (Exception ex)
                {
                    clsMessages.ShowError("حدث خطأ أثناء تحميل البيانات");
                    clsEventLogger.LogException("ctrlCustomers.LoadDataAsync", ex);
                    _ShowServerErrorState();
                }
                finally
                {
                    loading.Close();
                }
            }
        }
        private async Task _ReloadDataAsync()
        {
            string value = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                _currentSearchText = null;
                _currentColumn = null;
            }
            else
            {
                var filter = (enFilter)cbFilter.SelectedIndex;

                _currentSearchText = value;
                _currentColumn = _GetColumnNameForFilter(filter);
            }

            await _ApplyPagingAsync();
        }
        private async Task _ApplyPagingAsync()
        {
            try
            {
                var result = await _CustomerService.GetCustomersPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListCustomers.DataSource = null;
                    _columnsInitialized = false;
                    _UpdatePagingControls(0);
                    _ShowEmptyDataState();
                    return;
                }

                int newTotalPages = result.Data.TotalPages;

                if (_currentPage > newTotalPages && newTotalPages > 0)
                {
                    _currentPage = newTotalPages;
                }

                dgvListCustomers.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListCustomers.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomers._ApplyPagingAsync", ex);
                _ShowServerErrorState();
            }
        }
        private void _UpdatePagingControls(int newTotalPages)
        {
            lblTotalPages.Text = newTotalPages.ToString();
            _totalPages = newTotalPages;

            if (newTotalPages <= 0)
            {
                _currentPage = 1; 
                cbPageNumber.Items.Clear();

                try
                {
                    _isUpdatingPageCombo = true;
                    cbPageNumber.Text = "";
                    cbPageNumber.SelectedIndex = -1;
                }
                finally
                {
                    _isUpdatingPageCombo = false;
                }

                
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;

                return;
            }

            if (cbPageNumber.Items.Count != newTotalPages)
            {
                cbPageNumber.Items.Clear();
                for (int i = 1; i <= newTotalPages; i++)
                    cbPageNumber.Items.Add(i);
            }

            int newIndex = _currentPage - 1;
            if (newIndex >= 0 && newIndex < cbPageNumber.Items.Count)
            {
                try
                {
                    _isUpdatingPageCombo = true;
                    cbPageNumber.SelectedIndex = newIndex;
                }
                finally
                {
                    _isUpdatingPageCombo = false;
                }
            }

            btnPrevious.Enabled = (_currentPage > 1);
            btnNext.Enabled = (_currentPage < _totalPages);
        }
        private void _InitializeColumns()
        {
            if (dgvListCustomers.DataSource == null || dgvListCustomers.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.CustomerID             , "المعرف"             );
            _SetColumnHeader(Columns.FullName				, "الاسم الكامل"        );
            _SetColumnHeader(Columns.Age					, "العمر"              );
            _SetColumnHeader(Columns.Gender					, "الجنس"              );
            _SetColumnHeader(Columns.Email					, "البريد"             );
            _SetColumnHeader(Columns.Phone					, "الهاتف"             );
            _SetColumnHeader(Columns.Address			    , "العنوان"            );
            _SetColumnHeader(Columns.DriverLicenseNumber    , "الرخصة"             );
            _SetColumnHeader(Columns.DriverLicenseExpiry	, "تاريخ انتهاء الرخصة");
            _SetColumnHeader(Columns.IsDeleted				, "محذوف"              );
            _SetColumnHeader(Columns.CreatedDate			, "تاريخ الإنشاء"       );
            _SetColumnHeader(Columns.CreatedByUserID		, "المنشئ"             );
            _SetColumnHeader(Columns.EditedDate				, "تاريخ التعديل"      );
            _SetColumnHeader(Columns.EditedByUserID         , "المعدل"             );

            _HideColumn(Columns.PersonID);
            _HideColumn(Columns.IsDeleted);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListCustomers.Columns.Contains(columnName))
                dgvListCustomers.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListCustomers.Columns.Contains(columnName))
                dgvListCustomers.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListCustomers.Rows.Count == 0;
            lblTitleState.Text = isEmpty ? Properties.Resources.EmptyDataStateTitle : "";
            lblDescriptionState.Text = isEmpty ? Properties.Resources.EmptyDataStateDescription : "";
            pnlState.Visible = isEmpty;
        }
        private void _ShowServerErrorState()
        {
            lblTitleState.Text = Properties.Resources.ServerErrorTitle;
            lblDescriptionState.Text = Properties.Resources.ServerErrorDescription;
            pnlState.Visible = true;
        }
        private void _ExportToExcel()
        {
            var data = dgvListCustomers.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "العملاء");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"             ,typeof(int));
            exportTable.Columns.Add("الاسم الكامل"        , typeof(string));
            exportTable.Columns.Add("العمر"              , typeof(int));
            exportTable.Columns.Add("الجنس"              , typeof(string));
            exportTable.Columns.Add("البريد"             , typeof(string));
            exportTable.Columns.Add("الهاتف"             , typeof(string));
            exportTable.Columns.Add("العنوان"            , typeof(string));
            exportTable.Columns.Add("الرخصة"             , typeof(string));
            exportTable.Columns.Add("تاريخ انتهاء الرخصة", typeof(string));
            exportTable.Columns.Add("محذوف"              , typeof(int));
            exportTable.Columns.Add("تاريخ الإنشاء"       , typeof(string));
            exportTable.Columns.Add("المنشئ"             , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل"      , typeof(string));
            exportTable.Columns.Add("المعدل"             , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"             ] = row[Columns.CustomerID];
                newRow["الاسم الكامل"        ] = row[Columns.FullName];
                newRow["العمر"              ] = row[Columns.Age];
                newRow["الجنس"              ] = row[Columns.Gender];
                newRow["البريد"             ] = row[Columns.Email];
                newRow["الهاتف"             ] = row[Columns.Phone];
                newRow["العنوان"            ] = row[Columns.Address];
                newRow["الرخصة"             ] = row[Columns.DriverLicenseNumber];
                newRow["تاريخ انتهاء الرخصة"] = clsUtil.FormatDate(row[Columns.DriverLicenseExpiry]);
                newRow["محذوف"              ] = row[Columns.IsDeleted];
                newRow["تاريخ الإنشاء"       ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"             ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"      ] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"             ] = row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedCustomerId(out int CustomerId)
        {
            CustomerId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.CustomerID, out CustomerId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListCustomers.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
        }
        private bool _TryGetCellIntValue(DataGridViewRow row, string columnName, out int value)
        {
            value = 0;

            try
            {
                if (row == null)
                    return false;

                if (!row.DataGridView.Columns.Contains(columnName))
                    return false;

                var cell = row.Cells[columnName];
                if (cell?.Value == null || cell.Value == DBNull.Value)
                    return false;

                value = Convert.ToInt32(cell.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool _TryGetCellValue<T>(DataGridViewRow row, string columnName, out T value)
        {
            value = default(T);

            try
            {
                if (row == null)
                    return false;

                if (!row.DataGridView.Columns.Contains(columnName))
                    return false;

                var cell = row.Cells[columnName];
                if (cell?.Value == null || cell.Value == DBNull.Value)
                    return false;

                value = (T)Convert.ChangeType(cell.Value, typeof(T));
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> _CheckDatabaseConnection()
        {
            return await clsUtil.CheckDatabaseConnection();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.CustomerID: return Columns.CustomerID;
                case enFilter.FullName: return Columns.FullName;
                case enFilter.Phone: return Columns.Phone;
                case enFilter.Email: return Columns.Email;
                case enFilter.Address: return Columns.Address;
                case enFilter.Age: return Columns.Age;
                case enFilter.Gender: return Columns.Gender;
                case enFilter.DriverLicenseNumber: return Columns.DriverLicenseNumber;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

       
    }
}