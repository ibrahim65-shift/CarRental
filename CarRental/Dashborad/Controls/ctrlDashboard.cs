using CarRental.Customers.CustomersList.Forms;
using CarRental.Helper;
using CarRental.Rentals.RentalBooking.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Models.Dashboard;
using CarRental_Buisness.Services.Customers;
using CarRental_Buisness.Services.Dashboard;
using CarRental_Buisness.Services.RentalBooking;
using CarRental_Buisness.Services.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Dashborad.Controls
{
    public partial class ctrlDashboard : UserControl,IRefreshable
    {
        private readonly clsDashboardService _dashboardService;
        private readonly frmMain _frmMain;

        private readonly clsRentalBookingService _rentalBookingService;
        private readonly clsCustomerService _customerService;
        private readonly clsVehicleService _vehicleService;

        private readonly Dictionary<string, string> _columnsHeader = new Dictionary<string, string>
        {
            {"BookingID"      , "المعرف" },
            {"Customer"       , "العميل" },
            {"Vehicle"        , "المركبة" },
            {"RentalStartDate", "بداية الإيجار" },
            {"RentalEndDate"  , "نهاية الإيجار" },
            {"StatusName"     , "الحالة" },
        };

        private bool _isLoading;

        public event Action DataRefreshed;
        public ctrlDashboard(frmMain frmMain)
        {
            InitializeComponent();
            _dashboardService = new clsDashboardService();
            _frmMain = frmMain;

            _rentalBookingService = new clsRentalBookingService();
            _customerService = new clsCustomerService();
            _vehicleService = new clsVehicleService();
        }

        public async Task RefreshDataAsync()
        {
            await _LoadDashboardAsync();
        }
        private async void ctrlDashboard_Load(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }
        private async void linkLabelAddNewBooking_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmAddEditRentalBooking frm = new frmAddEditRentalBooking(_rentalBookingService))
            {
                if(frm.ShowDialog() == DialogResult.OK)
                {
                    await RefreshDataAsync();
                }
            }
        }
        private async void linkLabelAddNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmAddEditCustomer frm = new frmAddEditCustomer(_customerService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await RefreshDataAsync();
                }
            }
        }
        private async void linkLabelAddNewVehicle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmAddEditVehicle frm = new frmAddEditVehicle(_vehicleService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await RefreshDataAsync();
                }
            }
        }
        private void linkLabelAllBookings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _frmMain.OpenRentalBookingPage();
        }
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }

        // =============== METHODS ================

        private async Task _LoadDashboardAsync()
        {
            if (_isLoading)
                return;
            try
            {
                _isLoading = true;
                btnRefresh.Enabled = false;

                var result = await _dashboardService.GetDashboardDataAsync();
                if (!result.Success)
                {
                    clsMessages.ShowError(result.ErrorMessage);
                    return;
                }

                lblLastRefresh.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm");

                _FillStatisticData(result.Data.Statistics);
                _FillLatestBookingsData(result.Data.LatestBookings);
                _FillAlertsData(result.Data.Alerts);
            }
            finally
            {
                _isLoading = false;
                btnRefresh.Enabled = true;

                DataRefreshed?.Invoke();
            }
        }
        private void _FillStatisticData(clsStatisticsDto s)
        {
            usersStatisticCard.SetValue(s.UsersCount);
            customersStatisticCard.SetValue(s.CustomersCount);
            vehicleStatisticCard.SetValue(s.VehiclesCount);
            monthlyRevenueStatisticCard.SetValue(clsUiHelper.ToSAR(s.MonthlyRevenue));
            activeBookingsStatisticCard.SetValue(s.ActiveBookings);

            lblAvaliableVehicles.Text = $"{s.AvailableVehicles:N0}";
            lblRentedVehicles.Text = $"{s.RentedVehicles:N0}";
            lblMaintenanceVehicles.Text = $"{s.MaintenanceVehicles:N0}";
        }
        private void _FillLatestBookingsData(List<clsLatestBookingsDto> latestBookings)
        {
            dgvLatestBookings.DataSource = latestBookings;
            _SetLatestBookingsColumns();
        }
        private void _FillAlertsData(List<clsAlertsDto> alerts)
        {
            flowLayoutPanelAlerts.Controls.Clear();

            if (alerts.Count == 0)
            {
                lblNoAlerts.Visible = true;
                return;
            }

            lblNoAlerts.Visible = false;

            flowLayoutPanelAlerts.SuspendLayout();
            
            foreach(var alert in alerts)
            {
                var card = new ctrlAlertCard();
                card.LoadData(alert);

                flowLayoutPanelAlerts.Controls.Add(card);
            }

            flowLayoutPanelAlerts.ResumeLayout();
        }
        private void _SetLatestBookingsColumns()
        {
            if (dgvLatestBookings == null || dgvLatestBookings.Columns.Count == 0 )
                return;

            foreach (var col in _columnsHeader)
            {
                _SetColumnHeader(col.Key, col.Value);
            }
        }
        private void _SetColumnHeader(string column , string value)
        {
            if(dgvLatestBookings.Columns.Contains(column))
                dgvLatestBookings.Columns[column].HeaderText = value;
        }

    }
}
