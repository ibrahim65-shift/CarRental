namespace CarRental.Rentals.RatePlans.Forms
{
    partial class frmAddEditRatePlans
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblVehicleID = new System.Windows.Forms.Label();
            this.linkLabelSearchOnVehicle = new System.Windows.Forms.LinkLabel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.numericUpDownMinDays = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRentalPrice = new System.Windows.Forms.NumericUpDown();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.labelVehicleIDTitle = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.labelCategory = new System.Windows.Forms.Label();
            this.cbVehicleCategory = new System.Windows.Forms.ComboBox();
            this.cbRatePlanScope = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRentalPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.ratePlan_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(277, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(439, 191);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
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
            this.btnCancel.Location = new System.Drawing.Point(189, 729);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 54);
            this.btnCancel.TabIndex = 8;
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
            this.btnSave.Location = new System.Drawing.Point(8, 729);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblVehicleID);
            this.groupBox1.Controls.Add(this.linkLabelSearchOnVehicle);
            this.groupBox1.Controls.Add(this.txtNotes);
            this.groupBox1.Controls.Add(this.numericUpDownMinDays);
            this.groupBox1.Controls.Add(this.numericUpDownRentalPrice);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.labelVehicleIDTitle);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.labelCategory);
            this.groupBox1.Controls.Add(this.cbVehicleCategory);
            this.groupBox1.Controls.Add(this.cbRatePlanScope);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 209);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1009, 503);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "معلومات خطط الأسعار";
            // 
            // lblVehicleID
            // 
            this.lblVehicleID.AutoSize = true;
            this.lblVehicleID.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleID.ForeColor = System.Drawing.Color.Red;
            this.lblVehicleID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVehicleID.Location = new System.Drawing.Point(668, 136);
            this.lblVehicleID.Name = "lblVehicleID";
            this.lblVehicleID.Size = new System.Drawing.Size(69, 38);
            this.lblVehicleID.TabIndex = 18;
            this.lblVehicleID.Text = "????";
            this.lblVehicleID.Visible = false;
            // 
            // linkLabelSearchOnVehicle
            // 
            this.linkLabelSearchOnVehicle.AutoSize = true;
            this.linkLabelSearchOnVehicle.Location = new System.Drawing.Point(87, 79);
            this.linkLabelSearchOnVehicle.Name = "linkLabelSearchOnVehicle";
            this.linkLabelSearchOnVehicle.Size = new System.Drawing.Size(279, 32);
            this.linkLabelSearchOnVehicle.TabIndex = 18;
            this.linkLabelSearchOnVehicle.TabStop = true;
            this.linkLabelSearchOnVehicle.Text = "اضغط للبحث عن مركبة . . .";
            this.linkLabelSearchOnVehicle.Visible = false;
            this.linkLabelSearchOnVehicle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSearchOnVehicle_LinkClicked);
            // 
            // txtNotes
            // 
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Location = new System.Drawing.Point(18, 344);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(769, 132);
            this.txtNotes.TabIndex = 7;
            // 
            // numericUpDownMinDays
            // 
            this.numericUpDownMinDays.Location = new System.Drawing.Point(18, 261);
            this.numericUpDownMinDays.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownMinDays.Name = "numericUpDownMinDays";
            this.numericUpDownMinDays.Size = new System.Drawing.Size(268, 39);
            this.numericUpDownMinDays.TabIndex = 6;
            // 
            // numericUpDownRentalPrice
            // 
            this.numericUpDownRentalPrice.DecimalPlaces = 2;
            this.numericUpDownRentalPrice.Location = new System.Drawing.Point(491, 261);
            this.numericUpDownRentalPrice.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownRentalPrice.Name = "numericUpDownRentalPrice";
            this.numericUpDownRentalPrice.Size = new System.Drawing.Size(296, 39);
            this.numericUpDownRentalPrice.TabIndex = 5;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(18, 192);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(268, 39);
            this.dtpEndDate.TabIndex = 4;
            // 
            // labelVehicleIDTitle
            // 
            this.labelVehicleIDTitle.AutoSize = true;
            this.labelVehicleIDTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVehicleIDTitle.Image = global::CarRental.Properties.Resources.Hash_32;
            this.labelVehicleIDTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelVehicleIDTitle.Location = new System.Drawing.Point(757, 136);
            this.labelVehicleIDTitle.Name = "labelVehicleIDTitle";
            this.labelVehicleIDTitle.Size = new System.Drawing.Size(220, 38);
            this.labelVehicleIDTitle.TabIndex = 7;
            this.labelVehicleIDTitle.Text = "معرف المركبة:     ";
            this.labelVehicleIDTitle.Visible = false;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(491, 191);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(297, 39);
            this.dtpStartDate.TabIndex = 3;
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCategory.Image = global::CarRental.Properties.Resources.Category_32;
            this.labelCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCategory.Location = new System.Drawing.Point(317, 77);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(116, 38);
            this.labelCategory.TabIndex = 5;
            this.labelCategory.Text = "الفئة:     ";
            // 
            // cbVehicleCategory
            // 
            this.cbVehicleCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVehicleCategory.FormattingEnabled = true;
            this.cbVehicleCategory.Location = new System.Drawing.Point(18, 75);
            this.cbVehicleCategory.Name = "cbVehicleCategory";
            this.cbVehicleCategory.Size = new System.Drawing.Size(268, 40);
            this.cbVehicleCategory.TabIndex = 2;
            // 
            // cbRatePlanScope
            // 
            this.cbRatePlanScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRatePlanScope.FormattingEnabled = true;
            this.cbRatePlanScope.Location = new System.Drawing.Point(491, 75);
            this.cbRatePlanScope.Name = "cbRatePlanScope";
            this.cbRatePlanScope.Size = new System.Drawing.Size(297, 40);
            this.cbRatePlanScope.TabIndex = 1;
            this.cbRatePlanScope.SelectionChangeCommitted += new System.EventHandler(this.cbRatePlanScope_SelectionChangeCommitted);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = global::CarRental.Properties.Resources.startDate_32;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Location = new System.Drawing.Point(794, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(193, 38);
            this.label7.TabIndex = 6;
            this.label7.Text = "بداية الخطة:     ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::CarRental.Properties.Resources.Notes_32;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(794, 331);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 38);
            this.label5.TabIndex = 4;
            this.label5.Text = "الملاحظات:     ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::CarRental.Properties.Resources.expiryDate_32;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(292, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 38);
            this.label4.TabIndex = 3;
            this.label4.Text = "نهاية الخطة:     ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.counter_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(315, 256);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "عدد الأيام:     ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.rentalPrice_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(794, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "سعر الإيجار:     ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.ratePlanScope_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(794, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "نطاق الخطة:     ";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmAddEditRatePlans
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1029, 795);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditRatePlans";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة \\ تعديل خطط الأسعار";
            this.Load += new System.EventHandler(this.frmAddEditRatePlans_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRentalPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelVehicleIDTitle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRatePlanScope;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.NumericUpDown numericUpDownMinDays;
        private System.Windows.Forms.NumericUpDown numericUpDownRentalPrice;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cbVehicleCategory;
        private System.Windows.Forms.Label lblVehicleID;
        private System.Windows.Forms.LinkLabel linkLabelSearchOnVehicle;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}