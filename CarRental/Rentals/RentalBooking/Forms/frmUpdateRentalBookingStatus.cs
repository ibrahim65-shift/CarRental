using CarRental.Helper;
using CarRental_Buisness.Services.RentalBooking;
using SharedClass;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarRental.Rentals.RentalBooking.Forms
{
    public partial class frmUpdateRentalBookingStatus : Form
    {
        private clsRentalBookingService _rentalBookingService;

        private int _bookingId;
        private enBookingStatus _currentStatus;
        private bool _isSaving;
        public frmUpdateRentalBookingStatus(int bookingId , enBookingStatus status)
        {
            InitializeComponent();
            _rentalBookingService = new clsRentalBookingService();
            _bookingId = bookingId;
            _currentStatus = status;
        }

        private void frmUpdateRentalBookingStatus_Load(object sender, EventArgs e)
        {
            lblCurrentStatus.Text = clsUiHelper.GetEnumDescription(_currentStatus);
            _FillBookingStatusComboBox();
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _UpdateStatusAsync();

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmUpdateRentalBookingStatus.btnSave_Click", ex);
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

        // =================== METHODES ===================
        private void _FillBookingStatusComboBox()
        {
            var statuses = _rentalBookingService.GetAllowedStatuses(_currentStatus);

            var data = statuses.Select(s => new
            {
                Value = s,
                Text = clsUiHelper.GetEnumDescription(s)
            }).ToList();

            cbBookingStatus.DataSource = data;
            cbBookingStatus.DisplayMember = "Text";
            cbBookingStatus.ValueMember = "Value";
        }
        private async Task _UpdateStatusAsync()
        {
            var selectedStatus = (enBookingStatus)cbBookingStatus.SelectedValue;
            if(selectedStatus == _currentStatus)
            {
                clsMessages.ShowError("الحالة الجديدة هي نفسها الحالية");
                return;
            }

            var result = await _rentalBookingService.UpdateStatusAsync(_bookingId, selectedStatus);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess($"تم تحديث حالة الحجز بنجاح للرقم التعريفي: {_bookingId}");


            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
