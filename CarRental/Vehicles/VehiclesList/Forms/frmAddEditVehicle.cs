using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Vehicles;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Vehicles;
using DocumentFormat.OpenXml.Bibliography;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental.Vehicles.VehiclesList.Forms
{
    public partial class frmAddEditVehicle : Form
    {
        private enum enMode { AddNew , Update};

        private  enMode _mode;

        private readonly clsVehicleService _vehicleService;
        private readonly Dictionary<string, Control> _validationControls;
        private readonly HashSet<Control> _editControls;
        private int? _vehicleId;
        private bool _isSaving;
        public frmAddEditVehicle(clsVehicleService vehicleService , int? vehicleId = null)
        {
            InitializeComponent();
            _vehicleService = vehicleService;
            _vehicleId = vehicleId;
            if (vehicleId.HasValue)
            {
                this.Text = "تعديل مركبة";
                _mode = enMode.Update;
                _editControls = new HashSet<Control>
                 {
                     numericUpDownCurrentMileage ,
                     numericUpDownRentalPrice ,
                     cbVehicleStatus , 
                 };
            }
            else
            {
                this.Text = "إضافة مركبة";
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>
            {
                { "MakeID", cbMake},
                {"ModelID",cbModel },
                {"Year",numericUpDownYear },
                {"CurrentMileage",numericUpDownCurrentMileage },
                {"RentalPricePerDay",numericUpDownRentalPrice},
                {"FuelTypeID",cbFuelType },
                {"CategoryID",cbVehicleCategory},
                {"PlateNumber",txtPlateNumber},
                {"VIN",txtVIN},
                {"Color",pnlColorPreview},
                {"StatusID",cbVehicleStatus}
            };

           
        }

        private async void frmAddEditVehicle_Load(object sender, EventArgs e)
        {
            try
            {
                await _InitializeFormAsync();

                if(_mode ==enMode.Update)
                {
                    if (!await _LoadVehicleDataAsync())
                        return;

                    _DisableReadOnlyFields();
                }
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicle.frmAddEditCustomer_Load", ex);

                clsMessages.ShowError();
                Close();
            }
        }
        private async void cbMake_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(cbMake.SelectedValue is int makeId)
            {
                await _FillModelComboboxAsync(makeId);
            }
            //if (cbMake.SelectedValue == null)
            //    return;

            //int makeId = (int)cbMake.SelectedValue;
            //await _FillModelComboboxAsync(makeId);
        }
        private void btnChooseColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pnlColorPreview.BackColor = colorDialog1.Color;
                txtColor.Text = colorDialog1.Color.IsKnownColor ? colorDialog1.Color.Name : "Custom Color";
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
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditVehicle.btnSave_Click", ex);
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

        // ============= METHODS ==============

        private async Task _InitializeFormAsync()
        {
            if(_mode == enMode.Update)
            {
                lblVehicleStatus.Visible = true;
                cbVehicleStatus.Visible = true;
                clsUiHelper.FillComboBoxWithEnumDescriptions<enVehicleStatus>(cbVehicleStatus);
            }

            pnlColorPreview.BackColor = colorDialog1.Color;
            txtColor.Text = ColorTranslator.ToHtml(colorDialog1.Color);

            await _FillMakeComboboxAsync();
            clsUiHelper.FillComboBoxWithEnum<enFuelType>(cbFuelType);
            clsUiHelper.FillComboBoxWithEnum<enVehicleCategory>(cbVehicleCategory);
        }
        private async Task<bool> _LoadVehicleDataAsync()
        {
            if (!_vehicleId.HasValue)
                throw new InvalidOperationException("معرف المركبة غير معروف");

            var result = await _vehicleService.GetVehicleByIDAsync(_vehicleId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError($"المركبة التي تحمل الرقم التعريفي ({_vehicleId}) غير موجودة");
                Close();
                return false;
            }

            var vehicle = result.Data;

            var colorInfo = clsUiHelper.Deserialize(vehicle.Color);

            cbMake.SelectedValue = vehicle.MakeID;
            await _FillModelComboboxAsync(vehicle.MakeID);
            cbModel.SelectedValue = vehicle.ModelID;
            numericUpDownYear.Value = vehicle.Year;
            numericUpDownCurrentMileage.Value = vehicle.CurrentMileage;
            numericUpDownRentalPrice.Value = vehicle.RentalPricePerDay;
            colorDialog1.Color = colorInfo.Color;
            pnlColorPreview.BackColor = colorInfo.Color;
            txtColor.Text = colorInfo.name;
            cbFuelType.SelectedValue = vehicle.FuelTypeID;
            cbVehicleCategory.SelectedValue = vehicle.CategoryID;
            txtPlateNumber.Text = vehicle.PlateNumber;
            txtVIN.Text = vehicle.VIN;
            cbVehicleStatus.SelectedValue = vehicle.StatusID;

            return true;
        }
        private async Task _FillMakeComboboxAsync()
        {
           bool success =  await clsUiHelper.FillComboBoxGenericAsync<clsMakesDto>(cbMake,
                  () =>  _vehicleService.GetAllMakesAsync(), "MakeName", "MakeID");

            if(success)
            {
                if (cbMake.SelectedValue is int makeId)
                {
                    await _FillModelComboboxAsync(makeId);
                }
            }
        }
        private async Task _FillModelComboboxAsync(int makeId)
        {
            await clsUiHelper.FillComboBoxGenericAsync<clsModelsDto>(cbModel,
                  () =>  _vehicleService.GetModelsByMakeIDAsync(makeId), "ModelName", "ModelID");
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _vehicleService.AddNewAsync(_BuildVehicleAddNewModel());

            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة مركبة جديدة بنجاح. الرقم التعريفي: {result.Data.VehicleID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {

            var result = await _vehicleService.UpdateAsync(_vehicleId.Value,_BuildVehicleUpdateModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات المركبة بنجاح. الرقم التعريفي: {result.Data.VehicleID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsVehicleAddNewModel _BuildVehicleAddNewModel()
        {
            var addNewModel = new clsVehicleAddNewModel();


            addNewModel.MakeID = (int)cbMake.SelectedValue;
            addNewModel.ModelID = (int)cbModel.SelectedValue;
            addNewModel.Year = (int)numericUpDownYear.Value;
            addNewModel.CurrentMileage   =  (int) numericUpDownCurrentMileage.Value;
            addNewModel.RentalPricePerDay = numericUpDownRentalPrice.Value;
            addNewModel.FuelTypeID = (enFuelType)cbFuelType.SelectedItem;
            addNewModel.CategoryID = (enVehicleCategory)cbVehicleCategory.SelectedItem;
            addNewModel.PlateNumber = txtPlateNumber.Text.Trim();
            addNewModel.VIN = txtVIN.Text.Trim();
            addNewModel.Color = clsUiHelper.Serialize(txtColor.Text.Trim(), colorDialog1.Color);

            return addNewModel;
        }
        private clsVehicleUpdateModel _BuildVehicleUpdateModel()
        {
            var updateModel = new clsVehicleUpdateModel();

            updateModel.CurrentMileage    =(int) numericUpDownCurrentMileage.Value;
            updateModel.RentalPricePerDay = numericUpDownRentalPrice.Value;
            updateModel.StatusID = (enVehicleStatus)cbVehicleStatus.SelectedValue;

            return updateModel;
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
                if(_validationControls.TryGetValue(error.FieldName , out var control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
            }
        }
        private void _DisableReadOnlyFields()
        {
            foreach(Control ctrl in gbVehicleInfo.Controls)
            {
                if (ctrl is Label)
                    continue;

                ctrl.Enabled = _editControls.Contains(ctrl);
            }
        }

    }
}
