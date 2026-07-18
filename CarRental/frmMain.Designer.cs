namespace CarRental
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCustomers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCustomersList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeople = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAttachments = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFuelTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleInsurance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleDamage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRentals = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRentalBooking = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehicleReturn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemBookingStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReturnStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRatePlans = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPayments = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaymentTransactions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaymentMethods = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaymentStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInvoices = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMaintenance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReports = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRentalReports = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaymentsReports = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemVehiclesReports = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCustomersReports = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInsuranceTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLocations = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.menuStrip1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1694, 71);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDashboard,
            this.toolStripMenuItemCustomers,
            this.toolStripMenuItemVehicles,
            this.toolStripMenuItemRentals,
            this.toolStripMenuItemPayments,
            this.toolStripMenuItemMaintenance,
            this.toolStripMenuItemReports,
            this.toolStripMenuItemSystem});
            this.menuStrip1.Location = new System.Drawing.Point(32, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1662, 71);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "Main Page";
            // 
            // toolStripMenuItemDashboard
            // 
            this.toolStripMenuItemDashboard.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemDashboard.Image = global::CarRental.Properties.Resources.dashboard_64;
            this.toolStripMenuItemDashboard.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemDashboard.Name = "toolStripMenuItemDashboard";
            this.toolStripMenuItemDashboard.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemDashboard.Size = new System.Drawing.Size(255, 67);
            this.toolStripMenuItemDashboard.Text = "لوحة التحكم";
            this.toolStripMenuItemDashboard.Click += new System.EventHandler(this.toolStripMenuItemDashboard_Click);
            // 
            // toolStripMenuItemCustomers
            // 
            this.toolStripMenuItemCustomers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCustomersList,
            this.toolStripMenuItemPeople,
            this.toolStripMenuItemAttachments});
            this.toolStripMenuItemCustomers.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemCustomers.Image = global::CarRental.Properties.Resources.Customers_64;
            this.toolStripMenuItemCustomers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemCustomers.Name = "toolStripMenuItemCustomers";
            this.toolStripMenuItemCustomers.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemCustomers.Size = new System.Drawing.Size(189, 67);
            this.toolStripMenuItemCustomers.Text = "العملاء";
            // 
            // toolStripMenuItemCustomersList
            // 
            this.toolStripMenuItemCustomersList.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemCustomersList.Image = global::CarRental.Properties.Resources.customersList_64;
            this.toolStripMenuItemCustomersList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemCustomersList.Name = "toolStripMenuItemCustomersList";
            this.toolStripMenuItemCustomersList.Size = new System.Drawing.Size(311, 74);
            this.toolStripMenuItemCustomersList.Text = "قائمة العملاء";
            this.toolStripMenuItemCustomersList.Click += new System.EventHandler(this.toolStripMenuItemCustomersList_Click);
            // 
            // toolStripMenuItemPeople
            // 
            this.toolStripMenuItemPeople.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPeople.Image = global::CarRental.Properties.Resources.People_64;
            this.toolStripMenuItemPeople.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPeople.Name = "toolStripMenuItemPeople";
            this.toolStripMenuItemPeople.Size = new System.Drawing.Size(311, 74);
            this.toolStripMenuItemPeople.Text = "الأشخاص";
            this.toolStripMenuItemPeople.Click += new System.EventHandler(this.toolStripMenuItemPeople_Click);
            // 
            // toolStripMenuItemAttachments
            // 
            this.toolStripMenuItemAttachments.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemAttachments.Image = global::CarRental.Properties.Resources.attach_64;
            this.toolStripMenuItemAttachments.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemAttachments.Name = "toolStripMenuItemAttachments";
            this.toolStripMenuItemAttachments.Size = new System.Drawing.Size(311, 74);
            this.toolStripMenuItemAttachments.Text = "المرفقات";
            this.toolStripMenuItemAttachments.Click += new System.EventHandler(this.toolStripMenuItemAttachments_Click);
            // 
            // toolStripMenuItemVehicles
            // 
            this.toolStripMenuItemVehicles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemVehicleList,
            this.toolStripMenuItemVehicleCategory,
            this.toolStripMenuItemVehicleStatus,
            this.toolStripMenuItemFuelTypes,
            this.toolStripMenuItemVehicleInsurance,
            this.toolStripMenuItemVehicleDamage});
            this.toolStripMenuItemVehicles.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicles.Image = global::CarRental.Properties.Resources.Vehicles_64;
            this.toolStripMenuItemVehicles.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicles.Name = "toolStripMenuItemVehicles";
            this.toolStripMenuItemVehicles.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemVehicles.Size = new System.Drawing.Size(212, 67);
            this.toolStripMenuItemVehicles.Text = "المركبات";
            // 
            // toolStripMenuItemVehicleList
            // 
            this.toolStripMenuItemVehicleList.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleList.Image = global::CarRental.Properties.Resources.VehiclesList_64;
            this.toolStripMenuItemVehicleList.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleList.Name = "toolStripMenuItemVehicleList";
            this.toolStripMenuItemVehicleList.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemVehicleList.Text = "قائمة المركبات";
            this.toolStripMenuItemVehicleList.Click += new System.EventHandler(this.toolStripMenuItemVehicleList_Click);
            // 
            // toolStripMenuItemVehicleCategory
            // 
            this.toolStripMenuItemVehicleCategory.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleCategory.Image = global::CarRental.Properties.Resources.VehiclesCategory_64;
            this.toolStripMenuItemVehicleCategory.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleCategory.Name = "toolStripMenuItemVehicleCategory";
            this.toolStripMenuItemVehicleCategory.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemVehicleCategory.Text = "فئات المركبات";
            this.toolStripMenuItemVehicleCategory.Click += new System.EventHandler(this.toolStripMenuItemVehicleCategory_Click);
            // 
            // toolStripMenuItemVehicleStatus
            // 
            this.toolStripMenuItemVehicleStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleStatus.Image = global::CarRental.Properties.Resources.VehicleStatus_64;
            this.toolStripMenuItemVehicleStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleStatus.Name = "toolStripMenuItemVehicleStatus";
            this.toolStripMenuItemVehicleStatus.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemVehicleStatus.Text = "حالة المركبة";
            this.toolStripMenuItemVehicleStatus.Click += new System.EventHandler(this.toolStripMenuItemVehicleStatus_Click);
            // 
            // toolStripMenuItemFuelTypes
            // 
            this.toolStripMenuItemFuelTypes.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemFuelTypes.Image = global::CarRental.Properties.Resources.fuelType_64;
            this.toolStripMenuItemFuelTypes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemFuelTypes.Name = "toolStripMenuItemFuelTypes";
            this.toolStripMenuItemFuelTypes.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemFuelTypes.Text = "أنواع الوقود";
            this.toolStripMenuItemFuelTypes.Click += new System.EventHandler(this.toolStripMenuItemFuelTypes_Click);
            // 
            // toolStripMenuItemVehicleInsurance
            // 
            this.toolStripMenuItemVehicleInsurance.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleInsurance.Image = global::CarRental.Properties.Resources.VehicleInsurance_64;
            this.toolStripMenuItemVehicleInsurance.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleInsurance.Name = "toolStripMenuItemVehicleInsurance";
            this.toolStripMenuItemVehicleInsurance.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemVehicleInsurance.Text = "تأمين المركبات";
            this.toolStripMenuItemVehicleInsurance.Click += new System.EventHandler(this.toolStripMenuItemVehicleInsurance_Click);
            // 
            // toolStripMenuItemVehicleDamage
            // 
            this.toolStripMenuItemVehicleDamage.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleDamage.Image = global::CarRental.Properties.Resources.VehicleDamage_64;
            this.toolStripMenuItemVehicleDamage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleDamage.Name = "toolStripMenuItemVehicleDamage";
            this.toolStripMenuItemVehicleDamage.Size = new System.Drawing.Size(335, 74);
            this.toolStripMenuItemVehicleDamage.Text = "أضرار المركبات";
            this.toolStripMenuItemVehicleDamage.Click += new System.EventHandler(this.toolStripMenuItemVehicleDamage_Click);
            // 
            // toolStripMenuItemRentals
            // 
            this.toolStripMenuItemRentals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRentalBooking,
            this.toolStripMenuItemVehicleReturn,
            this.toolStripMenuItemBookingStatus,
            this.toolStripMenuItemReturnStatus,
            this.toolStripMenuItemRatePlans});
            this.toolStripMenuItemRentals.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemRentals.Image = global::CarRental.Properties.Resources.RentalCars_64;
            this.toolStripMenuItemRentals.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemRentals.Name = "toolStripMenuItemRentals";
            this.toolStripMenuItemRentals.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemRentals.Size = new System.Drawing.Size(181, 67);
            this.toolStripMenuItemRentals.Text = "التأجير";
            // 
            // toolStripMenuItemRentalBooking
            // 
            this.toolStripMenuItemRentalBooking.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemRentalBooking.Image = global::CarRental.Properties.Resources.RentalBooking_64;
            this.toolStripMenuItemRentalBooking.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemRentalBooking.Name = "toolStripMenuItemRentalBooking";
            this.toolStripMenuItemRentalBooking.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemRentalBooking.Text = "حجز المركبة";
            this.toolStripMenuItemRentalBooking.Click += new System.EventHandler(this.toolStripMenuItemRentalBooking_Click);
            // 
            // toolStripMenuItemVehicleReturn
            // 
            this.toolStripMenuItemVehicleReturn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehicleReturn.Image = global::CarRental.Properties.Resources.VehicleReturn_64;
            this.toolStripMenuItemVehicleReturn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehicleReturn.Name = "toolStripMenuItemVehicleReturn";
            this.toolStripMenuItemVehicleReturn.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemVehicleReturn.Text = "إرجاع المركبة";
            this.toolStripMenuItemVehicleReturn.Click += new System.EventHandler(this.toolStripMenuItemVehicleReturn_Click);
            // 
            // toolStripMenuItemBookingStatus
            // 
            this.toolStripMenuItemBookingStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemBookingStatus.Image = global::CarRental.Properties.Resources.BookingStatus_64;
            this.toolStripMenuItemBookingStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemBookingStatus.Name = "toolStripMenuItemBookingStatus";
            this.toolStripMenuItemBookingStatus.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemBookingStatus.Text = "حالة الحجز";
            this.toolStripMenuItemBookingStatus.Click += new System.EventHandler(this.toolStripMenuItemBookingStatus_Click);
            // 
            // toolStripMenuItemReturnStatus
            // 
            this.toolStripMenuItemReturnStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemReturnStatus.Image = global::CarRental.Properties.Resources.ReturnStatus_64;
            this.toolStripMenuItemReturnStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemReturnStatus.Name = "toolStripMenuItemReturnStatus";
            this.toolStripMenuItemReturnStatus.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemReturnStatus.Text = "حالة الإرجاع";
            this.toolStripMenuItemReturnStatus.Click += new System.EventHandler(this.toolStripMenuItemReturnStatus_Click);
            // 
            // toolStripMenuItemRatePlans
            // 
            this.toolStripMenuItemRatePlans.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemRatePlans.Image = global::CarRental.Properties.Resources.RatePlans_64;
            this.toolStripMenuItemRatePlans.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemRatePlans.Name = "toolStripMenuItemRatePlans";
            this.toolStripMenuItemRatePlans.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemRatePlans.Text = "خطط الأسعار";
            this.toolStripMenuItemRatePlans.Click += new System.EventHandler(this.toolStripMenuItemRatePlans_Click);
            // 
            // toolStripMenuItemPayments
            // 
            this.toolStripMenuItemPayments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPaymentTransactions,
            this.toolStripMenuItemPaymentMethods,
            this.toolStripMenuItemPaymentStatus,
            this.toolStripMenuItemInvoices});
            this.toolStripMenuItemPayments.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPayments.Image = global::CarRental.Properties.Resources.Payments_64;
            this.toolStripMenuItemPayments.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPayments.Name = "toolStripMenuItemPayments";
            this.toolStripMenuItemPayments.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemPayments.Size = new System.Drawing.Size(242, 67);
            this.toolStripMenuItemPayments.Text = "المدفوعات";
            // 
            // toolStripMenuItemPaymentTransactions
            // 
            this.toolStripMenuItemPaymentTransactions.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPaymentTransactions.Image = global::CarRental.Properties.Resources.PaymentTransactions_64;
            this.toolStripMenuItemPaymentTransactions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPaymentTransactions.Name = "toolStripMenuItemPaymentTransactions";
            this.toolStripMenuItemPaymentTransactions.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemPaymentTransactions.Text = "عمليات الدفع";
            this.toolStripMenuItemPaymentTransactions.Click += new System.EventHandler(this.toolStripMenuItemPaymentTransactions_Click);
            // 
            // toolStripMenuItemPaymentMethods
            // 
            this.toolStripMenuItemPaymentMethods.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPaymentMethods.Image = global::CarRental.Properties.Resources.PaymentMethods_64;
            this.toolStripMenuItemPaymentMethods.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPaymentMethods.Name = "toolStripMenuItemPaymentMethods";
            this.toolStripMenuItemPaymentMethods.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemPaymentMethods.Text = "طرق الدفع";
            this.toolStripMenuItemPaymentMethods.Click += new System.EventHandler(this.toolStripMenuItemPaymentMethods_Click);
            // 
            // toolStripMenuItemPaymentStatus
            // 
            this.toolStripMenuItemPaymentStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPaymentStatus.Image = global::CarRental.Properties.Resources.PaymentStatus_64;
            this.toolStripMenuItemPaymentStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPaymentStatus.Name = "toolStripMenuItemPaymentStatus";
            this.toolStripMenuItemPaymentStatus.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemPaymentStatus.Text = "حالة الدفع";
            this.toolStripMenuItemPaymentStatus.Click += new System.EventHandler(this.toolStripMenuItemPaymentStatus_Click);
            // 
            // toolStripMenuItemInvoices
            // 
            this.toolStripMenuItemInvoices.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.toolStripMenuItemInvoices.Image = global::CarRental.Properties.Resources.PaymentReports_64;
            this.toolStripMenuItemInvoices.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemInvoices.Name = "toolStripMenuItemInvoices";
            this.toolStripMenuItemInvoices.Size = new System.Drawing.Size(318, 74);
            this.toolStripMenuItemInvoices.Text = "الفواتير";
            this.toolStripMenuItemInvoices.Click += new System.EventHandler(this.toolStripMenuItemInvoices_Click);
            // 
            // toolStripMenuItemMaintenance
            // 
            this.toolStripMenuItemMaintenance.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemMaintenance.Image = global::CarRental.Properties.Resources.maintenance_64;
            this.toolStripMenuItemMaintenance.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemMaintenance.Name = "toolStripMenuItemMaintenance";
            this.toolStripMenuItemMaintenance.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemMaintenance.Size = new System.Drawing.Size(197, 67);
            this.toolStripMenuItemMaintenance.Text = "الصيانة";
            this.toolStripMenuItemMaintenance.Click += new System.EventHandler(this.toolStripMenuItemMaintenance_Click);
            // 
            // toolStripMenuItemReports
            // 
            this.toolStripMenuItemReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRentalReports,
            this.toolStripMenuItemPaymentsReports,
            this.toolStripMenuItemVehiclesReports,
            this.toolStripMenuItemCustomersReports});
            this.toolStripMenuItemReports.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemReports.Image = global::CarRental.Properties.Resources.reports_64;
            this.toolStripMenuItemReports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemReports.Name = "toolStripMenuItemReports";
            this.toolStripMenuItemReports.RightToLeftAutoMirrorImage = true;
            this.toolStripMenuItemReports.Size = new System.Drawing.Size(194, 67);
            this.toolStripMenuItemReports.Text = "التقارير";
            // 
            // toolStripMenuItemRentalReports
            // 
            this.toolStripMenuItemRentalReports.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemRentalReports.Image = global::CarRental.Properties.Resources.RentalReports_64;
            this.toolStripMenuItemRentalReports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemRentalReports.Name = "toolStripMenuItemRentalReports";
            this.toolStripMenuItemRentalReports.Size = new System.Drawing.Size(363, 74);
            this.toolStripMenuItemRentalReports.Text = "تقارير التأجير";
            this.toolStripMenuItemRentalReports.Click += new System.EventHandler(this.toolStripMenuItemRentalReports_Click);
            // 
            // toolStripMenuItemPaymentsReports
            // 
            this.toolStripMenuItemPaymentsReports.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemPaymentsReports.Image = global::CarRental.Properties.Resources.PaymentReports_64;
            this.toolStripMenuItemPaymentsReports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemPaymentsReports.Name = "toolStripMenuItemPaymentsReports";
            this.toolStripMenuItemPaymentsReports.Size = new System.Drawing.Size(363, 74);
            this.toolStripMenuItemPaymentsReports.Text = "تقارير المدفوعات";
            this.toolStripMenuItemPaymentsReports.Click += new System.EventHandler(this.toolStripMenuItemPaymentsReports_Click);
            // 
            // toolStripMenuItemVehiclesReports
            // 
            this.toolStripMenuItemVehiclesReports.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemVehiclesReports.Image = global::CarRental.Properties.Resources.VehiclesReports_64;
            this.toolStripMenuItemVehiclesReports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemVehiclesReports.Name = "toolStripMenuItemVehiclesReports";
            this.toolStripMenuItemVehiclesReports.Size = new System.Drawing.Size(363, 74);
            this.toolStripMenuItemVehiclesReports.Text = "تقارير المركبات";
            this.toolStripMenuItemVehiclesReports.Click += new System.EventHandler(this.toolStripMenuItemVehiclesReports_Click);
            // 
            // toolStripMenuItemCustomersReports
            // 
            this.toolStripMenuItemCustomersReports.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemCustomersReports.Image = global::CarRental.Properties.Resources.CustomersReports_64;
            this.toolStripMenuItemCustomersReports.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemCustomersReports.Name = "toolStripMenuItemCustomersReports";
            this.toolStripMenuItemCustomersReports.Size = new System.Drawing.Size(363, 74);
            this.toolStripMenuItemCustomersReports.Text = "تقارير العملاء";
            this.toolStripMenuItemCustomersReports.Click += new System.EventHandler(this.toolStripMenuItemCustomersReports_Click);
            // 
            // toolStripMenuItemSystem
            // 
            this.toolStripMenuItemSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemUsers,
            this.toolStripMenuItemInsuranceTypes,
            this.toolStripMenuItemLocations});
            this.toolStripMenuItemSystem.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemSystem.Image = global::CarRental.Properties.Resources.System_64;
            this.toolStripMenuItemSystem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemSystem.Name = "toolStripMenuItemSystem";
            this.toolStripMenuItemSystem.Size = new System.Drawing.Size(184, 67);
            this.toolStripMenuItemSystem.Text = "النظام";
            // 
            // toolStripMenuItemUsers
            // 
            this.toolStripMenuItemUsers.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemUsers.Image = global::CarRental.Properties.Resources.Users_64;
            this.toolStripMenuItemUsers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemUsers.Name = "toolStripMenuItemUsers";
            this.toolStripMenuItemUsers.Size = new System.Drawing.Size(310, 74);
            this.toolStripMenuItemUsers.Text = "المستخدمون";
            this.toolStripMenuItemUsers.Click += new System.EventHandler(this.toolStripMenuItemUsers_Click);
            // 
            // toolStripMenuItemInsuranceTypes
            // 
            this.toolStripMenuItemInsuranceTypes.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemInsuranceTypes.Image = global::CarRental.Properties.Resources.insuranceTypes_64;
            this.toolStripMenuItemInsuranceTypes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemInsuranceTypes.Name = "toolStripMenuItemInsuranceTypes";
            this.toolStripMenuItemInsuranceTypes.Size = new System.Drawing.Size(310, 74);
            this.toolStripMenuItemInsuranceTypes.Text = "أنواع التأمين";
            this.toolStripMenuItemInsuranceTypes.Click += new System.EventHandler(this.toolStripMenuItemInsuranceTypes_Click);
            // 
            // toolStripMenuItemLocations
            // 
            this.toolStripMenuItemLocations.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMenuItemLocations.Image = global::CarRental.Properties.Resources.Locations_64;
            this.toolStripMenuItemLocations.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemLocations.Name = "toolStripMenuItemLocations";
            this.toolStripMenuItemLocations.Size = new System.Drawing.Size(310, 74);
            this.toolStripMenuItemLocations.Text = "المواقع";
            this.toolStripMenuItemLocations.Click += new System.EventHandler(this.toolStripMenuItemLocations_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 71);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1694, 510);
            this.pnlMain.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1694, 581);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMain";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "الرئيسية";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDashboard;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomers;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRentals;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPayments;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMaintenance;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSystem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomersList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeople;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAttachments;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleCategory;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFuelTypes;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleInsurance;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleDamage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRentalBooking;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehicleReturn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemBookingStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReturnStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRatePlans;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaymentTransactions;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaymentMethods;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaymentStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRentalReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaymentsReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemVehiclesReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomersReports;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUsers;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInsuranceTypes;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLocations;
        internal System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInvoices;
    }
}