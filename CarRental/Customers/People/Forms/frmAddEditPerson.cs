using CarRental.Properties;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.People;
using CarRental.Helper;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRental_Buisness.Helpers;

namespace CarRental.Customers.People.Forms
{
    public partial class frmAddEditPerson : Form
    {
        private enum enMode
        {
            AddNew,
            Update
        }

        private readonly clsPersonService _personService;

        private readonly int? _personID;
        private readonly enMode _mode;

        private readonly clsPersonAddNewModel _addModel;
        private readonly clsPersonUpdateModel _updateModel;

        private readonly Dictionary<string, Control> _validationControls;
        private readonly HashSet<Control> _editableControls;

        private bool _isSaving;

        public frmAddEditPerson(clsPersonService personService,int? personID = null)
        {
            InitializeComponent();

            _personService = personService;

            _personID = personID;

            if (personID.HasValue)
            {
                _mode = enMode.Update;
                _updateModel = new clsPersonUpdateModel();
            }
            else
            {
                _mode = enMode.AddNew;
                _addModel = new clsPersonAddNewModel();
            }

            _validationControls = new Dictionary<string, Control>
            {
                ["NationalNo"] = numericTextBoxNationalNo,
                ["BirthDate"] = dateTimePicker1,
                ["FirstName"] = txtFirstName,
                ["SecondName"] = txtSecondName,
                ["ThirdName"] = txtThirdName,
                ["LastName"] = txtLastName,
                ["Email"] = txtEmail,
                ["Phone"] = numericTextBoxPhone,
                ["Address"] = txtAddress
            };

            _editableControls = new HashSet<Control>
            {
                txtAddress,
                txtEmail,
                numericTextBoxPhone
            };
        }

        private async void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            try
            {
                _InitializeForm();

                if (_mode == enMode.Update)
                {
                    if (!await _LoadPersonDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError(ex.Message);
                Close();
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

                _ClearErrors();

                if (_mode == enMode.AddNew)
                    await _SaveAddNewAsync();
                else
                    await _SaveUpdateAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditPerson.btnSave_Click", ex);
                clsMessages.ShowError(ex.Message);
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

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
                lblGenderImage.Image = Resources.male_32;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
                lblGenderImage.Image = Resources.female32;
        }

        // =================== METHODS ==========================

        private void _InitializeForm()
        {
            pictureBox1.Image = (_mode == enMode.Update) ? Resources.editPerson_512 : Resources.AddPerson_512;
            this.Text = (_mode == enMode.Update) ? "تعديل شخص" : "إضافة شخص";
            rbMale.Checked = true;
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
        }
        private async Task<bool> _LoadPersonDataAsync()
        {
            if (!_personID.HasValue)
                return false ;

            var result = await _personService.GetPersonByIDAsync(_personID.Value);

            if (!result.Success)
            {
                clsMessages.ShowError($"الشخص الذي يحمل الرقم التعريفي ({_personID}) غير موجود");

                Close();
                return false;
            }

            var person = result.Data;

            numericTextBoxNationalNo.Text = person.NationalNo;
            txtFirstName.Text = person.FirstName;
            txtSecondName.Text = person.SecondName;
            txtThirdName.Text = person.ThirdName;
            txtLastName.Text = person.LastName;
            dateTimePicker1.Value = person.BirthDate;
            numericTextBoxPhone.Text = person.Phone ?? string.Empty;
            txtEmail.Text = person.Email ?? string.Empty;
            txtAddress.Text = person.Address ?? string.Empty;

            rbMale.Checked = person.Gender == enGenderType.Male;
            rbFemale.Checked = person.Gender == enGenderType.Female;

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            _FillAddNewModel();

            var result = await _personService.AddNewAsync(_addModel);

            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess( $"تمت إضافة شخص جديد بنجاح. الرقم التعريفي: {result.Data.PersonID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            _FillUpdateModel();

            var result =await _personService.UpdateAsync(_personID.Value, _updateModel);

            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess( $"تم تعديل بيانات الشخص بنجاح. الرقم التعريفي: {result.Data.PersonID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private void _FillAddNewModel()
        {
            _addModel.NationalNo = clsUtil.NullIfEmpty(numericTextBoxNationalNo.Text);
            _addModel.FirstName = clsUtil.NullIfEmpty(txtFirstName.Text);
            _addModel.SecondName = clsUtil.NullIfEmpty(txtSecondName.Text);
            _addModel.ThirdName = clsUtil.NullIfEmpty(txtThirdName.Text);
            _addModel.LastName = clsUtil.NullIfEmpty(txtLastName.Text);

            _addModel.BirthDate = dateTimePicker1.Value;

            _addModel.Gender =rbMale.Checked ? enGenderType.Male : enGenderType.Female;

            _addModel.Phone = clsUtil.NullIfEmpty(numericTextBoxPhone.Text);
            _addModel.Email = clsUtil.NullIfEmpty(txtEmail.Text);
            _addModel.Address = clsUtil.NullIfEmpty(txtAddress.Text);
        }
        private void _FillUpdateModel()
        {
            _updateModel.Phone = clsUtil.NullIfEmpty(numericTextBoxPhone.Text);
            _updateModel.Email = clsUtil.NullIfEmpty(txtEmail.Text);
            _updateModel.Address = clsUtil.NullIfEmpty(txtAddress.Text);
        }
        private void _ClearErrors()
        {
            errorProvider1.Clear();
        }
        private void _ApplyValidationErrors( clsValidationResult validation)
        {
            _ClearErrors();

            if (validation == null)
                return;

            foreach (var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName,out Control control))
                {
                    errorProvider1.SetError(control,error.Message);
                }
            }
        }
        private void _DisableReadOnlyFields()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label)
                    continue;
                
                control.Enabled = _editableControls.Contains(control);
            }
        }
    }
}