using CarRental.Customers.Attachments.Controls;
using CarRental.Customers.CustomersList.Controls;
using CarRental.Customers.People.Controls;
using CarRental.Helper;
using CarRental.Maintenance.Controls;
using CarRental.Payments.Invoices.Controls;
using CarRental.Payments.PaymentMethods.Controls;
using CarRental.Payments.PaymentStatus.Controls;
using CarRental.Payments.PaymentTransactions.Controls;
using CarRental.Rentals.BookingStatus.Controls;
using CarRental.Rentals.RatePlans.Controls;
using CarRental.Rentals.RentalBooking.Controls;
using CarRental.Rentals.ReturnStatus.Controls;
using CarRental.Rentals.VehicleReturn.Controls;
using CarRental.Reports.CustomersReports.Controls;
using CarRental.Reports.VehiclesReports.Controls;
using CarRental.SystemSettings.InsuranceTypes.Controls;
using CarRental.SystemSettings.Locations.Controls;
using CarRental.SystemSettings.Users.Controls;
using CarRental.Vehicles.FuelTypes.Controls;
using CarRental.Vehicles.VehicleCategory.Controls;
using CarRental.Vehicles.VehicleDamage.Controls;
using CarRental.Vehicles.VehicleInsurance.Controls;
using CarRental.Vehicles.VehiclesList.Controls;
using CarRental.Vehicles.VehicleStatus.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class frmMain : Form
    {
        private clsPageHelper _pageHelper;
        public frmMain()
        {
            _SetWindowState();
            InitializeComponent();

            _pageHelper = new clsPageHelper(this);
        }

        private void _SetWindowState()
        {
            this.WindowState = (Properties.Settings.Default.IsMaxScreen) ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        private void _SaveWindowState()
        {
            Properties.Settings.Default.IsMaxScreen = (this.WindowState == FormWindowState.Maximized) ? true : false;
            Properties.Settings.Default.Save();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _SaveWindowState();
        }

        private void toolStripMenuItemUsers_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlUsers,frmMain>(this,u=>new ctrlUsers(this)));
        }

        private void toolStripMenuItemPeople_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPeople, frmMain>(this, u => new ctrlPeople(this)));
        }

        private void toolStripMenuItemDashboard_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
        }

        private void toolStripMenuItemCustomersList_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlCustomers, frmMain>(this, u => new ctrlCustomers(this,ctrlCustomers.enMode.Management)));
        }

        private void toolStripMenuItemAttachments_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlAttachments,frmMain>(this,a=> new ctrlAttachments(this)));
        }

        private void toolStripMenuItemVehicleList_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicles, frmMain>(this, a => new ctrlVehicles(this,ctrlVehicles.enMode.Management)));
        }

        private void toolStripMenuItemVehicleCategory_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleCategory, frmMain>(this, f => new ctrlVehicleCategory(this)));
        }

        private void toolStripMenuItemVehicleStatus_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleStatus,frmMain>(this,s=> new ctrlVehicleStatus(this)));
        }

        private void toolStripMenuItemFuelTypes_Click(object sender, EventArgs e)
        {
           _pageHelper.SetPage(clsPageManager.GetPage<ctrlFuelTypes,frmMain>(this,f=> new ctrlFuelTypes(this)));
        }

        private void toolStripMenuItemVehicleInsurance_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleInsurance,frmMain>(this,i=> new ctrlVehicleInsurance(this)));
        }

        private void toolStripMenuItemVehicleDamage_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleDamage,frmMain>(this,d=> new ctrlVehicleDamage(this)));
        }

        private void toolStripMenuItemRentalBooking_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRentalBooking,frmMain>(this,r=> new ctrlRentalBooking(this)));
        }

        private void toolStripMenuItemVehicleReturn_Click(object sender, EventArgs e)
        {
            OpenVehicleReturnPage();
        }

        private void toolStripMenuItemBookingStatus_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlBookingStatus, frmMain>(this, b => new ctrlBookingStatus(this)));
        }

        private void toolStripMenuItemReturnStatus_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlReturnStatus, frmMain>(this, r => new ctrlReturnStatus(this)));
        }
        private void toolStripMenuItemRatePlans_Click(object sender, EventArgs e)
        {
           _pageHelper.SetPage(clsPageManager.GetPage<ctrlRatePlans,frmMain>(this,r=>new ctrlRatePlans(this)));
        }
        private void toolStripMenuItemPaymentTransactions_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentTransactions, frmMain>(this, p => new ctrlPaymentTransactions(this)));
        }
        private void toolStripMenuItemPaymentMethods_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentMethods, frmMain>(this, p => new ctrlPaymentMethods(this)));
        }
        private void toolStripMenuItemPaymentStatus_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentStatus, frmMain>(this, p => new ctrlPaymentStatus(this)));

        }

        private void toolStripMenuItemMaintenance_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlMaintenance, frmMain>(this, m => new ctrlMaintenance(this)));
        }

        private void toolStripMenuItemRentalReports_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
        }

        private void toolStripMenuItemVehiclesReports_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehiclesReports, frmMain>(this, r => new ctrlVehiclesReports(this)));

        }

        private void toolStripMenuItemCustomersReports_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlCustomerReports, frmMain>(this, r => new ctrlCustomerReports(this)));
        }
        private void toolStripMenuItemInsuranceTypes_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlInsuranceTypes, frmMain>(this, i => new ctrlInsuranceTypes(this)));
        }

        private void toolStripMenuItemLocations_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlLocations,frmMain>(this,l=> new ctrlLocations(this)));
        }

        private void toolStripMenuItemInvoices_Click(object sender, EventArgs e)
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlInvoices,frmMain>(this,i=> new ctrlInvoices(this)));
        }

        // =================== METHODES ==========

        public void OpenVehicleReturnPage()
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleReturn, frmMain>(this, r => new ctrlVehicleReturn(this)));
        }
    }
}
