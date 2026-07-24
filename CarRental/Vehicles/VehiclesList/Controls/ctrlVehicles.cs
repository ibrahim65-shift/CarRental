using CarRental.Attachments.Forms;
using CarRental.Helper;
using CarRental.Vehicles.VehicleDamage.Forms;
using CarRental.Vehicles.VehicleInsurance.Forms;
using CarRental.Vehicles.VehiclesList.Controls;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Services.VehicleInsurance;
using CarRental_Buisness.Services.Vehicles;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehiclesList.Controls
{
    public partial class ctrlVehicles : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  VehicleID  = nameof(VehicleID);
            public const string  MakeName   = nameof(MakeName);
            public const string  ModelName  = nameof(ModelName);
            public const string  Year  = nameof(Year);
            public const string  CurrentMileage  = nameof(CurrentMileage);
            public const string  RentalPricePerDay   = nameof(RentalPricePerDay);
            public const string  FuelTypeName  = nameof(FuelTypeName);
            public const string  CategoryName = nameof(CategoryName);
            public const string  PlateNumber = nameof(PlateNumber);
            public const string  StatusName  = nameof(StatusName );
            public const string  VIN  = nameof(VIN);
            public const string  Color  = nameof(Color);
            public const string  IsDeleted  = nameof(IsDeleted);
            public const string  CreatedDate  = nameof(CreatedDate);
            public const string  CreatedByUserID   = nameof(CreatedByUserID);
            public const string  EditedDate  = nameof(EditedDate);
            public const string  EditedByUserID = nameof(EditedByUserID);
           
        }

        private enum enFilter
        {
             VehicleID,
             MakeName,
             ModelName,
             Year,
             RentalPricePerDay,
             FuelTypeName,
             CategoryName,
             PlateNumber,
             StatusName,
             VIN,
             Color,
             CreatedByUserID,
             EditedByUserID
        }

        public enum enMode { Management , Selection}
        private enMode _mode;

        public event Action<int> VehicleSelectedId;

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsVehicleService _VehicleService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlVehicles(frmMain frmMain, enMode mode)
        {
            InitializeComponent();
            _mode = mode;
            _VehicleService = new clsVehicleService();
            _frmMain = frmMain;
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlVehicles_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                _InitializeUserControl();
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicles.ctrlVehicles_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditVehicle frm = new frmAddEditVehicle(_VehicleService))
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
                clsEventLogger.LogException("ctrlVehicles.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int VehicleId))
                return;

            try
            {
                using (frmAddEditVehicle frm = new frmAddEditVehicle(_VehicleService,VehicleId))
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
                clsEventLogger.LogException("ctrlVehicles.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int VehicleId))
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

                var result = await _VehicleService.DeleteAsync(VehicleId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف المركبة التي تحمل الرقم التعريفي '{VehicleId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف المركبة");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlVehicles.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicles.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicles.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlVehicles.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicles.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicles.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicles.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicles.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void dgvListVehicles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int vehicleId))
                return;

            if(_mode == enMode.Selection)
            {
                VehicleSelectedId?.Invoke(vehicleId);
            }
        }
        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int vehicleId))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(vehicleId))
                frm.ShowDialog();
        }
        private void toolStripMenuItemVehicleDamage_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int vehicleId))
                return;

            using (frmAddEditVehicleDamage frm = new frmAddEditVehicleDamage(vehicleId, null, null))
                frm.ShowDialog();
        }
        private void toolStripMenuItemVehicleInsurance_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleId(out int vehicleId))
                return;

            using (frmAddEditVehicleInsurance frm = new frmAddEditVehicleInsurance(new clsVehicleInsuranceService(),vehicleId, null))
                frm.ShowDialog();
        }
        private void toolStripMenuItemAttach_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, Columns.VehicleID, out int vehicleId))
                return;

            if (!_TryGetCellValue<string>(row, Columns.MakeName, out string make))
                return;

            if (!_TryGetCellValue<string>(row, Columns.ModelName, out string model))
                return;

            if (!_TryGetCellValue<string>(row, Columns.PlateNumber, out string plateNumber))
                return;

            string vehicle = make + " " + model + " - " + plateNumber;

            using (frmRelatedAttachments frm = new frmRelatedAttachments("Vehicles", vehicleId, vehicle))
                frm.ShowDialog();
        }

        // ==================  METHODS ===================

        private void _InitializeUserControl()
        {
            if(_mode == enMode.Management)
            {
                flowLayoutPanelButtons.Visible = true;
                toolStripSeparator1.Visible =true;
                toolStripSeparator2.Visible =true;
                toolStripMenuItemVehicleInsurance.Visible = true;
                toolStripMenuItemVehicleDamage.Visible = true;
            }
            else
            {
                flowLayoutPanelButtons.Visible = false;
                toolStripSeparator1.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripMenuItemVehicleInsurance.Visible = false;
                toolStripMenuItemVehicleDamage.Visible = false;
            }
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
                    clsEventLogger.LogException("ctrlVehicles.LoadDataAsync", ex);
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
                var result = await _VehicleService.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListVehicles.DataSource = null;
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

                dgvListVehicles.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListVehicles.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicles._ApplyPagingAsync", ex);
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
            if (dgvListVehicles.DataSource == null || dgvListVehicles.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.VehicleID        , "المعرف"                  );
            _SetColumnHeader(Columns.MakeName             , "الماركة"                 );
            _SetColumnHeader(Columns.ModelName            , "الموديل"                 );
            _SetColumnHeader(Columns.Year             , "السنة"                   );
            _SetColumnHeader(Columns.CurrentMileage   , "المسافة المقطوعة الحالية");
            _SetColumnHeader(Columns.RentalPricePerDay, "سعر الإيجار اليومي"       );
            _SetColumnHeader(Columns.FuelTypeName     , "نوع الوقود"              );
            _SetColumnHeader(Columns.CategoryName     , "الفئة"                   );
            _SetColumnHeader(Columns.PlateNumber      , "اللوحة"                  );
            _SetColumnHeader(Columns.StatusName       , "الحالة"                  );
            _SetColumnHeader(Columns.VIN              , "الشاصي"                  );
            _SetColumnHeader(Columns.Color            , "اللون"                   );
            _SetColumnHeader(Columns.IsDeleted        , "محذوف ؟"                 );
            _SetColumnHeader(Columns.CreatedDate      , "تاريخ الإنشاء"            );
            _SetColumnHeader(Columns.CreatedByUserID  , "المنشئ"                  );
            _SetColumnHeader(Columns.EditedDate       , "تاريخ التعديل"           );
            _SetColumnHeader(Columns.EditedByUserID   , "المعدل"                  );

            _HideColumn(Columns.IsDeleted);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListVehicles.Columns.Contains(columnName))
                dgvListVehicles.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListVehicles.Columns.Contains(columnName))
                dgvListVehicles.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListVehicles.Rows.Count == 0;
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
            var data = dgvListVehicles.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "المركبات");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"                  , typeof(int));
            exportTable.Columns.Add("الماركة"                 , typeof(string));
            exportTable.Columns.Add("الموديل"                 , typeof(string));
            exportTable.Columns.Add("السنة"                   , typeof(int));
            exportTable.Columns.Add("المسافة المقطوعة الحالية", typeof(int));
            exportTable.Columns.Add("سعر الإيجار اليومي"       , typeof(decimal));
            exportTable.Columns.Add("نوع الوقود"              , typeof(string));
            exportTable.Columns.Add("الفئة"                   , typeof(string));
            exportTable.Columns.Add("اللوحة"                  , typeof(string));
            exportTable.Columns.Add("الحالة"                  , typeof(string));
            exportTable.Columns.Add("الشاصي"                  , typeof(string));
            exportTable.Columns.Add("اللون"                   , typeof(string));
            exportTable.Columns.Add("محذوف ؟"                 , typeof(bool));
            exportTable.Columns.Add("تاريخ الإنشاء"            , typeof(string));
            exportTable.Columns.Add("المنشئ"                  , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل"           , typeof(string));
            exportTable.Columns.Add("المعدل"                  , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"                  ] = row[Columns.VehicleID];
                newRow["الماركة"                 ] = row[Columns.MakeName];
                newRow["الموديل"                 ] = row[Columns.ModelName];
                newRow["السنة"                   ] = row[Columns.Year];
                newRow["المسافة المقطوعة الحالية"] = row[Columns.CurrentMileage];
                newRow["سعر الإيجار اليومي"       ] = row[Columns.RentalPricePerDay];
                newRow["نوع الوقود"              ] = row[Columns.FuelTypeName];
                newRow["الفئة"                   ] = row[Columns.CategoryName];
                newRow["اللوحة"                  ] = row[Columns.PlateNumber];
                newRow["الحالة"                  ] = row[Columns.StatusName];
                newRow["الشاصي"                  ] = row[Columns.VIN];
                newRow["اللون"                   ] = row[Columns.Color];
                newRow["محذوف ؟"                 ] = row[Columns.IsDeleted];
                newRow["تاريخ الإنشاء"            ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"                  ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"           ] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"                  ] = row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedVehicleId(out int VehicleId)
        {
            VehicleId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.VehicleID, out VehicleId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListVehicles.CurrentRow;

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
                case enFilter.VehicleID: return Columns.VehicleID;
                case enFilter.MakeName: return Columns.MakeName;
                case enFilter.ModelName: return Columns.ModelName;
                case enFilter.Year: return Columns.Year;
                case enFilter.RentalPricePerDay: return Columns.RentalPricePerDay;
                case enFilter.FuelTypeName: return Columns.FuelTypeName;
                case enFilter.CategoryName: return Columns.CategoryName;
                case enFilter.PlateNumber: return Columns.PlateNumber;
                case enFilter.StatusName: return Columns.StatusName;
                case enFilter.VIN: return Columns.VIN;
                case enFilter.Color: return Columns.Color;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

    }
}