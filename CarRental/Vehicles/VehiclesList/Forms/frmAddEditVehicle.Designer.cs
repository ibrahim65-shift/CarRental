namespace CarRental.Vehicles.VehiclesList.Forms
{
    partial class frmAddEditVehicle
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
            this.cbMake = new System.Windows.Forms.ComboBox();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbVehicleInfo = new System.Windows.Forms.GroupBox();
            this.numericUpDownCurrentMileage = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRentalPrice = new System.Windows.Forms.NumericUpDown();
            this.cbVehicleStatus = new System.Windows.Forms.ComboBox();
            this.lblVehicleStatus = new System.Windows.Forms.Label();
            this.pnlColorPreview = new System.Windows.Forms.Panel();
            this.txtColor = new System.Windows.Forms.TextBox();
            this.btnChooseColor = new System.Windows.Forms.Button();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.txtPlateNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbVehicleCategory = new System.Windows.Forms.ComboBox();
            this.cbFuelType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownYear = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbVehicleInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentMileage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRentalPrice)).BeginInit();
            this.pnlColorPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMake
            // 
            this.cbMake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMake.FormattingEnabled = true;
            this.cbMake.Location = new System.Drawing.Point(610, 116);
            this.cbMake.Name = "cbMake";
            this.cbMake.Size = new System.Drawing.Size(458, 40);
            this.cbMake.TabIndex = 0;
            this.cbMake.SelectionChangeCommitted += new System.EventHandler(this.cbMake_SelectionChangeCommitted);
            // 
            // cbModel
            // 
            this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(261, 115);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(322, 40);
            this.cbModel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.VehicleInfo_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(351, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 153);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // gbVehicleInfo
            // 
            this.gbVehicleInfo.Controls.Add(this.numericUpDownCurrentMileage);
            this.gbVehicleInfo.Controls.Add(this.numericUpDownRentalPrice);
            this.gbVehicleInfo.Controls.Add(this.cbVehicleStatus);
            this.gbVehicleInfo.Controls.Add(this.lblVehicleStatus);
            this.gbVehicleInfo.Controls.Add(this.pnlColorPreview);
            this.gbVehicleInfo.Controls.Add(this.btnChooseColor);
            this.gbVehicleInfo.Controls.Add(this.txtVIN);
            this.gbVehicleInfo.Controls.Add(this.txtPlateNumber);
            this.gbVehicleInfo.Controls.Add(this.label9);
            this.gbVehicleInfo.Controls.Add(this.label8);
            this.gbVehicleInfo.Controls.Add(this.cbVehicleCategory);
            this.gbVehicleInfo.Controls.Add(this.cbFuelType);
            this.gbVehicleInfo.Controls.Add(this.label7);
            this.gbVehicleInfo.Controls.Add(this.label6);
            this.gbVehicleInfo.Controls.Add(this.label5);
            this.gbVehicleInfo.Controls.Add(this.label4);
            this.gbVehicleInfo.Controls.Add(this.numericUpDownYear);
            this.gbVehicleInfo.Controls.Add(this.label3);
            this.gbVehicleInfo.Controls.Add(this.label2);
            this.gbVehicleInfo.Controls.Add(this.label1);
            this.gbVehicleInfo.Controls.Add(this.cbMake);
            this.gbVehicleInfo.Controls.Add(this.cbModel);
            this.gbVehicleInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbVehicleInfo.Location = new System.Drawing.Point(12, 198);
            this.gbVehicleInfo.Name = "gbVehicleInfo";
            this.gbVehicleInfo.Size = new System.Drawing.Size(1092, 583);
            this.gbVehicleInfo.TabIndex = 6;
            this.gbVehicleInfo.TabStop = false;
            this.gbVehicleInfo.Text = "معلومات المركبة";
            // 
            // numericUpDownCurrentMileage
            // 
            this.numericUpDownCurrentMileage.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownCurrentMileage.Location = new System.Drawing.Point(412, 187);
            this.numericUpDownCurrentMileage.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownCurrentMileage.Name = "numericUpDownCurrentMileage";
            this.numericUpDownCurrentMileage.Size = new System.Drawing.Size(313, 45);
            this.numericUpDownCurrentMileage.TabIndex = 3;
            // 
            // numericUpDownRentalPrice
            // 
            this.numericUpDownRentalPrice.DecimalPlaces = 2;
            this.numericUpDownRentalPrice.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRentalPrice.Location = new System.Drawing.Point(412, 248);
            this.numericUpDownRentalPrice.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownRentalPrice.Name = "numericUpDownRentalPrice";
            this.numericUpDownRentalPrice.Size = new System.Drawing.Size(313, 45);
            this.numericUpDownRentalPrice.TabIndex = 4;
            // 
            // cbVehicleStatus
            // 
            this.cbVehicleStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVehicleStatus.FormattingEnabled = true;
            this.cbVehicleStatus.Location = new System.Drawing.Point(261, 509);
            this.cbVehicleStatus.Name = "cbVehicleStatus";
            this.cbVehicleStatus.Size = new System.Drawing.Size(288, 40);
            this.cbVehicleStatus.TabIndex = 10;
            this.cbVehicleStatus.Visible = false;
            // 
            // lblVehicleStatus
            // 
            this.lblVehicleStatus.AutoSize = true;
            this.lblVehicleStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleStatus.Image = global::CarRental.Properties.Resources.fuelType_32;
            this.lblVehicleStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVehicleStatus.Location = new System.Drawing.Point(580, 511);
            this.lblVehicleStatus.Name = "lblVehicleStatus";
            this.lblVehicleStatus.Size = new System.Drawing.Size(196, 38);
            this.lblVehicleStatus.TabIndex = 19;
            this.lblVehicleStatus.Text = "حالة المركبة:     ";
            this.lblVehicleStatus.Visible = false;
            // 
            // pnlColorPreview
            // 
            this.pnlColorPreview.Controls.Add(this.txtColor);
            this.pnlColorPreview.Location = new System.Drawing.Point(105, 255);
            this.pnlColorPreview.Name = "pnlColorPreview";
            this.pnlColorPreview.Size = new System.Drawing.Size(258, 39);
            this.pnlColorPreview.TabIndex = 18;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtColor.Location = new System.Drawing.Point(41, 0);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(217, 39);
            this.txtColor.TabIndex = 17;
            // 
            // btnChooseColor
            // 
            this.btnChooseColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseColor.Image = global::CarRental.Properties.Resources.color_32;
            this.btnChooseColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChooseColor.Location = new System.Drawing.Point(105, 193);
            this.btnChooseColor.Name = "btnChooseColor";
            this.btnChooseColor.Size = new System.Drawing.Size(258, 52);
            this.btnChooseColor.TabIndex = 5;
            this.btnChooseColor.Text = "اضغط لاختيار اللون";
            this.btnChooseColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChooseColor.UseVisualStyleBackColor = true;
            this.btnChooseColor.Click += new System.EventHandler(this.btnChooseColor_Click);
            // 
            // txtVIN
            // 
            this.txtVIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVIN.Location = new System.Drawing.Point(38, 440);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(286, 39);
            this.txtVIN.TabIndex = 9;
            // 
            // txtPlateNumber
            // 
            this.txtPlateNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlateNumber.Location = new System.Drawing.Point(639, 441);
            this.txtPlateNumber.Name = "txtPlateNumber";
            this.txtPlateNumber.Size = new System.Drawing.Size(286, 39);
            this.txtPlateNumber.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Image = global::CarRental.Properties.Resources.VIN_32;
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.Location = new System.Drawing.Point(349, 442);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 38);
            this.label9.TabIndex = 13;
            this.label9.Text = "الشاصي:     ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Image = global::CarRental.Properties.Resources.plateNumber_32;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label8.Location = new System.Drawing.Point(931, 441);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 38);
            this.label8.TabIndex = 12;
            this.label8.Text = "اللوحة:     ";
            // 
            // cbVehicleCategory
            // 
            this.cbVehicleCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVehicleCategory.FormattingEnabled = true;
            this.cbVehicleCategory.Location = new System.Drawing.Point(105, 348);
            this.cbVehicleCategory.Name = "cbVehicleCategory";
            this.cbVehicleCategory.Size = new System.Drawing.Size(258, 40);
            this.cbVehicleCategory.TabIndex = 7;
            // 
            // cbFuelType
            // 
            this.cbFuelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFuelType.FormattingEnabled = true;
            this.cbFuelType.Location = new System.Drawing.Point(563, 346);
            this.cbFuelType.Name = "cbFuelType";
            this.cbFuelType.Size = new System.Drawing.Size(288, 40);
            this.cbFuelType.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = global::CarRental.Properties.Resources.vehicleCategory_32;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Location = new System.Drawing.Point(387, 346);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 38);
            this.label7.TabIndex = 9;
            this.label7.Text = "الفئة:     ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = global::CarRental.Properties.Resources.fuelType_32;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(857, 342);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 38);
            this.label6.TabIndex = 8;
            this.label6.Text = "نوع الوقود:     ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::CarRental.Properties.Resources.rentalPrice_32;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(745, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(275, 38);
            this.label5.TabIndex = 6;
            this.label5.Text = "سعر الإيجار اليومي:     ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::CarRental.Properties.Resources.Mileage_32;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(745, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 38);
            this.label4.TabIndex = 4;
            this.label4.Text = "المسافة المقطوعة حاليا:     ";
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.Location = new System.Drawing.Point(49, 116);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.Size = new System.Drawing.Size(174, 39);
            this.numericUpDownYear.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.year_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(105, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "السنة:     ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.model_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(387, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "اختر الموديل:     ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.Make_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(883, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "اختر الماركة:     ";
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
            this.btnCancel.Location = new System.Drawing.Point(193, 811);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 54);
            this.btnCancel.TabIndex = 11;
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
            this.btnSave.Location = new System.Drawing.Point(12, 811);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmAddEditVehicle
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1116, 877);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbVehicleInfo);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditVehicle";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة / تعديل مركبة";
            this.Load += new System.EventHandler(this.frmAddEditVehicle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbVehicleInfo.ResumeLayout(false);
            this.gbVehicleInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrentMileage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRentalPrice)).EndInit();
            this.pnlColorPreview.ResumeLayout(false);
            this.pnlColorPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMake;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbVehicleInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbVehicleCategory;
        private System.Windows.Forms.ComboBox cbFuelType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnChooseColor;
        private System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.TextBox txtPlateNumber;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnlColorPreview;
        private System.Windows.Forms.TextBox txtColor;
        private System.Windows.Forms.ComboBox cbVehicleStatus;
        private System.Windows.Forms.Label lblVehicleStatus;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.NumericUpDown numericUpDownRentalPrice;
        private System.Windows.Forms.NumericUpDown numericUpDownCurrentMileage;
    }
}