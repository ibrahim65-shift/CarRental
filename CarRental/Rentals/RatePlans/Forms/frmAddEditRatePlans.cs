using CarRental.Helper;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.RatePlans;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.RatePlans;
using CarRental_Buisness.Services.RentalBooking;
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

namespace CarRental.Rentals.RatePlans.Forms
{
    public partial class frmAddEditRatePlans : Form
    {
        enum enMode { AddNew , Update}

        private clsRatePlansService _ratePlanService;
        private Dictionary<string, Control> _validationControls;

        private enRatePlaneScope SelectedScope => (enRatePlaneScope)cbRatePlanScope.SelectedValue;

        private int? _ratePlanId;
        private int? _selectedVehicleId;
        private bool _isSaving;
        private enMode _mode;
        public frmAddEditRatePlans(clsRatePlansService ratePlansService, int? ratePlanId=null)
        {
            InitializeComponent();
            _ratePlanService = ratePlansService;
            _ratePlanId = ratePlanId;

            if(_ratePlanId.HasValue)
                _mode = enMode.Update;
            else
                _mode = enMode.AddNew;

            _validationControls = new Dictionary<string, Control>
            {
                {"RatePlanScope" ,cbRatePlanScope},
                {"CategoryID" ,cbVehicleCategory},
                {"VehicleID" ,lblVehicleID},
                {"StartDate" ,dtpStartDate},
                {"EndDate" ,dtpEndDate},
                {"Notes" ,txtNotes},
                {"PricePerDay" ,numericUpDownRentalPrice},
            };
        }

        private async void frmAddEditRatePlans_Load(object sender, EventArgs e)
        {
            try
            {
                _InitializeForm();

                if (_mode == enMode.Update)
                    await _LoadRatePlanDataAsync();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRatePlans.frmAddEditRatePlans_Load", ex);

                clsMessages.ShowError();
                Close();
            }
        }
        private void cbRatePlanScope_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _ApplyRulesForRatePlanScope();

            if(SelectedScope == enRatePlaneScope.Category)
            {
                _selectedVehicleId = null;
                lblVehicleID.Text = "????";
            }
            else
            {
                cbVehicleCategory.SelectedIndex = 0;
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
                clsEventLogger.LogException("frmAddEditRatePlans.btnSave_Click", ex);
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
            using (var frm = new frmSelectVehicle())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _selectedVehicleId = frm.SelectedvehicleId;
                    lblVehicleID.Text = _selectedVehicleId.ToString();
                }
            }
        }

        // ================ METHODES =================

        private void _InitializeForm()
        {
            clsUiHelper.FillComboBoxWithEnum<enRatePlaneScope>(cbRatePlanScope);
            clsUiHelper.FillComboBoxWithEnum<enVehicleCategory>(cbVehicleCategory);
        }
        private async Task _LoadRatePlanDataAsync()
        {
            if (!_ratePlanId.HasValue)
                throw new InvalidOperationException("معرف خطة السعر غير معروف");

            var result = await _ratePlanService.GetRatePlansByIDAsync(_ratePlanId.Value);
            if (!result.Success)
            {
                clsMessages.ShowError($"خطة السعر التي تحمل الرقم التعريفي ({_ratePlanId.Value}) غير موجودة");
                Close();
                return ;
            }

            var ratePlanData = result.Data;

            cbRatePlanScope.SelectedValue = ratePlanData.RatePlanScope;
            dtpStartDate.Value = ratePlanData.StartDate;
            dtpEndDate.Value = ratePlanData.EndDate;
            numericUpDownRentalPrice.Value = ratePlanData.PricePerDay;
            numericUpDownMinDays.Value = ratePlanData.MinDays.HasValue ? ratePlanData.MinDays.Value : 0;
            txtNotes.Text = ratePlanData.Notes;

            if(ratePlanData.VehicleID.HasValue)
            {
                _selectedVehicleId = ratePlanData.VehicleID.Value;
                lblVehicleID.Text = _selectedVehicleId.ToString();
            }
            else
            {
                cbVehicleCategory.SelectedValue = ratePlanData.CategoryID;
            }

            _ApplyRulesForRatePlanScope();
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _ratePlanService.AddNewAsync(_BuildRatePlanModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة خطة سعر بنجاح. الرقم التعريفي: {result.Data.RatePlanID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {

            var result = await _ratePlanService.UpdateAsync(_ratePlanId.Value , _BuildRatePlanModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات خطة السعر بنجاح. الرقم التعريفي: {_ratePlanId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsRatePlansCreateUpdateModel _BuildRatePlanModel()
        {
            var model = new clsRatePlansCreateUpdateModel();

            model.RatePlanScope = (enRatePlaneScope)cbRatePlanScope.SelectedValue;
            model.StartDate = dtpStartDate.Value;
            model.EndDate = dtpEndDate.Value;
            model.PricePerDay = numericUpDownRentalPrice.Value;
            model.MinDays = numericUpDownMinDays.Value == 0 ? null : (int?)numericUpDownMinDays.Value;
            model.Notes = clsUtil.NullIfEmpty(txtNotes.Text);

            if (cbRatePlanScope.SelectedValue is enRatePlaneScope selectedScope && selectedScope == enRatePlaneScope.Category)
            {
                model.CategoryID = (enVehicleCategory)cbVehicleCategory.SelectedValue;
                model.VehicleID = null;
            }
            else
            {
                model.CategoryID = null;
                model.VehicleID = _selectedVehicleId;
            }

            return model;
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
                else
                {
                    clsMessages.ShowError(error.Message);
                }
            }
        }
        private void _ApplyRulesForRatePlanScope()
        {
            bool isCategory = SelectedScope == enRatePlaneScope.Category;

            labelCategory.Visible = isCategory;
            cbVehicleCategory.Visible = isCategory;

            labelVehicleIDTitle.Visible = !isCategory;
            lblVehicleID.Visible = !isCategory;
            linkLabelSearchOnVehicle.Visible = !isCategory;
        }
    }
}
