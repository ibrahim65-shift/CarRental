using CarRental.Helper;
using CarRental_Buisness.Models.Permissions.Permission;
using CarRental_Buisness.Models.Permissions.RolePermission;
using CarRental_Buisness.Services.Permissions.Permission;
using CarRental_Buisness.Services.Permissions.Role;
using CarRental_Buisness.Services.Permissions.RolePermission;
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

namespace CarRental.Permissions.RolePermissions.Forms
{
    public partial class frmAddEditRolePermissions : Form
    {

        private static class dgvColumns
        {
            public static string PermissionID = "PermissionID";
            public static string PermissionName = "PermissionName";
            public static string IsChecked = "IsChecked";
            public static string IsAllowed = "IsAllowed";
        }

        private enum enMode { AddNew  , Update}

        private readonly clsRolePermissionService _rolePermissionsService;
        private readonly clsRoleService _roleService;
        private readonly clsPermissionService _permissionService;

        private List<clsPermissionDto> _allPermissions = new List<clsPermissionDto>();
        private Dictionary<int,DataGridViewRow> _permissionRows = new Dictionary<int, DataGridViewRow>();

        private enMode _mode;
        private int? _rolePermissionId;
        private bool _isSaving;

        private int? _SelectedRoleId
        {
            get
            {
                if (cbRoles.SelectedValue != null && int.TryParse(cbRoles.SelectedValue.ToString(), out int roleId))
                    return roleId;
                else
                    return null;
            }
        }

        public frmAddEditRolePermissions(clsRolePermissionService rolePermissionService , int? rolePermissionId = null)
        {
            InitializeComponent();

            _rolePermissionsService = rolePermissionService;
            _rolePermissionId = rolePermissionId;

            _roleService = new clsRoleService();
            _permissionService = new clsPermissionService();

            _mode = rolePermissionId.HasValue ? enMode.Update : enMode.AddNew;
            this.Text = _mode == enMode.Update ? "تعديل صلاحيات الدور" : "إضافة صلاحيات للدور";
        }

        private async void frmAddEditRolePermissions_Load(object sender, EventArgs e)
        {
            try
            {
                await _FillRolesComboBoxAsync();
                await _FillPermissionsDataGridViewAsync();

                if (cbRoles.SelectedValue != null && int.TryParse(cbRoles.SelectedValue.ToString(), out int roleId))
                    await _DeterminePermissionsForRoleAsync(roleId);

                if (_mode == enMode.Update)
                    await _LoadRolePermissionsDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRolePermissions.frmAddEditRolePermissions_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _SaveRolePermissionsAsync();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRolePermissions.frmAddEditRolePermissions_Load", ex);
                clsMessages.ShowError();
            }
            finally
            {
                _isSaving = false;
                btnSave.Enabled = true;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnSearch_Click(object sender, EventArgs e) => _FilterData(txtSearch.Text.Trim());
        private async void cbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoles.SelectedValue != null && int.TryParse(cbRoles.SelectedValue.ToString(), out int roleId))
                await _DeterminePermissionsForRoleAsync(roleId);
        }
        private void txtSearch_TextChanged(object sender, EventArgs e) => _FilterData(txtSearch.Text.Trim());
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                _FilterData(txtSearch.Text.Trim());
            }
        }

        // ==================== METHODS ==============

        private async Task _SaveRolePermissionsAsync()
        {
            if (!_SelectedRoleId.HasValue)
            {
                clsMessages.ShowError("الرجاء اختيار الدور أولا");
                return;
            }

            var permissions = _MapUIToRolePermissions();
            if (permissions == null)
                return;

            var result = await _rolePermissionsService.SaveRolePermissionsAsync(_SelectedRoleId.Value, permissions);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess(_mode == enMode.AddNew ? "تم حفظ الصلاحيات للدور بنجاح" : "تم تعديل الصلاحيات للدور بنجاح");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _FillRolesComboBoxAsync()
        {
            await clsUiHelper.FillComboBoxGenericAsync(cbRoles, () => _roleService.GetAllAsync(), "RoleName", "RoleID");
        }
        private async Task _FillPermissionsDataGridViewAsync()
        {
            var result = await _permissionService.GetAllAsync();
            if(!result.Success)
            {
                clsMessages.ShowError("حدث خطأ أثناء تحميل بيانات الصلاحيات");
                Close();
                return;
            }

            _allPermissions.Clear();
            _allPermissions = result.Data;

            dataGridView1.Rows.Clear();
            _permissionRows.Clear();


            foreach (var perm in _allPermissions)
            {
                int index = dataGridView1.Rows.Add(perm.PermissionID, perm.PermissionName, false, false);

                _permissionRows[perm.PermissionID] = dataGridView1.Rows[index];
            }
        }
        private async Task _DeterminePermissionsForRoleAsync(int roleID)
        {
            var result = await _rolePermissionsService.GetPermissionsForRoleAsync(roleID);
            if(!result.Success)
            {
                clsMessages.ShowError("حدث خطأ أثناء تحميل الصلاحيات للدور");
                return;
            }

            var rolePermissions = result.Data;

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[dgvColumns.IsChecked].Value = false;
                row.Cells[dgvColumns.IsAllowed].Value = false;
            }

            foreach(var permission in rolePermissions)
            {
                if(_permissionRows.TryGetValue(permission.PermissionID , out DataGridViewRow row))
                {
                    row.Cells[dgvColumns.IsChecked].Value = true;
                    row.Cells[dgvColumns.IsAllowed].Value = permission.IsAllowed;
                }
            }
        }
        private List<clsRolePermissionItem> _MapUIToRolePermissions()
        {
            if(!_SelectedRoleId.HasValue)
            {
                clsMessages.ShowError("الرجاء اختيار الدور أولا");
                return null;
            }

            dataGridView1.EndEdit();

            var permissionList = new List<clsRolePermissionItem>();

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = row.Cells[dgvColumns.IsChecked].Value as bool? ?? false;
                if (!isChecked)
                    continue;

                permissionList.Add(new clsRolePermissionItem((int)row.Cells[dgvColumns.PermissionID].Value, 
                    row.Cells[dgvColumns.IsAllowed].Value as bool? ?? false));
            }

            if (permissionList.Count == 0)
            {
                clsMessages.ShowError("يجب اختيار صلاحية واحدة على الأقل");
                return null;
            }

            return permissionList;
        }
        private async Task _LoadRolePermissionsDataAsync()
        {
            if (!_rolePermissionId.HasValue)
                throw new InvalidOperationException("معرف صلاحيات الدور غير معروف");

            var result = await _rolePermissionsService.GetRolePermissionsByIDAsync(_rolePermissionId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError("تعذر تحميل بيانات الصلاحية المحددة");
                Close();
                return;
            }
  
            cbRoles.SelectedValue = result.Data.RoleID;

            dataGridView1.Columns[dgvColumns.IsChecked].ReadOnly = true;

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = row.Cells[dgvColumns.IsChecked].Value as bool? ?? false;
                row.Cells[dgvColumns.IsAllowed].ReadOnly = !isChecked;
            }
        }
        private void _FilterData(string searchText)
        {
            if(string.IsNullOrWhiteSpace(searchText))
            {
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Visible = true;
                }
                return;
            }

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                string permissionName = row.Cells[dgvColumns.PermissionName].Value?.ToString();

                row.Visible = permissionName?.IndexOf(searchText,StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
    }
}
