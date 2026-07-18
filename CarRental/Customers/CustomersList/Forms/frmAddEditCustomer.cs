using CarRental.Helper;
using CarRental.Properties;
using CarRental_Buisness.Models.Customers;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Customers;
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

namespace CarRental.Customers.CustomersList.Forms
{
    public partial class frmAddEditCustomer : Form
    {
        private enum enMode { AddNew  , Update}

        private readonly enMode _mode;

        private readonly int? _customerId;

        private readonly clsCustomerService _customerService;

        private readonly Dictionary<string, Control> _validationControls;
        private readonly HashSet<Control> _editableControls;

        private bool _isSaving;
        public frmAddEditCustomer(clsCustomerService customerService,int? customerId=null)
        {
            InitializeComponent();



            _customerService = customerService;
            _customerId = customerId;

            if(customerId.HasValue)
                _mode = enMode.Update;
            else
                _mode = enMode.AddNew;

            if(_mode == enMode.AddNew)
            {
                ctrPersonCardWithFilter1.ExistsValidator = async personId =>
                {
                    var result = await _customerService.ExistsByPersonIDAsync(personId);

                    if (result.Success)
                        clsMessages.ShowInfo("هذا الشخص مسجل بالفعل كعميل");

                    return result.Success;
                };
            }

            _validationControls = new Dictionary<string, Control>
            {
                ["CustomerID"] = lblCustomerID,
                ["DriverLicenseNumber"] = txtDriverLicense,
                ["DriverLicenseExpiry"] = dtpLicenseExpiry
            };

            _editableControls = new HashSet<Control>
            {
                dtpLicenseExpiry
            };
        }

        private async void frmAddEditCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                _InitializeForm();

                if(_mode == enMode.Update)
                {
                    if (!await _LoadCustomerDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditCustomer.frmAddEditCustomer_Load", ex);

                clsMessages.ShowError();
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

                if (_mode == enMode.Update)
                    await _SaveUpdateAsync();
                else
                    await _SaveAddNewAsync();

            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditCustomer.btnSave_Click", ex);
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
            this.Close();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(ctrPersonCardWithFilter1.SelectedPerson == null)
            {
                clsMessages.ShowInfo("يرجى اختيار الشخص أولا");
                return;
            }

            _ClearErrors();
            tabControl1.SelectedTab = tabPage2;
        }

        // ============= METHODS =============

        private void _InitializeForm()
        {
            if (_mode == enMode.AddNew)
            {
                this.Text = "إضافة عميل";
                pictureBox1.Image = Resources.AddPerson_512;

                dtpLicenseExpiry.MinDate = DateTime.Today.Date;
            }
            else
            {
                this.Text = "تعديل عميل";
                pictureBox1.Image = Resources.editPerson_512;
            }
        }
        private async Task<bool> _LoadCustomerDataAsync()
        {
            if (!_customerId.HasValue)
                throw new InvalidOperationException("معرف العميل غير معروف");

            var result = await _customerService.GetCustomerByCustomerIDAsync(_customerId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError($"العميل الذي يحمل الرقم التعريفي ({_customerId}) غير موجود");

                Close();
                return false;
            }

            var customer = result.Data;

            if (!await ctrPersonCardWithFilter1.SearchByPersonIDAsync(customer.PersonID))
            {
                clsMessages.ShowError("تعذر تحميل بيانات الشخص");
                Close();
                return false;
            }

            ctrPersonCardWithFilter1.SearchPanelEnabled = false;

            lblCustomerID.Text = customer.CustomerID.ToString();
            txtDriverLicense.Text = customer.DriverLicenseNumber;
            dtpLicenseExpiry.Value = customer.DriverLicenseExpiry;

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            var model = _BuildAddNewModel();

            var result = await _customerService.AddNewAsync(model);

            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة عميل جديد بنجاح. الرقم التعريفي: {result.Data.CustomerID}");

            lblCustomerID.Text = result.Data.CustomerID.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            

            var model = _BuildUpdateModel();

            var result = await _customerService.UpdateAsync(_customerId.Value, model);

            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات العميل بنجاح. الرقم التعريفي: {result.Data.CustomerID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsCustomerAddNewModel _BuildAddNewModel()
        {
            var person = ctrPersonCardWithFilter1.SelectedPerson;

            if (person == null)
                throw new InvalidOperationException("يجب اختيار الشخص");

            return new clsCustomerAddNewModel
            {
                PersonID = person.PersonID,
                DriverLicenseNumber = txtDriverLicense.Text.Trim(),
                DriverLicenseExpiry = dtpLicenseExpiry.Value
            };
        }
        private clsCustomerUpdateModel _BuildUpdateModel()
        {
            return new clsCustomerUpdateModel
            {
                DriverLicenseExpiry = dtpLicenseExpiry.Value
            };
        }
        private void _ClearErrors()
        {
            errorProvider1.Clear();
        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearErrors();

            if (validation == null)
                return;

            foreach(var error in validation.Errors)
            {
                if(_validationControls.TryGetValue(error.FieldName, out var control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
            }
        }
        private void _DisableReadOnlyFields()
        {
            foreach(Control control in pnlContainer.Controls)
            {
                if (control is Label)
                    continue;

                control.Enabled = _editableControls.Contains(control);
            }
        }

    }
}
