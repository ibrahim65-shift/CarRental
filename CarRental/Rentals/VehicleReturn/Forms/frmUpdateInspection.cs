using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.VehicleReturn;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace CarRental.Rentals.VehicleReturn.Forms
{
    public partial class frmUpdateInspection : Form
    {
        private clsVehicleReturnService _vehicleReturnService;
        private Dictionary<string, Control> _validationControls;

        private int _returnId;
        private int? _StartMileage;
        private bool _isSaving;

        clsUpdateInspectionModel _inspectionInfo;
        public frmUpdateInspection(clsVehicleReturnService vehicleReturnService , int returnId , int? currentMileage ,
            clsUpdateInspectionModel inspectionInfo)
        {
            InitializeComponent();

            _vehicleReturnService = vehicleReturnService;
            _returnId = returnId;
            _StartMileage = currentMileage;
            _inspectionInfo = inspectionInfo;

            _validationControls = new Dictionary<string, Control>
            {
                {"MileageStart" , lblCurrentMileage}, 
                {"MileageEnd" , numericUpDownMileageEnd}, 
                {"FinalCheckNotes" , txtFinalCheckNotes}, 
                {"AdditionalCharges" , numericUpDownAdditionalCharges}, 
            };

        }

        private void frmUpdateInspection_Load(object sender, EventArgs e)
        {
            _InitializeForm();
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _SaveUpdateInspectionAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmUpdateInspection.btnSave_Click", ex);

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


        // ============= METHODES ================

        private void _InitializeForm()
        {
            lblCurrentMileage.Text = _StartMileage.HasValue ? _StartMileage.Value.ToString() : "غير متوفر";
            numericUpDownMileageEnd.Value = _inspectionInfo.MileageEnd.HasValue ? _inspectionInfo.MileageEnd.Value : 0;
            numericUpDownAdditionalCharges.Value = _inspectionInfo.AdditionalCharges.HasValue ? _inspectionInfo.AdditionalCharges.Value : 0;
            txtFinalCheckNotes.Text = _inspectionInfo.FinalCheckNotes;
        }
        private async Task _SaveUpdateInspectionAsync()
        {
            var result = await _vehicleReturnService.UpdateInspectionAsync(_returnId, _BuildUpdateInspectionModel());
            if(!result.Success)
            {
                if(!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تحديث الفحص للمركبة المرجعة التي تحمل الرقم التعريفي {_returnId} بنجاح");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsUpdateInspectionModel _BuildUpdateInspectionModel()
        {
            return new clsUpdateInspectionModel
            {
                MileageEnd = (int)numericUpDownMileageEnd.Value,
                AdditionalCharges = numericUpDownAdditionalCharges.Value,
                FinalCheckNotes = clsUtil.NullIfEmpty(txtFinalCheckNotes.Text),
            };
        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation == null || validation.Errors.Count == 0)
                return;

            foreach(var  error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
            }
        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();
        }

    }
}
