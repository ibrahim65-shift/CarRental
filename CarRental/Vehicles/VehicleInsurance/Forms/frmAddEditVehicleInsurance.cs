using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.VehicleInsurance;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.VehicleInsurance;
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

namespace CarRental.Vehicles.VehicleInsurance.Forms
{
    public partial class frmAddEditVehicleInsurance : Form
    {
        private enum enMode { AddNew , Update}

        private  clsVehicleInsuranceService _vehicleInsuranceService;
        private Dictionary<string, Control> _validationControls;
        private HashSet<Control> _editControls;

        private enMode _mode;
        private int? _vehicleInsuranceId;
        private int _vehicleId;
        private bool _isSaving;
        public frmAddEditVehicleInsurance(clsVehicleInsuranceService vehicleInsuranceService,int vehicleId, int? vehicelInsuranceId=null )
        {
            InitializeComponent();
            _vehicleInsuranceService = vehicleInsuranceService;
            _vehicleId = vehicleId;
            _vehicleInsuranceId = vehicelInsuranceId;

            if(_vehicleInsuranceId.HasValue)
            {
                _mode = enMode.Update;

                _editControls = new HashSet<Control>
                {
                    cbInsuranceType ,
                    dtpStartDate ,
                    dtpEndDate,
                    numericUpDownInsuranceCost,
                    txtNotes,
                };
            }
            else
            {
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>
            {
                {"VehicleID",lblVehicleID },
                {"PolicyNumber",txtPolicyNumber },
                {"ProviderID",cbProviders },
                {"InsuranceTypeID",cbInsuranceType },
                {"StartDate",dtpStartDate },
                {"EndDate",dtpEndDate },
                {"InsuranceCost",numericUpDownInsuranceCost },
                {"Notes",txtNotes }
            };
        }

        private async void frmAddEditVehicleInsurance_Load(object sender, EventArgs e)
        {
            try
            {
                 _InitializeForm();

                if (_mode == enMode.Update)
                {
                    if (!await _LoadVehicleInsuranceDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicleInsurance.frmAddEditVehicleInsurance_Load", ex);

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
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicleInsurance.btnSave_Click", ex);
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


        // ============== METHODS ======================

        private void _InitializeForm()
        {
            lblVehicleID.Text = _vehicleId.ToString();
            clsUiHelper.FillComboBoxWithEnum<enInsuranceProviders>(cbProviders);
            clsUiHelper.FillComboBoxWithEnumDescriptions<enInsuranceType>(cbInsuranceType);

            if(_mode == enMode.AddNew)
            {
                dtpStartDate.MinDate = DateTime.Today;
                dtpEndDate.MinDate = DateTime.Today;
            }
        }
        private async Task<bool> _LoadVehicleInsuranceDataAsync()
        {
            if (!_vehicleInsuranceId.HasValue)
                throw new InvalidOperationException("معرف التأمين غير معروف");

            var result = await _vehicleInsuranceService.GetVehicleInsuranceByIDAsync(_vehicleInsuranceId.Value);
            if (!result.Success)
            {
                clsMessages.ShowError($"التأمين الذي يحمل الرقم التعريفي ({_vehicleInsuranceId}) غير موجود");
                Close();
                return false;
            }

            var insuranceData = result.Data;

            lblVehicleID.Text = insuranceData.VehicleID.ToString();
            txtPolicyNumber.Text = insuranceData.PolicyNumber;
            cbProviders.SelectedValue = insuranceData.ProviderID;
            cbInsuranceType.SelectedValue = insuranceData.InsuranceTypeID;
            dtpStartDate.Value = insuranceData.StartDate;
            dtpEndDate.Value = insuranceData.EndDate;
            numericUpDownInsuranceCost.Value = insuranceData.InsuranceCost;
            txtNotes.Text = insuranceData.Notes;

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _vehicleInsuranceService.AddNewAsync(_BuildAddNewModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة تأمين جديد للمركبة بنجاح. الرقم التعريفي: {result.Data.InsuranceID}");


            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
           
            var result = await _vehicleInsuranceService.UpdateAsync(_vehicleInsuranceId.Value, _BuildUpdateModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات التأمين بنجاح. الرقم التعريفي: {result.Data.InsuranceID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsVehicleInsuranceAddNewModel _BuildAddNewModel()
        {
            var addNewModel = new clsVehicleInsuranceAddNewModel();

            addNewModel.VehicleID = _vehicleId;
            addNewModel.PolicyNumber = txtPolicyNumber.Text.Trim();
            addNewModel.ProviderID = (enInsuranceProviders)cbProviders.SelectedValue;
            addNewModel.InsuranceTypeID = (enInsuranceType)cbInsuranceType.SelectedValue;
            addNewModel.InsuranceCost = numericUpDownInsuranceCost.Value;
            addNewModel.StartDate = dtpStartDate.Value;
            addNewModel.EndDate = dtpEndDate.Value;
            addNewModel.Notes = clsUtil.NullIfEmpty(txtNotes.Text);

            return addNewModel;
        }
        private clsVehicleInsuranceUpdateModel _BuildUpdateModel()
        {
            var updateModel = new clsVehicleInsuranceUpdateModel();

            updateModel.InsuranceTypeID = (enInsuranceType)cbInsuranceType.SelectedValue;
            updateModel.InsuranceCost = numericUpDownInsuranceCost.Value;
            updateModel.StartDate = dtpStartDate.Value;
            updateModel.EndDate = dtpEndDate.Value;
            updateModel.Notes = clsUtil.NullIfEmpty(txtNotes.Text);

            return updateModel;
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
                if(_validationControls.TryGetValue(error.FieldName , out Control control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
            }
        }
        private void _DisableReadOnlyFields()
        {
            foreach(Control ctrl in groupBox1.Controls)
            {
                if (ctrl is Label)
                    continue;

                ctrl.Enabled = _editControls.Contains(ctrl);
            }
        }
    }
}
