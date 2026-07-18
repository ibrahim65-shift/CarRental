namespace CarRental.Vehicles.VehicleDamage.Forms
{
    partial class frmAddEditVehicleDamage
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
            this.gbVehicleDamageInfo = new System.Windows.Forms.GroupBox();
            this.pnlContainBookingID = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBookingID = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.numericUpDownEstimatedCost = new System.Windows.Forms.NumericUpDown();
            this.lblVehicleID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbVehicleDamageInfo.SuspendLayout();
            this.pnlContainBookingID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEstimatedCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.vehicleDamage_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(202, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 159);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
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
            this.btnCancel.Location = new System.Drawing.Point(189, 596);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(148, 54);
            this.btnCancel.TabIndex = 3;
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
            this.btnSave.Location = new System.Drawing.Point(21, 596);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbVehicleDamageInfo
            // 
            this.gbVehicleDamageInfo.Controls.Add(this.pnlContainBookingID);
            this.gbVehicleDamageInfo.Controls.Add(this.txtDescription);
            this.gbVehicleDamageInfo.Controls.Add(this.numericUpDownEstimatedCost);
            this.gbVehicleDamageInfo.Controls.Add(this.lblVehicleID);
            this.gbVehicleDamageInfo.Controls.Add(this.label3);
            this.gbVehicleDamageInfo.Controls.Add(this.label1);
            this.gbVehicleDamageInfo.Controls.Add(this.label6);
            this.gbVehicleDamageInfo.Location = new System.Drawing.Point(12, 177);
            this.gbVehicleDamageInfo.Name = "gbVehicleDamageInfo";
            this.gbVehicleDamageInfo.Size = new System.Drawing.Size(757, 394);
            this.gbVehicleDamageInfo.TabIndex = 15;
            this.gbVehicleDamageInfo.TabStop = false;
            this.gbVehicleDamageInfo.Text = "معلومات الضرر للمركبة";
            // 
            // pnlContainBookingID
            // 
            this.pnlContainBookingID.Controls.Add(this.label2);
            this.pnlContainBookingID.Controls.Add(this.lblBookingID);
            this.pnlContainBookingID.Location = new System.Drawing.Point(59, 75);
            this.pnlContainBookingID.Name = "pnlContainBookingID";
            this.pnlContainBookingID.Size = new System.Drawing.Size(276, 47);
            this.pnlContainBookingID.TabIndex = 15;
            this.pnlContainBookingID.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.Hash_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(75, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 38);
            this.label2.TabIndex = 11;
            this.label2.Text = "معرف الحجز:     ";
            // 
            // lblBookingID
            // 
            this.lblBookingID.AutoSize = true;
            this.lblBookingID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBookingID.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookingID.ForeColor = System.Drawing.Color.Red;
            this.lblBookingID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBookingID.Location = new System.Drawing.Point(0, 0);
            this.lblBookingID.Name = "lblBookingID";
            this.lblBookingID.Size = new System.Drawing.Size(70, 41);
            this.lblBookingID.TabIndex = 14;
            this.lblBookingID.Text = "????";
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescription.Location = new System.Drawing.Point(22, 221);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(465, 148);
            this.txtDescription.TabIndex = 2;
            // 
            // numericUpDownEstimatedCost
            // 
            this.numericUpDownEstimatedCost.DecimalPlaces = 2;
            this.numericUpDownEstimatedCost.Location = new System.Drawing.Point(248, 151);
            this.numericUpDownEstimatedCost.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownEstimatedCost.Name = "numericUpDownEstimatedCost";
            this.numericUpDownEstimatedCost.Size = new System.Drawing.Size(239, 39);
            this.numericUpDownEstimatedCost.TabIndex = 1;
            // 
            // lblVehicleID
            // 
            this.lblVehicleID.AutoSize = true;
            this.lblVehicleID.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleID.ForeColor = System.Drawing.Color.Red;
            this.lblVehicleID.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVehicleID.Location = new System.Drawing.Point(441, 75);
            this.lblVehicleID.Name = "lblVehicleID";
            this.lblVehicleID.Size = new System.Drawing.Size(70, 41);
            this.lblVehicleID.TabIndex = 13;
            this.lblVehicleID.Text = "????";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.AdditionalCharges_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(517, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 38);
            this.label3.TabIndex = 12;
            this.label3.Text = "التكلفة المقدرة:     ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.edit_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(517, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 38);
            this.label1.TabIndex = 10;
            this.label1.Text = "الوصف:     ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = global::CarRental.Properties.Resources.Hash_32;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(517, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(220, 38);
            this.label6.TabIndex = 9;
            this.label6.Text = "معرف المركبة:     ";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmAddEditVehicleDamage
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(781, 662);
            this.Controls.Add(this.gbVehicleDamageInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditVehicleDamage";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة \\ تعديل ضرر للمركبة";
            this.Load += new System.EventHandler(this.frmAddEditVehicleDamage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbVehicleDamageInfo.ResumeLayout(false);
            this.gbVehicleDamageInfo.PerformLayout();
            this.pnlContainBookingID.ResumeLayout(false);
            this.pnlContainBookingID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEstimatedCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbVehicleDamageInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBookingID;
        private System.Windows.Forms.Label lblVehicleID;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.NumericUpDown numericUpDownEstimatedCost;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel pnlContainBookingID;
    }
}