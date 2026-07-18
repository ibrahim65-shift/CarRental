using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.VehicleDamage;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.VehicleDamage;
using CarRental_Buisness.Services.WorkFlow;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehicleDamage.Forms
{
    public partial class frmAddEditVehicleDamage : Form
    {
        private enum enMode { AddNew , Update}

        private readonly clsVehicleDamageService _vehicleDamageService;
        private readonly clsVehicleDamageWorkflowService _damageWorkflowService;
        private readonly Dictionary<string, Control> _validationControls;
        private readonly HashSet<Control> _editControls;

        private enMode _mode;
        private int? _damageId;
        private int _vehicleId;
        private int? _bookingId;

        private bool _isSaving;
        public frmAddEditVehicleDamage(int vehicleId, int? bookingId = null, int? damageId=null)
        {
            InitializeComponent();

            _vehicleDamageService = new clsVehicleDamageService();
            _damageWorkflowService = new clsVehicleDamageWorkflowService(_vehicleDamageService);
            _damageId = damageId;
            _vehicleId = vehicleId;
            _bookingId = bookingId;

            if(_damageId.HasValue)
            {
                this.Text = "تعديل ضرر للمركبة";
                _mode = enMode.Update;

                _editControls = new HashSet<Control>
                {
                    txtDescription,numericUpDownEstimatedCost
                };
            }
            else
            {
                this.Text = "إضافة ضرر للمركبة";
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>
            {
                {"VehicleID" , lblVehicleID },
                {"Description" , txtDescription },
                {"EstimatedCost" , numericUpDownEstimatedCost },
                {"BookingID" , pnlContainBookingID },
            };

        }

        private async void frmAddEditVehicleDamage_Load(object sender, EventArgs e)
        {
            try
            {
                _InitializeForm();

                if (_mode == enMode.Update)
                {
                    if (!await _LoadVehicleDamageDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicleDamage.frmAddEditVehicleDamage_Load", ex);
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

                _ClearAllErrors();

                if (_mode == enMode.AddNew)
                   await _SaveAddNewAsync();
                else
                    await _SaveUpdateAsync();

            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicleDamage.btnSave_Click", ex);
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

        // =============== METHODS ===========

        private void _InitializeForm()
        {
            pnlContainBookingID.Visible = _bookingId.HasValue;
            lblBookingID.Text = _bookingId.HasValue ? _bookingId.Value.ToString() : "????";
            lblVehicleID.Text = _vehicleId.ToString();
        }
        private async Task<bool> _LoadVehicleDamageDataAsync()
        {
            if (!_damageId.HasValue)
                throw new InvalidOperationException("معرف الضرر غير معروف");

            var result = await _vehicleDamageService.GetVehicleDamageByIDAsync(_damageId.Value);
            if (!result.Success)
            {
                clsMessages.ShowError($"الضرر الذي يحمل الرقم التعريفي ({_damageId}) غير موجود");
                Close();
                return false;
            }

            var vehicleDamageData = result.Data;

            lblVehicleID.Text = vehicleDamageData.VehicleID.ToString();
            lblBookingID.Text = vehicleDamageData.BookingID.HasValue ? vehicleDamageData.BookingID.Value.ToString() : "????";
            numericUpDownEstimatedCost.Value = vehicleDamageData.EstimatedCost ?? 0;
            txtDescription.Text = clsUtil.NullIfEmpty(vehicleDamageData.Description);

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _damageWorkflowService.CreateVehicleDamageAsync(_BuildAddNewModel(),clsUiHelper.applicationSettings);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة ضرر جديد للمركبة بنجاح. الرقم التعريفي: {result.Data.DamageID}");


            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var result = await _damageWorkflowService.UpdateVehicleDamageAsync(_damageId.Value, _BuildUpdateModel(), clsUiHelper.applicationSettings);
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات الضرر للمركبة بنجاح. الرقم التعريفي: {_damageId}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsVehicleDamageAddNewModel _BuildAddNewModel()
        {
            var addNewModel = new clsVehicleDamageAddNewModel();

            addNewModel.VehicleID = _vehicleId;
            addNewModel.BookingID = _bookingId;
            addNewModel.EstimatedCost = numericUpDownEstimatedCost.Value;
            addNewModel.Description = clsUtil.NullIfEmpty(txtDescription.Text);

            return addNewModel;
        }
        private clsVehicleDamageUpdateModel _BuildUpdateModel()
        {
            var updateModel = new clsVehicleDamageUpdateModel();

            updateModel.EstimatedCost = numericUpDownEstimatedCost.Value;
            updateModel.Description = clsUtil.NullIfEmpty(txtDescription.Text);

            return updateModel; 
        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();
        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation?.Errors == null)
                return;

            foreach(var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
                else
                    clsMessages.ShowError(error.Message);
            }
    }
        private void _DisableReadOnlyFields()
        {
            foreach(Control ctrl in gbVehicleDamageInfo.Controls)
            {
                if (ctrl is Label)
                    continue;

                ctrl.Enabled = _editControls.Contains(ctrl);
            }
        }
    }
}
