using CarRental.Customers.CustomersList.Forms;
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental.Rentals.RatePlans.Controls;
using CarRental.Rentals.RatePlans.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.RatePlans;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Rentals.RatePlans.Controls
{
    public partial class ctrlRatePlans : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  RatePlanID         = nameof(RatePlanID);
            public const string  RatePlanScope      = nameof(RatePlanScope);
            public const string  VehicleID          = nameof(VehicleID);
            public const string  PricePerDay        = nameof(PricePerDay);
            public const string  MinDays            = nameof(MinDays);
            public const string  StartDate          = nameof(StartDate);
            public const string  EndDate            = nameof(EndDate);
            public const string  IsActive           = nameof(IsActive);
            public const string  IsCurrentlyActive  = nameof(IsCurrentlyActive);
            public const string  Notes              = nameof(Notes);
            public const string  CreatedDate        = nameof(CreatedDate);
            public const string  CreatedByUserID    = nameof(CreatedByUserID);
            public const string  EditedDate         = nameof(EditedDate);
            public const string  EditedByUserID     = nameof(EditedByUserID);
           
        }
        private enum enFilter
        {
            RatePlanID,
            RatePlanScope,
            PricePerDay,
            MinDays,
            IsActive,
            IsCurrentlyActive,
            CreatedByUserID,
            EditedByUserID
        }

        private enum enYesOrNo { Yes = 0, No = 1 };

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsRatePlansService _RatePlanservice;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlRatePlans(frmMain frmMain)
        {
            InitializeComponent();
            _RatePlanservice = new clsRatePlansService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlRatePlans_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRatePlans.ctrlRatePlans_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditRatePlans frm = new frmAddEditRatePlans(_RatePlanservice))
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
                clsEventLogger.LogException("ctrlRatePlans.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRatePlanId(out int RatePlansId))
                return;

            try
            {
                using (frmAddEditRatePlans frm = new frmAddEditRatePlans(_RatePlanservice,RatePlansId))
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
                clsEventLogger.LogException("ctrlRatePlans.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRatePlanId(out int RatePlanId))
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

                var result = await _RatePlanservice.DeleteAsync(RatePlanId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف الخطة التي تحمل الرقم التعريفي '{RatePlanId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف الخطة");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlRatePlans.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsActive || _currentColumn == Columns.IsCurrentlyActive )
            {
                cbYesOrNo.SelectedIndex = 0;
                txtSearch.Visible = false;
                cbYesOrNo.Visible = true;
                lblSearch.Visible = false;
            }
            else
            {
                txtSearch.Visible = true;
                cbYesOrNo.Visible = false;
                lblSearch.Visible = true;
            }
        }
        private async void cbYesOrNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _ReloadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRatePlans.cbYesOrNo_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
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
                clsEventLogger.LogException("ctrlRatePlans.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlRatePlans.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }

        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int VehicleID))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(VehicleID))
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
                    clsEventLogger.LogException("ctrlRatePlans.LoadDataAsync", ex);
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
            string value = _GetFilterValue();

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
                var result = await _RatePlanservice.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListRatePlans.DataSource = null;
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

                dgvListRatePlans.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListRatePlans.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRatePlans._ApplyPagingAsync", ex);
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
            if (dgvListRatePlans.DataSource == null || dgvListRatePlans.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.RatePlanID       , "المعرف"       );
            _SetColumnHeader(Columns.RatePlanScope    , "نطاق الخطة"   );
            _SetColumnHeader(Columns.VehicleID        , "معرف المركبة" );
            _SetColumnHeader(Columns.PricePerDay      , "السعر اليومي" );
            _SetColumnHeader(Columns.MinDays          , "أقل مدة"      );
            _SetColumnHeader(Columns.StartDate        , "تاريخ البداية");
            _SetColumnHeader(Columns.EndDate          , "تاريخ النهاية");
            _SetColumnHeader(Columns.IsActive         , "مفعلة ؟"      );
            _SetColumnHeader(Columns.IsCurrentlyActive, "نشط حاليََا ؟"  );
            _SetColumnHeader(Columns.Notes            , "الملاحظات"     );
            _SetColumnHeader(Columns.CreatedDate      , "تاريخ الإنشاء" );
            _SetColumnHeader(Columns.CreatedByUserID  , "المنشئ"       );
            _SetColumnHeader(Columns.EditedDate       , "تاريخ التعديل");
            _SetColumnHeader(Columns.EditedByUserID   , "المعدل"       );


            _HideColumn(Columns.VehicleID);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);
        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListRatePlans.Columns.Contains(columnName))
                dgvListRatePlans.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListRatePlans.Columns.Contains(columnName))
                dgvListRatePlans.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListRatePlans.Rows.Count == 0;
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
            var data = dgvListRatePlans.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "خطط الأسعار");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"       , typeof(int));
            exportTable.Columns.Add("نطاق الخطة"   , typeof(string));
            exportTable.Columns.Add("معرف المركبة" , typeof(int));
            exportTable.Columns.Add("السعر اليومي" , typeof(decimal));
            exportTable.Columns.Add("أقل مدة"      , typeof(int));
            exportTable.Columns.Add("تاريخ البداية", typeof(string));
            exportTable.Columns.Add("تاريخ النهاية", typeof(string));
            exportTable.Columns.Add("مفعلة ؟"      , typeof(bool));
            exportTable.Columns.Add("نشط حاليََا ؟"  , typeof(bool));
            exportTable.Columns.Add("الملاحظات"     , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء" , typeof(string));
            exportTable.Columns.Add("المنشئ"       , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل", typeof(string));
            exportTable.Columns.Add("المعدل"       , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"       ] = row[Columns.RatePlanID];
                newRow["نطاق الخطة"   ] = row[Columns.RatePlanScope];
                newRow["معرف المركبة" ] = row[Columns.VehicleID];
                newRow["السعر اليومي" ] = row[Columns.PricePerDay];
                newRow["أقل مدة"      ] = row[Columns.MinDays];
                newRow["تاريخ البداية"] = clsUtil.FormatDate(row[Columns.StartDate]);
                newRow["تاريخ النهاية"] = clsUtil.FormatDate(row[Columns.EndDate]);
                newRow["مفعلة ؟"      ] = row[Columns.IsActive];
                newRow["نشط حاليََا ؟"  ] = row[Columns.IsCurrentlyActive];
                newRow["الملاحظات"     ] = row[Columns.Notes];
                newRow["تاريخ الإنشاء" ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"       ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"       ] = row[Columns.EditedByUserID];


                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedRatePlanId(out int RatePlanID)
        {
            RatePlanID = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.RatePlanID, out RatePlanID);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListRatePlans.CurrentRow;

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
        private string _GetFilterValue()
        {
            if (_currentColumn == Columns.IsActive || _currentColumn == Columns.IsCurrentlyActive )
                return cbYesOrNo.SelectedIndex == (int)enYesOrNo.Yes ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.RatePlanID: return Columns.RatePlanID;
                case enFilter.RatePlanScope: return Columns.RatePlanScope ;
                case enFilter.PricePerDay: return Columns.PricePerDay;
                case enFilter.MinDays: return Columns.MinDays;
                case enFilter.IsActive: return Columns.IsActive;
                case enFilter.IsCurrentlyActive: return Columns.IsCurrentlyActive;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}