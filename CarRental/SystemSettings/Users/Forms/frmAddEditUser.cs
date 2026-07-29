using CarRental.Helper;
using CarRental.Properties;
using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Permissions.Role;
using CarRental_Buisness.Services.Users;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.SystemSettings.Users.Forms
{
    public partial class frmAddEditUser : Form
    {
        private enum enMode { AddNew , Update}

        private readonly clsUserService _userService;
        private readonly Dictionary<string, Control> _validationControls;
        private readonly HashSet<Control> _editControls;

        private readonly clsRoleService _roleService;

        private enMode _mode;
        private int? _userId;
        private bool _isSaving;

        private int? SelectedRoleID
        {
            get
            {
                if (cbRoles.SelectedValue != null && int.TryParse(cbRoles.SelectedValue.ToString(), out int roleId))
                    return roleId;
                else
                    return null;
            }
        }
        public frmAddEditUser(clsUserService userService , int? userId = null)
        {
            InitializeComponent();

            _userService = userService;
            _userId = userId;

            _roleService = new clsRoleService();

            _mode = _userId.HasValue ? enMode.Update : enMode.AddNew;
            this.Text = _mode == enMode.Update ? "تعديل مستخدم" : "إضافة مستخدم";

            _validationControls = new Dictionary<string, Control>
            {
                {"UserName" , txtUserName },
                {"Password" , txtPassword },
                {"RoleID" , cbRoles }
            };

            if (_mode == enMode.AddNew)
            {
                ctrPersonCardWithFilter1.ExistsValidator = async personId =>
                {
                    var result = await _userService.ExistsByPersonIDAsync(personId);

                    if (result.Success)
                        clsMessages.ShowInfo("هذا الشخص مسجل بالفعل كمستخدم");

                    return result.Success;
                };
            }

            if (_mode == enMode.Update)
            {
                _editControls = new HashSet<Control>
                {
                    cbRoles , txtUserName , chkIsActive
                };
            }

            pictureBox1.Image = _mode == enMode.AddNew ? Resources.AddPerson_512 : Resources.editPerson_512;
            chkIsActive.Visible = _mode == enMode.Update;
        }

        private async void frmAddEditUser_Load(object sender, EventArgs e)
        {
            try
            {
                await _FillComboBoxRolesAsync();

                if(_mode ==enMode.Update)
                {
                    if (!await _LoadUserDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditUser.frmAddEditUser_Load", ex);
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
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditUser.btnSave_Click", ex);
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
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
        private async void btnNext_Click(object sender, EventArgs e)
        {
            var person = ctrPersonCardWithFilter1.SelectedPerson;
            if (person == null)
            {
                clsMessages.ShowInfo("يرجى اختيار الشخص أولاً");
                return;
            }

            var result = await _userService.GetUserByPersonIDAsync(person.PersonID);
            if (result.Success && result.Data.IsDeleted)
            {

                if (MessageBox.Show("يوجد حساب محذوف لهذا الشخص , هل تريد إعادة تفعيله ؟", "مستخدم",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                var restoreResult = await _userService.RestoreUserAsync(result.Data.UserID);

                if (!restoreResult.Success)
                {
                    clsMessages.ShowError(restoreResult.ErrorMessage);
                    return;
                }

                clsMessages.ShowSuccess("تمت إعادة التفعيل بنجاح");

                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            _ClearAllErrors();
            tabControl1.SelectedTab = tabPage2;
            txtUserName.Focus();
        }

        // =============== METHODS ================

        private async Task _FillComboBoxRolesAsync()
        {
            await clsUiHelper.FillComboBoxGenericAsync(cbRoles,() => _roleService.GetAllAsync(), "RoleName", "RoleID");
        }
        private async Task<bool> _LoadUserDataAsync()
        {
            if (!_userId.HasValue)
                throw new InvalidOperationException("معرف المستخدم غير معروف");

            var result = await _userService.GetUserByUserIDAsync(_userId.Value);
            if (!result.Success)
            {
                clsMessages.ShowError($"المستخدم الذي يحمل الرقم التعريفي ({_userId}) غير موجود");

                Close();
                return false;
            }

            var user = result.Data;

            if (!await ctrPersonCardWithFilter1.SearchByPersonIDAsync(user.PersonID))
            {
                clsMessages.ShowError("تعذر تحميل بيانات الشخص");
                Close();
                return false;
            }

            ctrPersonCardWithFilter1.SearchPanelEnabled = false;

            cbRoles.SelectedValue = user.RoleID;
            txtUserName.Text = user.UserName;
            chkIsActive.Checked = user.IsActive;

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            if (!SelectedRoleID.HasValue)
            {
                clsMessages.ShowError("الرجاء اختيار الدور");
                return;
            }

            var model = _BuildAddNewModel();
            if (model == null)
                return;

            var result = await _userService.AddNewAsync(model);
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة مستخدم جديد بنجاح. الرقم التعريفي: {result.Data.UserID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            if (!SelectedRoleID.HasValue)
            {
                clsMessages.ShowError("الرجاء اختيار الدور");
                return;
            }

            var result = await _userService.UpdateAsync(_userId.Value,_BuildUpdateModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت تعديل بيانات مستخدم بنجاح. الرقم التعريفي: {_userId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsUserAddNewModel _BuildAddNewModel()
        {
            var person = ctrPersonCardWithFilter1.SelectedPerson;

            if (person == null)
            {
                clsMessages.ShowError("يجب اختيار الشخص");
                return null;
            }

            return new clsUserAddNewModel
            {
                PersonID = person.PersonID,
                RoleID = SelectedRoleID.Value,
                UserName = txtUserName.Text.Trim(),
                Password = txtPassword.Text.Trim()
            };

        }
        private clsUserUpdateModel _BuildUpdateModel()
        {
            return new clsUserUpdateModel
            {
                RoleID = SelectedRoleID.Value,
                UserName = txtUserName.Text.Trim(),
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
                else
                    clsMessages.ShowError(error.Message);
            }
        }
        private void _DisableReadOnlyFields()
        {
            foreach(Control ctrl in gbUserInfo.Controls)
            {
                if (ctrl is Label)
                    continue;

                ctrl.Enabled = _editControls.Contains(ctrl);
            }
        }
    }
}
