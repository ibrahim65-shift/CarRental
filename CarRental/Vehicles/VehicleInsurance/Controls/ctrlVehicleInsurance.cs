using CarRental.Attachments.Forms;
using CarRental.Helper;
using CarRental.Vehicles.VehicleInsurance.Controls;
using CarRental.Vehicles.VehicleInsurance.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.VehicleInsurance;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehicleInsurance.Controls
{
    public partial class ctrlVehicleInsurance : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  InsuranceID       = nameof(InsuranceID);
            public const string  VehicleID		   = nameof(VehicleID);
            public const string  PolicyNumber 	   = nameof(PolicyNumber);
            public const string  ProviderName 	  = nameof(ProviderName);
            public const string  InsuranceType 	   = nameof(InsuranceType);
            public const string  InsuranceCost     = nameof(InsuranceCost);
            public const string  StartDate 		   = nameof(StartDate);
            public const string  EndDate  		   = nameof(EndDate);
            public const string  IsActive 		   = nameof(IsActive);
            public const string  Notes 			   = nameof(Notes);
            public const string  CreatedDate 	   = nameof(CreatedDate);
            public const string  CreatedByUserID   = nameof(CreatedByUserID);
           
        }

        private enum enFilter
        {

             InsuranceID,
             VehicleID,
             PolicyNumber,
             ProviderName,
             InsuranceType,
             IsActive,
             CreatedByUserID
        }

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsVehicleInsuranceService _VehicleInsuranceService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlVehicleInsurance(frmMain frmMain)
        {
            InitializeComponent();
            _VehicleInsuranceService = new clsVehicleInsuranceService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlVehicleInsurance_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleInsurance.ctrlVehicleInsurance_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.InsuranceID, out int? insuranceId))
                return;

            if (!_TryGetSelectedVehicleId(out int? vehicleId))
                return;

            try
            {
                using (frmAddEditVehicleInsurance frm = new frmAddEditVehicleInsurance(_VehicleInsuranceService,vehicleId.Value , insuranceId.Value))
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
                clsEventLogger.LogException("ctrlVehicleInsurance.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.InsuranceID, out int? insuranceId))
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

                var result = await _VehicleInsuranceService.DeleteAsync(insuranceId.Value);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف تأمين المركبة التي تحمل الرقم التعريفي '{insuranceId.Value}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف تأمين المركبة");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlVehicleInsurance.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleInsurance.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int? VehicleID))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(VehicleID.Value))
                frm.ShowDialog();
        }

        private void toolStripMenuItemAttach_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.InsuranceID, out int? insuranceId))
                return;


            using (frmRelatedAttachments frm = new frmRelatedAttachments("VehicleInsurance", insuranceId.Value, "تأمين المركبة"))
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
                    clsEventLogger.LogException("ctrlVehicleInsurance.LoadDataAsync", ex);
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
                var result = await _VehicleInsuranceService.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListVehicleInsurance.DataSource = null;
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

                dgvListVehicleInsurance.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListVehicleInsurance.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleInsurance._ApplyPagingAsync", ex);
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
            if (dgvListVehicleInsurance.DataSource == null || dgvListVehicleInsurance.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.InsuranceID    , "المعرف"        );
            _SetColumnHeader(Columns.VehicleID		, "معرف المركبة"  );
            _SetColumnHeader(Columns.PolicyNumber 	, "رقم الوثيقة"   );
            _SetColumnHeader(Columns.ProviderName 		, "المزوِّد"        );
            _SetColumnHeader(Columns.InsuranceType 	, "نوع التأمين"   );
            _SetColumnHeader(Columns.InsuranceCost  , "تكلفة التأمين" );
            _SetColumnHeader(Columns.StartDate 		, "بداية التأمين" );
            _SetColumnHeader(Columns.EndDate  		, "نهاية التأمين" );
            _SetColumnHeader(Columns.IsActive 		, "نشط ؟"         );
            _SetColumnHeader(Columns.Notes 			, "الملاحظات"      );
            _SetColumnHeader(Columns.CreatedDate 	, "تاريخ الإنشاء"  );
            _SetColumnHeader(Columns.CreatedByUserID, "المنشئ"        );

            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.VehicleID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListVehicleInsurance.Columns.Contains(columnName))
                dgvListVehicleInsurance.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListVehicleInsurance.Columns.Contains(columnName))
                dgvListVehicleInsurance.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListVehicleInsurance.Rows.Count == 0;
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
            var data = dgvListVehicleInsurance.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "تأمين المركبات");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"       , typeof(int));
            exportTable.Columns.Add("معرف المركبة" , typeof(int));
            exportTable.Columns.Add("رقم الوثيقة"  , typeof(string));
            exportTable.Columns.Add("المزوِّد"       , typeof(string));
            exportTable.Columns.Add("نوع التأمين"  , typeof(string));
            exportTable.Columns.Add("تكلفة التأمين", typeof(decimal));
            exportTable.Columns.Add("بداية التأمين", typeof(string));
            exportTable.Columns.Add("نهاية التأمين", typeof(string));
            exportTable.Columns.Add("نشط ؟"        , typeof(string));
            exportTable.Columns.Add("الملاحظات"     , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء" , typeof(string));
            exportTable.Columns.Add("المنشئ"       , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"       ] = row[Columns.InsuranceID];
                newRow["معرف المركبة" ] = row[Columns.VehicleID];
                newRow["رقم الوثيقة"  ] = row[Columns.PolicyNumber];
                newRow["المزوِّد"       ] = row[Columns.ProviderName];
                newRow["نوع التأمين"  ] = row[Columns.InsuranceType];
                newRow["تكلفة التأمين"] = row[Columns.InsuranceCost];
                newRow["بداية التأمين"] = clsUtil.FormatDate(row[Columns.StartDate]);
                newRow["نهاية التأمين"] = clsUtil.FormatDate(row[Columns.EndDate]);
                newRow["نشط ؟"        ] = row[Columns.IsActive];
                newRow["الملاحظات"     ] = row[Columns.Notes];
                newRow["تاريخ الإنشاء" ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"       ] = row[Columns.CreatedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedVehicleId(out int? VehicleId)
        {
            VehicleId = null;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.VehicleID, out VehicleId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListVehicleInsurance.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
        }
        private bool _TryGetCellIntValue(DataGridViewRow row, string columnName, out int? value)
        {
            value = null;

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

                case enFilter.InsuranceID: return Columns.InsuranceID;
                case enFilter.VehicleID: return Columns.VehicleID;
                case enFilter.PolicyNumber: return Columns.PolicyNumber;
                case enFilter.ProviderName: return Columns.ProviderName;
                case enFilter.InsuranceType: return Columns.InsuranceType;
                case enFilter.IsActive: return Columns.IsActive;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

    }
}