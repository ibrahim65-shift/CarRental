namespace CarRental.Rentals.RentalBooking.Forms
{
    partial class frmAddEditRentalBooking
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddEditRentalBooking));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblEstimatedCost = new System.Windows.Forms.Label();
            this.lblRentalDays = new System.Windows.Forms.Label();
            this.lblRentalPrice = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDropOffLocation = new System.Windows.Forms.ComboBox();
            this.cbPickupLocation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelSearchOnCustomer = new System.Windows.Forms.LinkLabel();
            this.linkLabelSearchOnVehicle = new System.Windows.Forms.LinkLabel();
            this.lblCustomerId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVehicleID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.rentalBooking_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(328, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(486, 198);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnCancel.Image = global::CarRental.Properties.Resources.Cancel_32;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(194, 821);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 54);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "  إلغاء    ";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnSave.Image = global::CarRental.Properties.Resources.Save_32;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(13, 821);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.txtNotes);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbDropOffLocation);
            this.groupBox1.Controls.Add(this.cbPickupLocation);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.linkLabelSearchOnCustomer);
            this.groupBox1.Controls.Add(this.linkLabelSearchOnVehicle);
            this.groupBox1.Controls.Add(this.lblCustomerId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblVehicleID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(17, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1113, 597);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "معلومات الحجز";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEstimatedCost);
            this.panel1.Controls.Add(this.lblRentalDays);
            this.panel1.Controls.Add(this.lblRentalPrice);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(6, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 179);
            this.panel1.TabIndex = 37;
            // 
            // lblEstimatedCost
            // 
            this.lblEstimatedCost.AutoSize = true;
            this.lblEstimatedCost.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblEstimatedCost.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEstimatedCost.Location = new System.Drawing.Point(16, 135);
            this.lblEstimatedCost.Name = "lblEstimatedCost";
            this.lblEstimatedCost.Size = new System.Drawing.Size(73, 38);
            this.lblEstimatedCost.TabIndex = 41;
            this.lblEstimatedCost.Text = "؟؟؟؟";
            // 
            // lblRentalDays
            // 
            this.lblRentalDays.AutoSize = true;
            this.lblRentalDays.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblRentalDays.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRentalDays.Location = new System.Drawing.Point(75, 74);
            this.lblRentalDays.Name = "lblRentalDays";
            this.lblRentalDays.Size = new System.Drawing.Size(73, 38);
            this.lblRentalDays.TabIndex = 40;
            this.lblRentalDays.Text = "؟؟؟؟";
            // 
            // lblRentalPrice
            // 
            this.lblRentalPrice.AutoSize = true;
            this.lblRentalPrice.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblRentalPrice.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRentalPrice.Location = new System.Drawing.Point(16, 13);
            this.lblRentalPrice.Name = "lblRentalPrice";
            this.lblRentalPrice.Size = new System.Drawing.Size(73, 38);
            this.lblRentalPrice.TabIndex = 39;
            this.lblRentalPrice.Text = "؟؟؟؟";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label10.Image = ((System.Drawing.Image)(resources.GetObject("label10.Image")));
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label10.Location = new System.Drawing.Point(250, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 38);
            this.label10.TabIndex = 38;
            this.label10.Text = "أيام الإيجار:     ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label9.Image = ((System.Drawing.Image)(resources.GetObject("label9.Image")));
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.Location = new System.Drawing.Point(208, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(231, 38);
            this.label9.TabIndex = 37;
            this.label9.Text = "التكلفة المقدرة:     ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label8.Location = new System.Drawing.Point(250, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(189, 38);
            this.label8.TabIndex = 36;
            this.label8.Text = "سعر الإيجار:     ";
            // 
            // txtNotes
            // 
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Location = new System.Drawing.Point(22, 439);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(839, 134);
            this.txtNotes.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = global::CarRental.Properties.Resources.Notes_32;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Location = new System.Drawing.Point(874, 436);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 38);
            this.label7.TabIndex = 33;
            this.label7.Text = "الملاحظات:     ";
            // 
            // cbDropOffLocation
            // 
            this.cbDropOffLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDropOffLocation.FormattingEnabled = true;
            this.cbDropOffLocation.Location = new System.Drawing.Point(352, 369);
            this.cbDropOffLocation.Name = "cbDropOffLocation";
            this.cbDropOffLocation.Size = new System.Drawing.Size(519, 40);
            this.cbDropOffLocation.TabIndex = 6;
            // 
            // cbPickupLocation
            // 
            this.cbPickupLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPickupLocation.FormattingEnabled = true;
            this.cbPickupLocation.Location = new System.Drawing.Point(352, 298);
            this.cbPickupLocation.Name = "cbPickupLocation";
            this.cbPickupLocation.Size = new System.Drawing.Size(519, 40);
            this.cbPickupLocation.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = global::CarRental.Properties.Resources.dropOffLocation_32;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(884, 367);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 38);
            this.label6.TabIndex = 30;
            this.label6.Text = "موقع التسليم:     ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::CarRental.Properties.Resources.location_32;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(884, 296);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 38);
            this.label5.TabIndex = 29;
            this.label5.Text = "موقع الاستلام:     ";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(22, 217);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(268, 39);
            this.dtpEndDate.TabIndex = 4;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(574, 219);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(297, 39);
            this.dtpStartDate.TabIndex = 3;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::CarRental.Properties.Resources.startDate_32;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(890, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 38);
            this.label4.TabIndex = 26;
            this.label4.Text = "بداية الحجز:     ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(318, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 38);
            this.label3.TabIndex = 25;
            this.label3.Text = "نهاية الحجز:     ";
            // 
            // linkLabelSearchOnCustomer
            // 
            this.linkLabelSearchOnCustomer.AutoSize = true;
            this.linkLabelSearchOnCustomer.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.linkLabelSearchOnCustomer.Location = new System.Drawing.Point(456, 62);
            this.linkLabelSearchOnCustomer.Name = "linkLabelSearchOnCustomer";
            this.linkLabelSearchOnCustomer.Size = new System.Drawing.Size(321, 38);
            this.linkLabelSearchOnCustomer.TabIndex = 1;
            this.linkLabelSearchOnCustomer.TabStop = true;
            this.linkLabelSearchOnCustomer.Text = "اضغط للبحث عن عميل . . .";
            this.linkLabelSearchOnCustomer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSearchOnCustomer_LinkClicked);
            // 
            // linkLabelSearchOnVehicle
            // 
            this.linkLabelSearchOnVehicle.AutoSize = true;
            this.linkLabelSearchOnVehicle.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.linkLabelSearchOnVehicle.Location = new System.Drawing.Point(456, 129);
            this.linkLabelSearchOnVehicle.Name = "linkLabelSearchOnVehicle";
            this.linkLabelSearchOnVehicle.Size = new System.Drawing.Size(324, 38);
            this.linkLabelSearchOnVehicle.TabIndex = 2;
            this.linkLabelSearchOnVehicle.TabStop = true;
            this.linkLabelSearchOnVehicle.Text = "اضغط للبحث عن مركبة . . .";
            this.linkLabelSearchOnVehicle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSearchOnVehicle_LinkClicked);
            // 
            // lblCustomerId
            // 
            this.lblCustomerId.AutoSize = true;
            this.lblCustomerId.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerId.ForeColor = System.Drawing.Color.Red;
            this.lblCustomerId.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCustomerId.Location = new System.Drawing.Point(809, 62);
            this.lblCustomerId.Name = "lblCustomerId";
            this.lblCustomerId.Size = new System.Drawing.Size(69, 38);
            this.lblCustomerId.TabIndex = 22;
            this.lblCustomerId.Text = "????";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.Hash_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(890, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 38);
            this.label2.TabIndex = 21;
            this.label2.Text = "معرف العميل:     ";
            // 
            // lblVehicleID
            // 
            this.lblVehicleID.AutoSize = true;
            this.lblVehicleID.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleID.ForeColor = System.Drawing.Color.Red;
            this.lblVehicleID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVehicleID.Location = new System.Drawing.Point(809, 129);
            this.lblVehicleID.Name = "lblVehicleID";
            this.lblVehicleID.Size = new System.Drawing.Size(69, 38);
            this.lblVehicleID.TabIndex = 20;
            this.lblVehicleID.Text = "????";
            this.lblVehicleID.TextChanged += new System.EventHandler(this.lblVehicleID_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.Hash_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(890, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 38);
            this.label1.TabIndex = 19;
            this.label1.Text = "معرف المركبة:     ";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmAddEditRentalBooking
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1142, 887);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditRentalBooking";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة \\ تعديل حجز";
            this.Load += new System.EventHandler(this.frmAddEditRentalBooking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCustomerId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVehicleID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelSearchOnCustomer;
        private System.Windows.Forms.LinkLabel linkLabelSearchOnVehicle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDropOffLocation;
        private System.Windows.Forms.ComboBox cbPickupLocation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblEstimatedCost;
        private System.Windows.Forms.Label lblRentalDays;
        private System.Windows.Forms.Label lblRentalPrice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}