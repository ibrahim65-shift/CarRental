using CarRental.Attachments.Forms;
using CarRental.Helper;
using CarRental.Permissions.Roles.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Permissions.Role;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Permissions.Role;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Permissions.Roles.Controls
{
    public partial class ctrlRoles : UserControl,IRefreshable
    {
        private readonly Dictionary<string, string> _columnHeaders = new Dictionary<string, string>
        {
            {"RoleID" ,"المعرف" },
            {"RoleName" ,"الدور" },
            {"Description" ,"الوصف" },
            {"IsActive" ,"نشط ؟" },
           
        };

        private clsRoleService _roleService;
        private frmMain _frmMain;

        public event Action DataRefreshed;
        public ctrlRoles(frmMain main)
        {
            InitializeComponent();
            _frmMain = main;
            _roleService = new clsRoleService();
            cbFilter.SelectedIndex = 0; 
        }

        public async Task RefreshDataAsync()
        {
            await _LoadCurrentFilterAsync();
        }
        private async void ctrlRoles_Load(object sender, EventArgs e)
        {
            await _LoadCurrentFilterAsync();
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using(frmAddEditRole frm = new frmAddEditRole(_roleService , null))
                {
                    if(frm.ShowDialog()==DialogResult.OK)
                    {
                        await RefreshDataAsync();
                        DataRefreshed?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRoles.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, "RoleID", out int roleID))
                return;

            try
            {
                using (frmAddEditRole frm = new frmAddEditRole(_roleService, roleID))
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
                clsEventLogger.LogException("ctrlRoles.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, "RoleID", out int roleID))
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

                var result = await _roleService.DeleteAsync(roleID);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف الدور الذي يحمل الرقم التعريفي '{roleID}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف الدور");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlRoles.btnDelete_Click", ex);
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            _ExportToExcel();
        }
        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            await _LoadCurrentFilterAsync();
        }

        // ================ METHODS =================

        private Task _LoadCurrentFilterAsync()
        {
            return cbFilter.SelectedIndex == 0 ? 
                _LoadDataAsync(() => _roleService.GetAllAsync()) : _LoadDataAsync(() => _roleService.GetActiveRolesAsync());
        }
        private async Task _LoadDataAsync(Func<Task<clsServiceResult<List<clsRoleDto>>>> loader)
        {
           try
           {
                var result = await loader();
                if (!result.Success)
                {
                    dgvListRoles.DataSource = null;
                    _ShowEmptyDataState();
                    return;
                }

                dgvListRoles.DataSource = null;
                dgvListRoles.DataSource = result.Data;
                _InitializeColumns();
                _ShowEmptyDataState();
           }
           catch(Exception ex)
           {
                clsEventLogger.LogException("ctrlRoles._LoadDataAsync", ex);
                _ShowServerErrorState();
            }
        }
        private void _InitializeColumns()
        {
            if (dgvListRoles.DataSource == null || dgvListRoles.Rows.Count == 0)
                return;

            foreach (var col in _columnHeaders)
            {
                _SetColumnHeader(col.Key, col.Value);
            }
        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListRoles.Columns.Contains(columnName))
                dgvListRoles.Columns[columnName].HeaderText = headerText;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListRoles.Rows.Count == 0;

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
            var data = dgvListRoles.DataSource as List<clsRoleDto>;

            if (data == null || data.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var exportData = _CreateExportTable(data);
            clsExcelHelper.Export(_frmMain, exportData, "الأدوار");
        }
        private DataTable _CreateExportTable(List<clsRoleDto> source)
        {
            var exportTable = new DataTable();

            foreach (var column in _columnHeaders)
            {
                exportTable.Columns.Add(column.Value);
            }

            exportTable.BeginLoadData();

            foreach (var role in source)
            {
                exportTable.Rows.Add(role.RoleID , role.RoleName , role.Description , role.IsActive);
            }

            exportTable.EndLoadData();

            return exportTable;
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListRoles.CurrentRow;

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

     
    }
}
