using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Services.Users;
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

namespace CarRental.SystemSettings.Users.Forms
{
    public partial class frmChangePassword : Form
    {
        private readonly clsUserService _userService;

        private int _userId;
        private bool _isSaving;
        public frmChangePassword(clsUserService userService , int userId)
        {
            InitializeComponent();
            _userService = userService;
            _userId = userId;
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _ChangePasswordAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmChangePassword.btnSave_Click", ex);
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

        // ===================== METHODS ==================

        private async Task _ChangePasswordAsync()
        {
            if (!await _ValidateInputsAsync())
                return;

            var result = await _userService.UpdateUsePasswordAsync(_userId,txtNewPassword.Text);

            if (!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess("تم تغيير كلمة المرور بنجاح");

            DialogResult = DialogResult.OK;
            Close();
        }
        private void _ClearErrors()
        {
            errorProvider1.Clear();
        }
        private async Task<bool> _ValidateInputsAsync()
        {
            _ClearErrors();

            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (!_ValidateRequiredFields(currentPassword,newPassword,confirmPassword))
                return false;

            if (!_ValidatePasswordRules(currentPassword,newPassword,confirmPassword))
                return false;

            return await _ValidateCurrentPasswordAsync(currentPassword);
        }
        private bool _ValidateRequiredFields(string currentPassword,string newPassword,string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                errorProvider1.SetError(txtCurrentPassword,"كلمة المرور الحالية مطلوبة");
                return false;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                errorProvider1.SetError(txtNewPassword,"كلمة المرور الجديدة مطلوبة");
                return false;
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                errorProvider1.SetError(txtConfirmPassword,"تأكيد كلمة المرور مطلوب");
                return false;
            }

            return true;
        }
        private bool _ValidatePasswordRules(string currentPassword, string newPassword,string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                errorProvider1.SetError(txtConfirmPassword, "كلمة المرور غير متطابقة");
                return false;
            }

            if (currentPassword == newPassword)
            {
                errorProvider1.SetError(txtNewPassword,"يجب أن تكون كلمة المرور الجديدة مختلفة عن الحالية");
                return false;
            }

            if (!clsUtil.IsValidFormatPassword(newPassword))
            {
                errorProvider1.SetError(txtNewPassword,"يجب أن تكون كلمة المرور 8 أحرف على الأقل وتحتوي على حرف ورقم");
                return false;
            }

            return true;
        }
        private async Task<bool> _ValidateCurrentPasswordAsync(string currentPassword)
        {
            var result = await _userService.VerifyCurrentPasswordAsync(_userId, currentPassword);
            if (result.Success)
                return true;

            errorProvider1.SetError(txtCurrentPassword,"كلمة المرور الحالية غير صحيحة");
            return false;
        }
    }
}
