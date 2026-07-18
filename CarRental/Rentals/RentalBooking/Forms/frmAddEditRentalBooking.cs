using CarRental.Customers.CustomersList.Forms;
using CarRental.Helper;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Models.RentalBooking;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.RentalBooking;
using CarRental_Buisness.Services.WorkFlow;
using DocumentFormat.OpenXml.Vml.Office;
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

namespace CarRental.Rentals.RentalBooking.Forms
{
    public partial class frmAddEditRentalBooking : Form
    {
        enum enMode { AddNew, Update }

        private clsRentalBookingService _rentalBookingService;
        private clsRentalBookingWorkflowService _workflowService;
        private Dictionary<string, Control> _validationControls;

        private enMode _mode;

        private int? _bookingId;
        private int? _selectedCustomerId;
        private DateTime _driverLicenseExpiry;
        private int? _selectedVehicleId;
        private bool _isSaving;
        public frmAddEditRentalBooking(clsRentalBookingService rentalBookingService, int? bookingId = null)
        {
            InitializeComponent();
            _rentalBookingService = rentalBookingService;
            _workflowService = new clsRentalBookingWorkflowService(rentalBookingService);

            _bookingId = bookingId;

            if (bookingId.HasValue)
            {
                this.Text = "تعديل حجز";
                _mode = enMode.Update;
            }
            else
            {
                this.Text = "إضافة حجز";
                _mode = enMode.AddNew;
            }

            _validationControls = new Dictionary<string, Control>()
            {
                { "CustomerID", lblCustomerId },
                { "DriverLicenseExpiry", lblCustomerId },
                { "VehicleID", lblVehicleID },
                { "RentalStartDate", dtpStartDate },
                { "RentalEndDate", dtpEndDate },
                { "PickupLocationID", cbPickupLocation },
                { "DropOffLocationID", cbDropOffLocation },
                { "InitialCheckNotes", txtNotes }
            };
        }

