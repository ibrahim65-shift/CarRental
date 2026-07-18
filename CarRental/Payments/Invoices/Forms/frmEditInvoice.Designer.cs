namespace CarRental.Payments.Invoices.Forms
{
    partial class frmEditInvoice
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblInvoiceNumber = new System.Windows.Forms.Label();
            this.numericUpDownAdditionalCharges = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLateFees = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDiscountAmount = new System.Windows.Forms.NumericUpDown();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdditionalCharges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLateFees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiscountAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.Invoice_512;
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(156, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(403, 171);
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
            this.btnCancel.Location = new System.Drawing.Point(193, 634);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 54);
            this.btnCancel.TabIndex = 13;
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
            this.btnSave.Location = new System.Drawing.Point(12, 634);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 54);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "  حفظ";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.AdditionalCharges_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(506, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 38);
            this.label1.TabIndex = 15;
            this.label1.Text = "رسوم إضافية:     ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.Notes_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(506, 496);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 38);
            this.label2.TabIndex = 16;
            this.label2.Text = "الملاحظات:     ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.LateFees_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(506, 352);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 38);
            this.label3.TabIndex = 17;
            this.label3.Text = "رسوم تأخير:     ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::CarRental.Properties.Resources.Discount_32;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(506, 424);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(197, 38);
            this.label4.TabIndex = 18;
            this.label4.Text = "مبلغ الخصم:     ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::CarRental.Properties.Resources.invoice_32;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(506, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(195, 38);
            this.label5.TabIndex = 19;
            this.label5.Text = "رقم الفاتورة:     ";
            // 
            // lblInvoiceNumber
            // 
            this.lblInvoiceNumber.AutoSize = true;
            this.lblInvoiceNumber.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceNumber.ForeColor = System.Drawing.Color.Red;
            this.lblInvoiceNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblInvoiceNumber.Location = new System.Drawing.Point(134, 208);
            this.lblInvoiceNumber.Name = "lblInvoiceNumber";
            this.lblInvoiceNumber.Size = new System.Drawing.Size(69, 38);
            this.lblInvoiceNumber.TabIndex = 20;
            this.lblInvoiceNumber.Text = "????";
            // 
            // numericUpDownAdditionalCharges
            // 
            this.numericUpDownAdditionalCharges.DecimalPlaces = 2;
            this.numericUpDownAdditionalCharges.Location = new System.Drawing.Point(89, 283);
            this.numericUpDownAdditionalCharges.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownAdditionalCharges.Name = "numericUpDownAdditionalCharges";
            this.numericUpDownAdditionalCharges.Size = new System.Drawing.Size(383, 39);
            this.numericUpDownAdditionalCharges.TabIndex = 21;
            this.numericUpDownAdditionalCharges.ThousandsSeparator = true;
            // 
            // numericUpDownLateFees
            // 
            this.numericUpDownLateFees.DecimalPlaces = 2;
            this.numericUpDownLateFees.Location = new System.Drawing.Point(89, 355);
            this.numericUpDownLateFees.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownLateFees.Name = "numericUpDownLateFees";
            this.numericUpDownLateFees.Size = new System.Drawing.Size(383, 39);
            this.numericUpDownLateFees.TabIndex = 22;
            this.numericUpDownLateFees.ThousandsSeparator = true;
            // 
            // numericUpDownDiscountAmount
            // 
            this.numericUpDownDiscountAmount.DecimalPlaces = 2;
            this.numericUpDownDiscountAmount.Location = new System.Drawing.Point(89, 427);
            this.numericUpDownDiscountAmount.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownDiscountAmount.Name = "numericUpDownDiscountAmount";
            this.numericUpDownDiscountAmount.Size = new System.Drawing.Size(383, 39);
            this.numericUpDownDiscountAmount.TabIndex = 23;
            this.numericUpDownDiscountAmount.ThousandsSeparator = true;
            // 
            // txtNotes
            // 
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Location = new System.Drawing.Point(12, 499);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(460, 99);
            this.txtNotes.TabIndex = 24;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmEditInvoice
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(714, 700);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.numericUpDownDiscountAmount);
            this.Controls.Add(this.numericUpDownLateFees);
            this.Controls.Add(this.numericUpDownAdditionalCharges);
            this.Controls.Add(this.lblInvoiceNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditInvoice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تعديل الفاتورة";
            this.Load += new System.EventHandler(this.frmEditInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAdditionalCharges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLateFees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiscountAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblInvoiceNumber;
        private System.Windows.Forms.NumericUpDown numericUpDownAdditionalCharges;
        private System.Windows.Forms.NumericUpDown numericUpDownLateFees;
        private System.Windows.Forms.NumericUpDown numericUpDownDiscountAmount;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}