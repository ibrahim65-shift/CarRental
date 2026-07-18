using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Locations;
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

namespace CarRental.SystemSettings.Locations.Forms
{
    public partial class frmAddEditLocation : Form
    {
        enum enMode { AddNew , Update}

        private clsLocationService _locationService;
        private Dictionary<string, Control> _validationControl;

        private enMode _mode;
        private int? _locationId;
        private bool _isSaving;
        public frmAddEditLocation(clsLocationService locationService , int? locationId = null)
        {
            InitializeComponent();
            _locationService = locationService;
            _locationId = locationId;

            if (locationId.HasValue)
                _mode = enMode.Update;
            else
                _mode = enMode.AddNew;

            _validationControl = new Dictionary<string, Control>
            {
                {"Name" , txtName },
                {"Address" , txtAddress },
                {"Phone" , numericTextBoxPhone }
            };
        }

        private async void frmAddEditLocation_Load(object sender, EventArgs e)
        {
            try
            {
                if (_mode == enMode.Update)
                {
                    this.Text = "إضافة موقع";
                    chkIsActive.Visible = true;
                    await _LoadLocationDataAsync();
                }
                else
                {
                    this.Text = "تعديل موقع";
                    chkIsActive.Visible = false;
                }

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditLocation.frmAddEditLocation_Load", ex);
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

                if (_mode == enMode.AddNew)
                    await _SaveAddNewAsync();
                else
                    await _SaveUpdateAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditLocation.frmAddEditLocation_Load", ex);
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

        // ================== METHODES ================

        private async Task _LoadLocationDataAsync()
        {
            if (!_locationId.HasValue)
                throw new InvalidOperationException("معرف الموقع غير معروف");

            var result = await _locationService.GetLocationByIDAsync(_locationId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                Close();
            }

            var locationData = result.Data;

            txtName.Text = locationData.Name;
            txtAddress.Text = locationData.Address;
            numericTextBoxPhone.Text = locationData.Phone;
            chkIsActive.Checked = locationData.IsActive;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _locationService.AddNewAsync(_BuildLocationAddNewModel());
            if(!result.Success)
            {
                if(!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    clsMessages.ShowError(result.ErrorMessage);
                }

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة موقع جديد بنجاح. الرقم التعريفي: {result.Data.LocationID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var result = await _locationService.UpdateAsync(_locationId.Value,_BuildLocationUpdateModel());
            if (!result.Success)
            {
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    clsMessages.ShowError(result.ErrorMessage);
                }

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات الموقع بنجاح. الرقم التعريفي: {_locationId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsLocationAddNewModel _BuildLocationAddNewModel()
        {
            return new clsLocationAddNewModel
            {
                Name = txtName.Text.Trim(),
                Address = clsUtil.NullIfEmpty( txtAddress.Text),
                Phone = clsUtil.NullIfEmpty(numericTextBoxPhone.Text)
            };
        }
        private clsLocationUpdateModel _BuildLocationUpdateModel()
        {
            return new clsLocationUpdateModel
            {
                Name = txtName.Text.Trim(),
                Address = clsUtil.NullIfEmpty(txtAddress.Text),
                Phone = clsUtil.NullIfEmpty(numericTextBoxPhone.Text),
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

            foreach(var error in  validation.Errors)
            {
                if(_validationControl.TryGetValue(error.FieldName , out Control control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
            }
        }

    }
}
