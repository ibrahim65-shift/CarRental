namespace CarRental.Customers.CustomersList.Forms
{
    partial class frmCustomerCardInfo
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
            this.ctrlCustomerCard1 = new CarRental.Customers.CustomersList.Controls.ctrlCustomerCard();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlCustomerCard1
            // 
            this.ctrlCustomerCard1.BackColor = System.Drawing.Color.White;
            this.ctrlCustomerCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctrlCustomerCard1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlCustomerCard1.Location = new System.Drawing.Point(13, 235);
            this.ctrlCustomerCard1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlCustomerCard1.Name = "ctrlCustomerCard1";
            this.ctrlCustomerCard1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctrlCustomerCard1.Size = new System.Drawing.Size(1191, 372);
            this.ctrlCustomerCard1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CarRental.Properties.Resources.CustomerCar_512;
            this.pictureBox1.Location = new System.Drawing.Point(445, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(327, 215);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // frmCustomerCardInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1217, 621);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ctrlCustomerCard1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerCardInfo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "معلومات العميل";
            this.Load += new System.EventHandler(this.frmCustomerCardInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ctrlCustomerCard ctrlCustomerCard1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}