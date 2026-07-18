using CarRental.Helper;
using CarRental.Maintenance.Controls;
using CarRental.Maintenance.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.Maintenance;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Maintenance.Controls
{
    public partial class ctrlMaintenance : UserControl, IRefreshable
    {
        private static class Columns
        {

            public const string  MaintenanceID    = nameof(MaintenanceID);
            public const string  VehicleID        = nameof(VehicleID);
            public const string  MakeName             = nameof(MakeName);
            public const string  ModelName            = nameof(ModelName);
            public const string  Year             = nameof(Year);
            public const string  FuelTypeName     = nameof(FuelTypeName);
            public const string  CategoryName     = nameof(CategoryName);
            public const string  PlateNumber      = nameof(PlateNumber);
            public const string  VIN              = nameof(VIN);
            public const string  Description      = nameof(Description);
            public const string  Cost             = nameof(Cost);
            public const string  Vendor           = nameof(Vendor);
            public const string  CreatedDate      = nameof(CreatedDate);
            public const string  CreatedByUserID  = nameof(CreatedByUserID);
            public const string  EditedDate       = nameof(EditedDate);
            public const string  EditedByUserID   = nameof(EditedByUserID);
           
        }

        private enum enFilter
        {
            MaintenanceID,
            VehicleID,
            MakeName,
            ModelName,
            Year,
            FuelTypeName,
            CategoryName,
            PlateNumber,
            VIN,
            Cost,
            Vendor,
            CreatedByUserID,
            EditedByUserID
        }

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsMaintenanceService _Maintenanceervice;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlMaintenance(frmMain frmMain)
        {
            InitializeComponent();
            _Maintenanceervice = new clsMaintenanceService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlMaintenance_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlMaintenance.ctrlMaintenance_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditMaintenance frm = new frmAddEditMaintenance(_Maintenanceervice))
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
                clsEventLogger.LogException("ctrlMaintenance.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedMaintenanceId(out int MaintenanceId))
                return;

            try
            {
                using (frmAddEditMaintenance frm = new frmAddEditMaintenance(_Maintenanceervice,MaintenanceId))
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
                clsEventLogger.LogException("ctrlMaintenance.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedMaintenanceId(out int MaintenanceId))
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

                var result = await _Maintenanceervice.DeleteAsync(MaintenanceId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف الصيانة التي تحمل الرقم التعريفي '{MaintenanceId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف الصيانة");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlMaintenance.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlMaintenance.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.MaintenanceID, out int MaintenanceID))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(MaintenanceID))
                frm.ShowDialog();
        }


        // ==================  METHODS ===================

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
                    clsEventLogger.LogException("ctrlMaintenance.LoadDataAsync", ex);
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
                var result = await _Maintenanceervice.GetMaintenancePageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListMaintenance.DataSource = null;
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

                dgvListMaintenance.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListMaintenance.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlMaintenance._ApplyPagingAsync", ex);
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

                // تعطيل أزرار التنقل لأن البيانات فارغة
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
            if (dgvListMaintenance.DataSource == null || dgvListMaintenance.Rows.Count == 0)
                return;


            
            _SetColumnHeader(Columns.MaintenanceID  , "المعرف"        );
            _SetColumnHeader(Columns.VehicleID      , "معرف المركبة" );
            _SetColumnHeader(Columns.MakeName           , "الماركة"      );
            _SetColumnHeader(Columns.ModelName          , "الموديل"      );
            _SetColumnHeader(Columns.Year           , "السنة"        );
            _SetColumnHeader(Columns.FuelTypeName   , "نوع الوقود"   );
            _SetColumnHeader(Columns.CategoryName   , "الفئة"        );
            _SetColumnHeader(Columns.PlateNumber    , "اللوحة"       );
            _SetColumnHeader(Columns.VIN            , "الشاصي"       );
            _SetColumnHeader(Columns.Description    , "الوصف"        );
            _SetColumnHeader(Columns.Cost           , "التكلفة"      );
            _SetColumnHeader(Columns.Vendor         , "جهة الصيانة"       );
            _SetColumnHeader(Columns.CreatedDate    , "تاريخ الإنشاء" );
            _SetColumnHeader(Columns.CreatedByUserID, "المنشئ"       );
            _SetColumnHeader(Columns.EditedDate     , "تاريخ التعديل");
            _SetColumnHeader(Columns.EditedByUserID , "المعدل"       );


            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListMaintenance.Columns.Contains(columnName))
                dgvListMaintenance.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListMaintenance.Columns.Contains(columnName))
                dgvListMaintenance.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListMaintenance.Rows.Count == 0;
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
            var data = dgvListMaintenance.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "الصيانات");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"       , typeof(string));
            exportTable.Columns.Add("معرف المركبة" , typeof(string));
            exportTable.Columns.Add("الماركة"      , typeof(string));
            exportTable.Columns.Add("الموديل"      , typeof(string));
            exportTable.Columns.Add("السنة"        , typeof(int));
            exportTable.Columns.Add("نوع الوقود"   , typeof(string));
            exportTable.Columns.Add("الفئة"        , typeof(string));
            exportTable.Columns.Add("اللوحة"       , typeof(string));
            exportTable.Columns.Add("الشاصي"       , typeof(string));
            exportTable.Columns.Add("الوصف"        , typeof(string));
            exportTable.Columns.Add("التكلفة"      , typeof(decimal));
            exportTable.Columns.Add("جهة الصيانة"       , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء" , typeof(string));
            exportTable.Columns.Add("المنشئ"       , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل", typeof(string));
            exportTable.Columns.Add("المعدل"       , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"       ] = row[Columns.MaintenanceID];
                newRow["معرف المركبة" ] = row[Columns.VehicleID];
                newRow["الماركة"      ] = row[Columns.MakeName];
                newRow["الموديل"      ] = row[Columns.ModelName];
                newRow["السنة"        ] = row[Columns.Year];
                newRow["نوع الوقود"   ] = row[Columns.FuelTypeName];
                newRow["الفئة"        ] = row[Columns.CategoryName];
                newRow["اللوحة"       ] = row[Columns.PlateNumber];
                newRow["الشاصي"       ] = row[Columns.VIN];
                newRow["الوصف"        ] = row[Columns.Description];
                newRow["التكلفة"      ] = row[Columns.Cost];
                newRow["جهة الصيانة"       ] = row[Columns.Vendor];
                newRow["تاريخ الإنشاء" ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"       ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"       ] = row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedMaintenanceId(out int MaintenanceId)
        {
            MaintenanceId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.MaintenanceID, out MaintenanceId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListMaintenance.CurrentRow;

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
        private async Task<bool> _CheckDatabaseConnection()
        {
            return await clsUtil.CheckDatabaseConnection();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {

            switch (filter)
            {
                case enFilter.MaintenanceID: return Columns.MaintenanceID;
                case enFilter.VehicleID: return Columns.VehicleID;
                case enFilter.MakeName: return Columns.MakeName;
                case enFilter.ModelName: return Columns.ModelName;
                case enFilter.Year: return Columns.Year;
                case enFilter.FuelTypeName: return Columns.FuelTypeName;
                case enFilter.CategoryName: return Columns.CategoryName;
                case enFilter.PlateNumber: return Columns.PlateNumber;
                case enFilter.VIN: return Columns.VIN;
                case enFilter.Cost: return Columns.Cost;
                case enFilter.Vendor: return Columns.Vendor;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

       
    }
}