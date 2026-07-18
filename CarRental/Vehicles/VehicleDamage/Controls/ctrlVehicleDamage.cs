using CarRental.Helper;
using CarRental.Vehicles.VehicleDamage.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.VehicleDamage;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehicleDamage.Controls
{
    public partial class ctrlVehicleDamage : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  DamageID       = nameof(DamageID);
            public const string  VehicleID      = nameof(VehicleID);
            public const string MakeName = nameof(MakeName);
            public const string ModelName = nameof(ModelName);
            public const string  PlateNumber    = nameof(PlateNumber);
            public const string  Year           = nameof(Year);
            public const string  BookingID      = nameof(BookingID);
            public const string  RentalStartDate= nameof(RentalStartDate);
            public const string  RentalEndDate  = nameof(RentalEndDate);
            public const string  Description    = nameof(Description);
            public const string  EstimatedCost  = nameof(EstimatedCost);
            public const string  CreatedDate    = nameof(CreatedDate);
            public const string  CreatedByUserID= nameof(CreatedByUserID);
            public const string  EditedDate     = nameof(EditedDate);
            public const string  EditedByUserID = nameof(EditedByUserID);           
        }

        private enum enFilter
        {
            DamageID,
            VehicleID,
            BookingID,
            Model,
            Make,
            PlateNumber,
            Year,
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
        private readonly clsVehicleDamageService _VehicleDamageervice;

        private int _currentPage = 1;
        private int _totalPages = 0;

        public event Action DataRefreshed;

        public ctrlVehicleDamage(frmMain frmMain)
        {
            InitializeComponent();
            _VehicleDamageervice = new clsVehicleDamageService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlVehicleDamage_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleDamage.ctrlVehicleDamage_Load", ex);
                clsMessages.ShowError();
            }
        }
     
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedDamageId(out int?damageId))
                return;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int?vehicleId))
                return;

            int? bookingId = null;

            if (_TryGetCellIntValue(row, Columns.BookingID, out int?value))
                bookingId = value;

            try
            {
                using (frmAddEditVehicleDamage frm = new frmAddEditVehicleDamage(vehicleId.Value, bookingId, damageId))
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
                clsEventLogger.LogException("ctrlVehicleDamage.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedDamageId(out int?VehicleId))
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

                var result = await _VehicleDamageervice.DeleteAsync(VehicleId.Value);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف تلف المركبة التي تحمل الرقم التعريفي '{VehicleId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء تلف المركبة");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlVehicleDamage.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleDamage.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int?VehicleID))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(VehicleID.Value))
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
                    clsEventLogger.LogException("ctrlVehicleDamage.LoadDataAsync", ex);
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
                var result = await _VehicleDamageervice.GetVehicleDamagePageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListVehicleDamage.DataSource = null;
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

                dgvListVehicleDamage.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListVehicleDamage.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleDamage._ApplyPagingAsync", ex);
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
            if (dgvListVehicleDamage.DataSource == null || dgvListVehicleDamage.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.DamageID         , "المعرف"         );
            _SetColumnHeader(Columns.VehicleID        , "معرف المركبة"   );
            _SetColumnHeader(Columns.MakeName, "الماركة");
            _SetColumnHeader(Columns.ModelName, "الموديل");
            _SetColumnHeader(Columns.PlateNumber      , "اللوحة"         );
            _SetColumnHeader(Columns.Year             , "السنة"          );
            _SetColumnHeader(Columns.BookingID        , "معرف الحجز"     );
            _SetColumnHeader(Columns.RentalStartDate  , "بداية التأجير"  );
            _SetColumnHeader(Columns.RentalEndDate    , "نهاية التأجير"  );
            _SetColumnHeader(Columns.Description      , "الوصف"          );
            _SetColumnHeader(Columns.EstimatedCost    , "التكلفة المقدرة");
            _SetColumnHeader(Columns.CreatedDate      , "تاريخ الإنشاء"   );
            _SetColumnHeader(Columns.CreatedByUserID  , "المنشئ"         );
            _SetColumnHeader(Columns.EditedDate       , "تاريخ التعديل"  );
            _SetColumnHeader(Columns.EditedByUserID   , "المعدل"         );

            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListVehicleDamage.Columns.Contains(columnName))
                dgvListVehicleDamage.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListVehicleDamage.Columns.Contains(columnName))
                dgvListVehicleDamage.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListVehicleDamage.Rows.Count == 0;
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
            var data = dgvListVehicleDamage.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "أضرار المركبات");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"         , typeof(int));
            exportTable.Columns.Add("معرف المركبة"   , typeof(int));
            exportTable.Columns.Add("الماركة", typeof(string));
            exportTable.Columns.Add("الموديل", typeof(string));
            exportTable.Columns.Add("اللوحة"         , typeof(string));
            exportTable.Columns.Add("السنة"          , typeof(int));
            exportTable.Columns.Add("معرف الحجز"     , typeof(int));
            exportTable.Columns.Add("بداية التأجير"  , typeof(string));
            exportTable.Columns.Add("نهاية التأجير"  , typeof(string));
            exportTable.Columns.Add("الوصف"          , typeof(string));
            exportTable.Columns.Add("التكلفة المقدرة", typeof(decimal));
            exportTable.Columns.Add("تاريخ الإنشاء"   , typeof(string));
            exportTable.Columns.Add("المنشئ"         , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل"  , typeof(string));
            exportTable.Columns.Add("المعدل"         , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"         ] = row[Columns.VehicleID];
                newRow["معرف المركبة"   ] = row[Columns.VehicleID];
                newRow["الماركة"] = row[Columns.MakeName];
                newRow["الموديل"] = row[Columns.ModelName];
                newRow["اللوحة"         ] = row[Columns.PlateNumber];
                newRow["السنة"          ] = row[Columns.Year];
                newRow["معرف الحجز"     ] = row[Columns.BookingID];
                newRow["بداية التأجير"  ] = clsUtil.FormatDate(row[Columns.RentalStartDate]);
                newRow["نهاية التأجير"  ] = clsUtil.FormatDate(row[Columns.RentalEndDate]);
                newRow["الوصف"          ] = row[Columns.Description];
                newRow["التكلفة المقدرة"] = row[Columns.EstimatedCost];
                newRow["تاريخ الإنشاء"   ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"         ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"  ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المعدل"         ] = row[Columns.CreatedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedDamageId(out int?damageId)
        {
            damageId = null;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.DamageID, out damageId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListVehicleDamage.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
        }
        private bool _TryGetCellIntValue(DataGridViewRow row, string columnName, out int?value)
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
                case enFilter.DamageID: return Columns.DamageID;
                case enFilter.VehicleID: return Columns.VehicleID;
                case enFilter.PlateNumber: return Columns.PlateNumber;
                case enFilter.Make: return Columns.MakeName;
                case enFilter.Model: return Columns.ModelName;
                case enFilter.Year: return Columns.Year;
                case enFilter.BookingID: return Columns.BookingID;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

       
    }
}