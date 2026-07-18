namespace CarRental.Vehicles.VehiclesList.Forms
{
    partial class frmVehicleCardInfo
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
            this.ctrlVehicleCard1 = new CarRental.Vehicles.VehiclesList.Controls.ctrlVehicleCard();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::CarRental.Properties.Resources.VehicleInfo_512;
            this.pictureBox1.Location = new System.Drawing.Point(456, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(477, 194);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // ctrlVehicleCard1
            // 
            this.ctrlVehicleCard1.BackColor = System.Drawing.Color.White;
            this.ctrlVehicleCard1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctrlVehicleCard1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlVehicleCard1.Location = new System.Drawing.Point(13, 223);
            this.ctrlVehicleCard1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlVehicleCard1.Name = "ctrlVehicleCard1";
            this.ctrlVehicleCard1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctrlVehicleCard1.Size = new System.Drawing.Size(1354, 350);
            this.ctrlVehicleCard1.TabIndex = 3;
            // 
            // frmVehicleCardInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1388, 587);
            this.Controls.Add(this.ctrlVehicleCard1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVehicleCardInfo";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "معلومات المركبة";
            this.Load += new System.EventHandler(this.frmVehicleCardInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Controls.ctrlVehicleCard ctrlVehicleCard1;
    }
}