        private async void frmAddEditRentalBooking_Load(object sender, EventArgs e)
        {
            try
            {
                await _InitializeFormAsync();

                if (_mode == enMode.Update)
                    await _LoadRentalBookingDataAsync();

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditRentalBooking.frmAddEditRentalBooking_Load", ex);

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
                clsEventLogger.LogException("frmAddEditRentalBooking.btnSave_Click", ex);
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
        private void linkLabelSearchOnCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new frmSelectCustomer())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _driverLicenseExpiry = frm.DriverLicenseExpiry;
                    _selectedCustomerId = frm.SelectedCustomerId;
                    lblCustomerId.Text = _selectedCustomerId.ToString();
                }
            }
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
        private async void lblVehicleID_TextChanged(object sender, EventArgs e)
        {
            await _GetBookingSummary();
        }
        private async void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            await _GetBookingSummary();
        }
        private async void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            await _GetBookingSummary();
        }

        // ===================== METHODES ======================

        private async Task _InitializeFormAsync()
        {
            if (!await _FillLocationsAsync())
            {
                clsMessages.ShowError();
                Close();
            }
        }
        private async Task _LoadRentalBookingDataAsync()
        {
            if (!_bookingId.HasValue)
                throw new InvalidOperationException("معرف الحجز غير معروف");

            var result = await _rentalBookingService.GetRentalBookingByIDAsync(_bookingId.Value);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                Close();
                return;
            }

            var rentalBooking = result.Data;

            if (rentalBooking.BookingStatusID != enBookingStatus.Draft)
            {
                clsMessages.ShowError("لايمكن تحديث البيانات في هذه الحالة");
                Close();
                return;
            }

            lblCustomerId.Text = rentalBooking.CustomerID.ToString();
            lblVehicleID.Text = rentalBooking.VehicleID.ToString();
            dtpStartDate.Value = rentalBooking.RentalStartDate;
            dtpEndDate.Value = rentalBooking.RentalEndDate;
            cbPickupLocation.SelectedValue = rentalBooking.PickupLocationID;
            cbDropOffLocation.SelectedValue = rentalBooking.DropOffLocationID;
            txtNotes.Text = rentalBooking.InitialCheckNotes;

            _selectedCustomerId = rentalBooking.CustomerID;
            _selectedVehicleId = rentalBooking.VehicleID;

            lblRentalPrice.Text =clsUiHelper.ToSAR(rentalBooking.RentalPricePerDay);
            lblRentalDays.Text = rentalBooking.InitialRentalDays.ToString();
            lblEstimatedCost.Text = clsUiHelper.ToSAR(rentalBooking.InitialTotalDueAmount);

        }
        private async Task<bool> _FillLocationsAsync()
        {
            var result = await _rentalBookingService.GetAllLocationAsync();

            if (!result.Success || result.Data == null || result.Data.Count == 0)
            {
                cbPickupLocation.DataSource = null;
                cbDropOffLocation.DataSource = null;
                return false;
            }

            clsUiHelper.FillComboBoxGeneric(cbPickupLocation, result.Data, "DisplayName", "LocationID");
            clsUiHelper.FillComboBoxGeneric(cbDropOffLocation, result.Data, "DisplayName", "LocationID");

            return true;
        }
        private async Task _SaveAddNewAsync()
        {
            var model = _BuildRentalBookingAddNewModel();

            var result = await _workflowService.CreateBookingAsync(model, clsUiHelper.applicationSettings,_driverLicenseExpiry);
            if(!result.Success)
            {
                if(!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تمت إضافة حجز جديد بنجاح. الرقم التعريفي: {result.Data.BookingID}");


            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var model = _BuildRentalBookingUpdateModel();

            var result = await _workflowService.UpdateBookingAsync(_bookingId.Value, model, clsUiHelper.applicationSettings);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات الحجز بنجاح. الرقم التعريفي: {_bookingId.Value}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsRentalBookingAddNewModel _BuildRentalBookingAddNewModel()
        {
            var addNewModel = new clsRentalBookingAddNewModel();

            addNewModel.CustomerID = _selectedCustomerId.HasValue ? _selectedCustomerId.Value : 0;
            addNewModel.VehicleID = _selectedVehicleId.HasValue ? _selectedVehicleId.Value : 0;
            addNewModel.RentalStartDate = dtpStartDate.Value;
            addNewModel.RentalEndDate = dtpEndDate.Value;
            addNewModel.PickupLocationID = (int)cbPickupLocation.SelectedValue;
            addNewModel.DropOffLocationID = (int)cbDropOffLocation.SelectedValue;
            addNewModel.InitialCheckNotes = clsUtil.NullIfEmpty(txtNotes.Text);

            return addNewModel;
        }
        private clsRentalBookingUpdateModel _BuildRentalBookingUpdateModel()
        {
            var updateModel = new clsRentalBookingUpdateModel();

            updateModel.VehicleID = _selectedVehicleId.HasValue ? _selectedVehicleId.Value : 0;
            updateModel.RentalStartDate = dtpStartDate.Value;
            updateModel.RentalEndDate = dtpEndDate.Value;
            updateModel.PickupLocationID = (int)cbPickupLocation.SelectedValue;
            updateModel.DropOffLocationID = (int)cbDropOffLocation.SelectedValue;
            updateModel.InitialCheckNotes = clsUtil.NullIfEmpty(txtNotes.Text);

            return updateModel;
        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();

        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation == null || validation.Errors == null || validation.Errors.Count == 0)
            {
                return;
            }

            foreach (var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
                else
                    clsMessages.ShowError(error.Message);
            }
        }
        private async Task _GetBookingSummary()
        {
            if (!_selectedVehicleId.HasValue)
                return;

            int rentalDays = (dtpEndDate.Value.Date - dtpStartDate.Value.Date).Days;
            var result = await _rentalBookingService.GetRentalPricePerDayAsync(_selectedVehicleId.Value,dtpStartDate.Value ,rentalDays);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            lblRentalPrice.Text = clsUiHelper.ToSAR(result.Data);
            lblRentalDays.Text = rentalDays.ToString();
            lblEstimatedCost.Text = clsUiHelper.ToSAR((rentalDays * result.Data));
        }
    }
    
}
