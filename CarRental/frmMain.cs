
using CarRental.AboutMe;
using CarRental.Customers.CustomersList.Controls;
using CarRental.Customers.People.Controls;
using CarRental.Dashborad.Controls;
using CarRental.Helper;
using CarRental.Login;
using CarRental.Maintenance.Controls;
using CarRental.Payments.Invoices.Controls;
using CarRental.Payments.PaymentMethods.Controls;
using CarRental.Payments.PaymentStatus.Controls;
using CarRental.Payments.PaymentTransactions.Controls;
using CarRental.Permissions.RolePermissions.Controls;
using CarRental.Permissions.Roles.Controls;
using CarRental.Rentals.BookingStatus.Controls;
using CarRental.Rentals.RatePlans.Controls;
using CarRental.Rentals.RentalBooking.Controls;
using CarRental.Rentals.ReturnStatus.Controls;
using CarRental.Rentals.VehicleReturn.Controls;
using CarRental.Reports.CustomersReports.Controls;
using CarRental.Reports.RentalReports.Controls;
using CarRental.Reports.VehiclesReports.Controls;
using CarRental.Settings;
using CarRental.SystemSettings.InsuranceTypes.Controls;
using CarRental.SystemSettings.Locations.Controls;
using CarRental.SystemSettings.Users.Controls;
using CarRental.Vehicles.FuelTypes.Controls;
using CarRental.Vehicles.VehicleCategory.Controls;
using CarRental.Vehicles.VehicleDamage.Controls;
using CarRental.Vehicles.VehicleInsurance.Controls;
using CarRental.Vehicles.VehiclesList.Controls;
using CarRental.Vehicles.VehicleStatus.Controls;
using CarRental_Buisness;
using CarRental_Buisness.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class frmMain : Form
    {
        private clsPageHelper _pageHelper;
        private frmLogin _frmLogin;

        private clsPermissionHelper.PermissionBehavior _permissionBehavior;
        public frmMain(frmLogin login)
        {
            _SetWindowState();
            InitializeComponent();

            _permissionBehavior = clsPermissionHelper.PermissionBehavior.Hide;

            clsPermissionHelper.ApplyPermissions(this,_permissionBehavior);

            _InitializeMainForm();
            _pageHelper = new clsPageHelper(this);
            _frmLogin = login;
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
            if(!clsAuthorizationCache.HasPermission(toolStripMenuItemUsers.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض المستخدمين");
                return;
            }

            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlUsers,frmMain>(this,u=>new ctrlUsers(this)));
        }

        private void toolStripMenuItemPeople_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemPeople.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض الأشخاص");
                return;
            }

            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPeople, frmMain>(this, u => new ctrlPeople(this)));
        }

        private void toolStripMenuItemDashboard_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemDashboard.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض لوحة التحكم");
                return;
            }

            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlDashboard, frmMain>(this, u => new ctrlDashboard(this)));
        }

        private void toolStripMenuItemCustomersList_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemCustomersList.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض العملاء");
                return;
            }

            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlCustomers, frmMain>(this, u => new ctrlCustomers(this,ctrlCustomers.enMode.Management)));
        }

        private void toolStripMenuItemVehicleList_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleList.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicles, frmMain>(this, a => new ctrlVehicles(this,ctrlVehicles.enMode.Management)));
        }

        private void toolStripMenuItemVehicleCategory_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleCategory.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض فئات المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleCategory, frmMain>(this, f => new ctrlVehicleCategory(this)));
        }

        private void toolStripMenuItemVehicleStatus_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleStatus.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض حالة المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleStatus,frmMain>(this,s=> new ctrlVehicleStatus(this)));
        }

        private void toolStripMenuItemFuelTypes_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemFuelTypes.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض أنواع الوقود");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlFuelTypes,frmMain>(this,f=> new ctrlFuelTypes(this)));
        }

        private void toolStripMenuItemVehicleInsurance_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleInsurance.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض تأمين المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleInsurance,frmMain>(this,i=> new ctrlVehicleInsurance(this)));
        }

        private void toolStripMenuItemVehicleDamage_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleDamage.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض أضرار المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleDamage,frmMain>(this,d=> new ctrlVehicleDamage(this)));
        }

        private void toolStripMenuItemRentalBooking_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemRentalBooking.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض الحجوزات");
                return;
            }
            pictureBox1.Visible = false;
            OpenRentalBookingPage();
        }

        private void toolStripMenuItemVehicleReturn_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehicleReturn.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض إرجاع المركبات");
                return;
            }
            pictureBox1.Visible = false;
            OpenVehicleReturnPage();
        }

        private void toolStripMenuItemBookingStatus_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemBookingStatus.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض حالات الحجز");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlBookingStatus, frmMain>(this, b => new ctrlBookingStatus(this)));
        }

        private void toolStripMenuItemReturnStatus_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemReturnStatus.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض حالات الإرجاع");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlReturnStatus, frmMain>(this, r => new ctrlReturnStatus(this)));
        }
        private void toolStripMenuItemRatePlans_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemRatePlans.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض خطط الأسعار");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRatePlans,frmMain>(this,r=>new ctrlRatePlans(this)));
        }
        private void toolStripMenuItemPaymentTransactions_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemPaymentTransactions.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض عمليات الدفع");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentTransactions, frmMain>(this, p => new ctrlPaymentTransactions(this)));
        }
        private void toolStripMenuItemPaymentMethods_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemPaymentMethods.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض طرق الدفع");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentMethods, frmMain>(this, p => new ctrlPaymentMethods(this)));
        }
        private void toolStripMenuItemPaymentStatus_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemPaymentStatus.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض حالات الدفع");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlPaymentStatus, frmMain>(this, p => new ctrlPaymentStatus(this)));

        }

        private void toolStripMenuItemMaintenance_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemMaintenance.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض الصيانات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlMaintenance, frmMain>(this, m => new ctrlMaintenance(this)));
        }

        private void toolStripMenuItemRentalReports_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemRentalReports.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض تقارير الإيجارات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRentalReports, frmMain>(this, r => new ctrlRentalReports(this)));
        }

        private void toolStripMenuItemVehiclesReports_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemVehiclesReports.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض تقارير المركبات");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehiclesReports, frmMain>(this, r => new ctrlVehiclesReports(this)));

        }

        private void toolStripMenuItemCustomersReports_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemCustomersReports.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض تقارير العملاء");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlCustomerReports, frmMain>(this, r => new ctrlCustomerReports(this)));
        }
        private void toolStripMenuItemInsuranceTypes_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemInsuranceTypes.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض أنواع التأمين");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlInsuranceTypes, frmMain>(this, i => new ctrlInsuranceTypes(this)));
        }

        private void toolStripMenuItemLocations_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemLocations.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض المواقع");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlLocations,frmMain>(this,l=> new ctrlLocations(this)));
        }

        private void toolStripMenuItemInvoices_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemInvoices.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض الفواتير");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlInvoices,frmMain>(this,i=> new ctrlInvoices(this)));
        }
        private void toolStripMenuItemRoles_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemRoles.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض الأدوار");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRoles, frmMain>(this, i => new ctrlRoles(this)));
        }

        private void toolStripMenuItemRolePermissions_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemRolePermissions.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية عرض صلاحيات الأدوار");
                return;
            }
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRolePermissions, frmMain>(this, i => new ctrlRolePermissions(this)));
        }

        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            if (!clsAuthorizationCache.HasPermission(toolStripMenuItemSettings.Tag.ToString()))
            {
                clsMessages.ShowError("ليس لديك صلاحية تعديل الإعدادات");
                return;
            }
            pictureBox1.Visible = false;
            using (frmSettings frm = new frmSettings())
                frm.ShowDialog();
        }

        private void toolStripMenuItemAboutMe_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlAboutMe, frmMain>(this, i => new ctrlAboutMe()));
        }
        private void toolStripMenuItemlogout_Click(object sender, EventArgs e)
        {
            clsCurrentUser.Clear();
            _frmLogin.Show();
            Close();
        }

        // =================== METHODS ==========

        public void OpenVehicleReturnPage()
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlVehicleReturn, frmMain>(this, r => new ctrlVehicleReturn(this)));
        }
        public void OpenRentalBookingPage()
        {
            _pageHelper.SetPage(clsPageManager.GetPage<ctrlRentalBooking, frmMain>(this, r => new ctrlRentalBooking(this)));
        }
        private void _InitializeMainForm()
        {
            _SetMenuItemState(toolStripMenuItemCustomers,toolStripMenuItemCustomersList,toolStripMenuItemPeople);

            _SetMenuItemState(toolStripMenuItemVehicles,toolStripMenuItemVehicleList,toolStripMenuItemVehicleDamage,
                toolStripMenuItemVehicleInsurance,toolStripMenuItemVehicleCategory,toolStripMenuItemVehicleStatus,
                toolStripMenuItemFuelTypes);

            _SetMenuItemState(toolStripMenuItemRentals,toolStripMenuItemRentalBooking,toolStripMenuItemVehicleReturn,
                toolStripMenuItemReturnStatus,toolStripMenuItemBookingStatus,toolStripMenuItemRatePlans);

            _SetMenuItemState(toolStripMenuItemPayments,toolStripMenuItemPaymentTransactions,toolStripMenuItemPaymentMethods,
                toolStripMenuItemPaymentStatus,toolStripMenuItemInvoices);

            _SetMenuItemState(toolStripMenuItemReports,toolStripMenuItemCustomersReports,toolStripMenuItemRentalReports,
                toolStripMenuItemVehiclesReports);

            _SetMenuItemState(toolStripMenuItemSystem,toolStripMenuItemUsers,toolStripMenuItemLocations,toolStripMenuItemInsuranceTypes);

            _SetMenuItemState(toolStripMenuItemUsers,toolStripMenuItemRolePermissions,toolStripMenuItemRoles);
        }
        private void _SetMenuItemState(ToolStripMenuItem parent,params ToolStripMenuItem[] children)
        {
            bool hasPermission = children.Any(item => _HasPermission(item));

            switch (_permissionBehavior)
            {
                case clsPermissionHelper.PermissionBehavior.Hide:
                    parent.Visible = hasPermission;
                    break;

                case clsPermissionHelper.PermissionBehavior.Disable:
                    parent.Enabled = hasPermission;
                    break;
            }
        }
        private bool _HasPermission(ToolStripMenuItem item)
        {
            if (!(item?.Tag is string permissionCode))
                return false;

            permissionCode = permissionCode.Trim();

            if (string.IsNullOrEmpty(permissionCode))
                return false;

            return clsAuthorizationCache.HasPermission(permissionCode);
        }

    }
}
