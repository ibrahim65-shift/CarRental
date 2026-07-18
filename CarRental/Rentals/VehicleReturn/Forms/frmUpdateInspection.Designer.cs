namespace CarRental.Rentals.VehicleReturn.Forms
{
    partial class frmUpdateInspection
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
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFinalCheckNotes = new System.Windows.Forms.TextBox();
            this.numericUpDownAdditionalCharges = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMileageEnd = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentMileage = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdditionalCharges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMileageEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.VehicleInfo_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(359, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 153);
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
            this.btnCancel.Location = new System.Drawing.Point(194, 546);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 54);
            this.btnCancel.TabIndex = 4;
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
            this.btnSave.Location = new System.Drawing.Point(12, 546);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = global::CarRental.Properties.Resources.Mileage_32;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Location = new System.Drawing.Point(304, 187);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(249, 38);
            this.label7.TabIndex = 15;
            this.label7.Text = "العداد عند الإرجاع:    ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.AdditionalCharges_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(871, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 38);
            this.label1.TabIndex = 16;
            this.label1.Text = "رسوم إضافية؟    ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.Notes_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(825, 333);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(277, 38);
            this.label2.TabIndex = 17;
            this.label2.Text = "الملاحظات النهائية:      ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.Mileage_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(830, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 38);
            this.label3.TabIndex = 18;
            this.label3.Text = "العداد عند الاستلام:     ";
            // 
            // txtFinalCheckNotes
            // 
            this.txtFinalCheckNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFinalCheckNotes.Location = new System.Drawing.Point(22, 379);
            this.txtFinalCheckNotes.Multiline = true;
            this.txtFinalCheckNotes.Name = "txtFinalCheckNotes";
            this.txtFinalCheckNotes.Size = new System.Drawing.Size(1073, 144);
            this.txtFinalCheckNotes.TabIndex = 3;
            // 
            // numericUpDownAdditionalCharges
            // 
            this.numericUpDownAdditionalCharges.DecimalPlaces = 2;
            this.numericUpDownAdditionalCharges.Location = new System.Drawing.Point(507, 260);
            this.numericUpDownAdditionalCharges.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownAdditionalCharges.Name = "numericUpDownAdditionalCharges";
            this.numericUpDownAdditionalCharges.Size = new System.Drawing.Size(343, 39);
            this.numericUpDownAdditionalCharges.TabIndex = 2;
            this.numericUpDownAdditionalCharges.ThousandsSeparator = true;
            // 
            // numericUpDownMileageEnd
            // 
            this.numericUpDownMileageEnd.Location = new System.Drawing.Point(22, 186);
            this.numericUpDownMileageEnd.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDownMileageEnd.Name = "numericUpDownMileageEnd";
            this.numericUpDownMileageEnd.Size = new System.Drawing.Size(263, 39);
            this.numericUpDownMileageEnd.TabIndex = 1;
            this.numericUpDownMileageEnd.ThousandsSeparator = true;
            // 
            // lblCurrentMileage
            // 
            this.lblCurrentMileage.AutoSize = true;
            this.lblCurrentMileage.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentMileage.Location = new System.Drawing.Point(700, 187);
            this.lblCurrentMileage.Name = "lblCurrentMileage";
            this.lblCurrentMileage.Size = new System.Drawing.Size(69, 38);
            this.lblCurrentMileage.TabIndex = 23;
            this.lblCurrentMileage.Text = "????";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmUpdateInspection
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1121, 612);
            this.Controls.Add(this.lblCurrentMileage);
            this.Controls.Add(this.numericUpDownMileageEnd);
            this.Controls.Add(this.numericUpDownAdditionalCharges);
            this.Controls.Add(this.txtFinalCheckNotes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateInspection";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تحديث الفحص";
            this.Load += new System.EventHandler(this.frmUpdateInspection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdditionalCharges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMileageEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFinalCheckNotes;
        private System.Windows.Forms.NumericUpDown numericUpDownAdditionalCharges;
        private System.Windows.Forms.NumericUpDown numericUpDownMileageEnd;
        private System.Windows.Forms.Label lblCurrentMileage;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}