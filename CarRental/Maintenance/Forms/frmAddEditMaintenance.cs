using CarRental.Helper;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Maintenance;
using CarRental_Buisness.Services.WorkFlow;
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

namespace CarRental.Maintenance.Forms
{
    public partial class frmAddEditMaintenance : Form
    {
        enum enMode { AddNew , Update}

        private clsMaintenanceService _maintenanceService;
        private clsMaintenanceWorkflowService _maintenanceWorkflowService;
        private Dictionary<string, Control> _validationControls;
        private HashSet<Control> _editControls;

        private  enMode _mode;

        private int? _maintenanceId;
        private int? _SelectedVehicleId;
        private bool _isSaving;
        public frmAddEditMaintenance(clsMaintenanceService maintenanceService , int? maintenacneId=null)
        {
            InitializeComponent();
            _maintenanceService = maintenanceService;
            _maintenanceWorkflowService = new clsMaintenanceWorkflowService(maintenanceService);

            _maintenanceId = maintenacneId;

            if(_maintenanceId.HasValue)
            {
                this.Text = "تعديل صيانة";
                _mode = enMode.Update;

                _editControls = new HashSet<Control>
                {
                    numericUpDownCost,
                    txtDescription,
                };
            }
            else
            {
                this.Text = "إضافة صيانة";
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>
            {
                {"VehicleID" , lblVehicleID },
                {"Description" , txtDescription },
                {"Vendor" , txtVendor },
                {"Cost" , numericUpDownCost }
            };

        }

        private async void frmAddEditMaintenance_Load(object sender, EventArgs e)
        {
            try
            {
                if(_mode == enMode.Update)
                {
                    if (await _LoadMaintenanceDataAsync())
                        _DisableReadOnlyFields();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditMaintenance.frmAddEditMaintenance_Load", ex);
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
                clsEventLogger.LogException("frmAddEditMaintenance.btnSave_Click", ex);
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
        private void linkLabelSearchOnVehicle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using(frmSelectVehicle frm = new frmSelectVehicle())
            {
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    _SelectedVehicleId = frm.SelectedvehicleId;
                    lblVehicleID.Text = _SelectedVehicleId.ToString();
                }
            }
        }

        // ================ METHODES ==============

        private async Task<bool> _LoadMaintenanceDataAsync()
        {
            if (!_maintenanceId.HasValue)
                throw new InvalidOperationException("معرف الصيانة غير معروف");

            var result = await _maintenanceService.GetMaintenanceByMaintenanceIDAsync(_maintenanceId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                Close();
                return false;
            }

            var maintenanceData = result.Data;

            _SelectedVehicleId = maintenanceData.VehicleID;

            lblVehicleID.Text = maintenanceData.VehicleID.ToString();
            txtVendor.Text = maintenanceData.Vendor;
            numericUpDownCost.Value = maintenanceData.Cost;
            txtDescription.Text = maintenanceData.Description;

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _maintenanceWorkflowService.CreateMaintenanceAsync(_BuildMaintenanceAddNewModel(), clsUiHelper.applicationSettings);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم إضافة عملية صيانة بنجاح. للمركبة التي تحمل الرقم التعريفي {_SelectedVehicleId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var result = await _maintenanceWorkflowService.UpdateMaintenanceAsync(_maintenanceId.Value,_BuildMaintenanceUpdateModel(), clsUiHelper.applicationSettings);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تحديث بيانات عملية صيانة بنجاح. للمركبة التي تحمل الرقم التعريفي {_SelectedVehicleId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }

        private clsMaintenanceAddNewModel _BuildMaintenanceAddNewModel()
        {
            return new clsMaintenanceAddNewModel
            {
                VehicleID = _SelectedVehicleId.HasValue ? _SelectedVehicleId.Value : 0,
                Vendor = clsUtil.NullIfEmpty(txtVendor.Text),
                Cost = numericUpDownCost.Value,
                Description = clsUtil.NullIfEmpty(txtDescription.Text)
            };

        }
        private clsMaintenanceUpdateModel _BuildMaintenanceUpdateModel()
        {
            return new clsMaintenanceUpdateModel
            {
                Cost = numericUpDownCost.Value,
                Description = clsUtil.NullIfEmpty(txtDescription.Text)
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

            foreach (var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
                else
                    clsMessages.ShowError(error.Message);
            }
        }
        private void _DisableReadOnlyFields()
        {
            if (_editControls.Count == 0)
                return;

            foreach(Control control in gbMaintenanceInfo.Controls)
            {
                if (control is Label && !(control is LinkLabel))
                    continue;

                control.Enabled = _editControls.Contains(control);
            }
        }
    }
}
