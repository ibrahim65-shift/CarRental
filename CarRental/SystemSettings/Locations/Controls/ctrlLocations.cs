
using CarRental.Helper;
using CarRental.SystemSettings.Locations.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Services.Locations;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.SystemSettings.Locations.Controls
{
    public partial class ctrlLocations : UserControl
    {
        private static class Columns
        {
            public const string LocationID  = nameof(clsLocationDto.LocationID);
            public const string Name        = nameof(clsLocationDto.Name);
            public const string Address     = nameof(clsLocationDto.Address);
            public const string Phone       = nameof(clsLocationDto.Phone);
            public const string IsActive    = nameof(clsLocationDto.IsActive);
            
        }

        private enum enFilter
        {
            LocationID   ,
            Name         ,
            Address      ,
            Phone        ,
            IsActive     
        }
        private enum enIsActive { Active , UnActive}

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsLocationService _Locationservice;

        private int _currentPage = 1;
        private int _totalPages = 0;

  
        public event Action DataRefreshed;

        public ctrlLocations(frmMain frmMain)
        {
            InitializeComponent();
            _Locationservice = new clsLocationService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlLocations_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlLocations.ctrlLocations_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditLocation frm = new frmAddEditLocation(_Locationservice))
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
                clsEventLogger.LogException("ctrlLocations.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedLocationId(out int LocationId))
                return;

            try
            {
                using (frmAddEditLocation frm = new frmAddEditLocation(_Locationservice,LocationId))
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
                clsEventLogger.LogException("ctrlLocations.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedLocationId(out int LocationId))
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

                var result = await _Locationservice.DeleteAsync(LocationId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف الموقع الذي يحمل الرقم التعريفي '{LocationId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف الموقع");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlLocations.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlLocations.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlLocations.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlLocations.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlLocations.btnSearch_Click", ex);
                clsMessages.ShowError();
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
                clsEventLogger.LogException("ctrlLocations.cbYesOrNo_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsActive)
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
                clsEventLogger.LogException("ctrlLocations.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlLocations.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlLocations.btnNext_Click", ex);
                clsMessages.ShowError();
            }
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
                    clsEventLogger.LogException("ctrlLocations.LoadDataAsync", ex);
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
                var result = await _Locationservice.GetLocationsPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListLocations.DataSource = null;
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

                dgvListLocations.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListLocations.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlLocations._ApplyPagingAsync", ex);
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
            if (dgvListLocations.DataSource == null || dgvListLocations.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.LocationID           , "المعرف" );
            _SetColumnHeader(Columns.Name       	  	  , "الاسم"   );
            _SetColumnHeader(Columns.Address     	  	  , "العنوان");
            _SetColumnHeader(Columns.Phone    	  	      , "الهاتف" );
            _SetColumnHeader(Columns.IsActive    	  	  , "نشط ؟"  );

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListLocations.Columns.Contains(columnName))
                dgvListLocations.Columns[columnName].HeaderText = headerText;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListLocations.Rows.Count == 0;
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
            var data = dgvListLocations.DataSource as List<clsLocationDto>;
            
            if (data == null || data.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "المواقع");
        }
        private DataTable _CreateOptimizedExportData(List<clsLocationDto> source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"   ,typeof(int));
            exportTable.Columns.Add("الاسم"     , typeof(string));
            exportTable.Columns.Add("العنوان"  , typeof(string));
            exportTable.Columns.Add("الهاتف"   , typeof(string));
            exportTable.Columns.Add("نشط ؟"    , typeof(bool));
          
            exportTable.BeginLoadData();

            foreach (var item in source)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"  ] = item.LocationID;
                newRow["الاسم"    ] = item.Name;
                newRow["العنوان" ] = item.Address;
                newRow["الهاتف"  ] = item.Phone;
                newRow["نشط ؟"   ] = item.IsActive;

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedLocationId(out int LocationId)
        {
            LocationId = 0;
            if (dgvListLocations.CurrentRow == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            if (dgvListLocations.CurrentRow.DataBoundItem is clsLocationDto Location)
            {
                LocationId = Location.LocationID;
                return true;
            }
            return false;
        }
        private async Task<bool> _CheckDatabaseConnection()
        {
            return await clsUtil.CheckDatabaseConnection();
        }
        private string _GetFilterValue()
        {
            if (_currentColumn == Columns.IsActive)
                return cbYesOrNo.SelectedIndex == (int)enIsActive.Active ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.LocationID: return Columns.LocationID;
                case enFilter.Name: return Columns.Name;
                case enFilter.Address: return Columns.Address;
                case enFilter.Phone: return Columns.Phone;
                case enFilter.IsActive: return Columns.IsActive;
                default: throw new ArgumentOutOfRangeException();
            }
        }

       
    }
}