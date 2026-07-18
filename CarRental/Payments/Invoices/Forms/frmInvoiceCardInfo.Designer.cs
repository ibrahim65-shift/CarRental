namespace CarRental.Payments.Invoices.Forms
{
    partial class frmInvoiceCardInfo
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctrlInvoicesCard1 = new CarRental.Payments.Invoices.Controls.ctrlInvoicesCard();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CarRental.Properties.Resources.Invoice_512;
            this.pictureBox1.Location = new System.Drawing.Point(330, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(513, 168);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ctrlInvoicesCard1
            // 
            this.ctrlInvoicesCard1.BackColor = System.Drawing.Color.White;
            this.ctrlInvoicesCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctrlInvoicesCard1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlInvoicesCard1.Location = new System.Drawing.Point(13, 215);
            this.ctrlInvoicesCard1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlInvoicesCard1.Name = "ctrlInvoicesCard1";
            this.ctrlInvoicesCard1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctrlInvoicesCard1.Size = new System.Drawing.Size(1159, 345);
            this.ctrlInvoicesCard1.TabIndex = 1;
            // 
            // frmInvoiceCardInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1185, 579);
            this.Controls.Add(this.ctrlInvoicesCard1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvoiceCardInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تفاصيل الفاتورة";
            this.Load += new System.EventHandler(this.frmInvoiceCardInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Controls.ctrlInvoicesCard ctrlInvoicesCard1;
    }
}