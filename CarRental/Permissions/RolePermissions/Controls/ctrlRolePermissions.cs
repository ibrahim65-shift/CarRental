using CarRental.Helper;
using CarRental_Buisness.Helpers;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRental_Buisness.Services.Permissions.RolePermission;
using System.Collections.Generic;
using CarRental.Permissions.RolePermissions.Forms;

namespace CarRental.Permissions.RolePermissions.Controls
{
    public partial class ctrlRolePermissions : UserControl, IRefreshable
    {

        private readonly Dictionary<string, string> _columnHeaders = new Dictionary<string, string>
        {
            {"RolePermissionID","المعرف" },
            {"RoleName","الدور" },
            {"Description","الوصف" },
            {"PermissionName","الصلاحية" },
            {"PermissionStatus","الحالة" },
        };
        private enum enFilter
        {
            RoleName,
            PermissionName,
            PermissionStatus
        }


        public event Action<int> RolePermissionselectedId;

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsRolePermissionService _rolePermissionsService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        public event Action DataRefreshed;

        public ctrlRolePermissions(frmMain frmMain)
        {
            InitializeComponent();

            clsPermissionHelper.ApplyPermissions(this);

            _rolePermissionsService = new clsRolePermissionService();
            _frmMain = frmMain;
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlRolePermissions_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRolePermissions.ctrlRolePermissions_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(btnAdd.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية إضافة صلاحيات للأدوار");
                return;
            }

            try
            {
                using (frmAddEditRolePermissions frm = new frmAddEditRolePermissions(_rolePermissionsService))
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
                clsEventLogger.LogException("ctrlRolePermissions.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(btnEdit.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية تعديل صلاحيات للأدوار");
                return;
            }

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, "RolePermissionID", out int rolePermissionId))
                return;

            try
            {
                using (frmAddEditRolePermissions frm = new frmAddEditRolePermissions(_rolePermissionsService,rolePermissionId))
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
                clsEventLogger.LogException("ctrlRolePermissions.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(btnExport.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية تصدير بيانات الصلاحيات للأدوار");
                return;
            }

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
                clsEventLogger.LogException("ctrlRolePermissions.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlRolePermissions.btnNext_Click", ex);
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
                    clsEventLogger.LogException("ctrlRolePermissions.LoadDataAsync", ex);
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
                var result = await _rolePermissionsService.GetRolePermissionsPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListRolePermissions.DataSource = null;
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

                dgvListRolePermissions.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListRolePermissions.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRolePermissions._ApplyPagingAsync", ex);
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
            if (dgvListRolePermissions.DataSource == null || dgvListRolePermissions.Rows.Count == 0)
                return;

            foreach(var col in _columnHeaders)
            {
                _SetColumnHeader(col.Key, col.Value);
            }
        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListRolePermissions.Columns.Contains(columnName))
                dgvListRolePermissions.Columns[columnName].HeaderText = headerText;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListRolePermissions.Rows.Count == 0;
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
            var data = dgvListRolePermissions.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "الصلاحيات للأدوار");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            foreach(var col in _columnHeaders)
            {
                exportTable.Columns.Add(col.Value);
            }

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                foreach(var col in _columnHeaders)
                {
                    var value = row[col.Key];
                    newRow[col.Value] = value;
                }

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListRolePermissions.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
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
                case enFilter.RoleName: return nameof(enFilter.RoleName);
                case enFilter.PermissionName: return nameof(enFilter.PermissionName);
                case enFilter.PermissionStatus: return nameof(enFilter.PermissionStatus);
                default: throw new ArgumentOutOfRangeException();
            }
        }

    }
}