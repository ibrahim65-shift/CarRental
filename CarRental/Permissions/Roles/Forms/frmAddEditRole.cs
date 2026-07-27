using CarRental.Helper;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Permissions.Roles.Forms
{
    public partial class frmAddEditRole : Form
    {
        private enum enMode { AddNew , Update}

        private readonly clsRoleService _roleService;
        private readonly Dictionary<string, Control> _validationControls;

        private enMode _mode;

        private int? _roleId;
        private bool _isSaving;
        public frmAddEditRole(clsRoleService roleService , int? roleId = null)
        {
            InitializeComponent();
            _roleService = roleService;
            _roleId = roleId;

            if(roleId.HasValue)
            {
                this.Text = "تعديل الدور";
                _mode = enMode.Update;
            }
            else
            {
                this.Text = "إضافة دور";
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>
            {
                {"RoleName" , txtRoleName}, 
                {"Description" , txtDescription}
            };

        }

        private async void frmAddEditRole_Load(object sender, EventArgs e)
        {
            try
            {
                if (_mode == enMode.Update)
                    await _LoadRoleDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRole.frmAddEditRole_Load", ex);
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

                if (_mode == enMode.AddNew)
                    await _SaveAddNewAsync();
                else
                    await _SaveUpdateAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRole.btnSave_Click", ex);
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

        // ================ METHODS ================

        private async Task _LoadRoleDataAsync()
        {
            if (!_roleId.HasValue)
                throw new InvalidOperationException("معرف الدور غير معروف");

            var result = await _roleService.GetByIDAsync(_roleId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                Close();
            }

            var roleData = result.Data;

            txtRoleName.Text = roleData.RoleName;
            txtDescription.Text = roleData.Description;
            chkIsActive.Checked = roleData.IsActive;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _roleService.AddNewAsync(_BuildRoleCreateUpdateModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة الدور بنجاح. والرقم التعريفي هو {result.Data.RoleID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var result = await _roleService.UpdateAsync(_roleId.Value,_BuildRoleCreateUpdateModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت تعديل بيانات الدور بنجاح. للرقم التعريفي {_roleId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsRoleCreateUpdateModel _BuildRoleCreateUpdateModel()
        {
            return new clsRoleCreateUpdateModel
            {
                RoleName = txtRoleName.Text.Trim(),
                Description = clsUtil.NullIfEmpty(txtDescription.Text),
                IsActive = chkIsActive.Checked
            };

        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();
        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation == null || validation.Errors.Count == 0)
                return;

            foreach(var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
            }
        }
    }
}
