namespace CarRental
{
    partial class frmStart
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
            this.lblState = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.llSettingConnection = new System.Windows.Forms.LinkLabel();
            this.llCloseProgram = new System.Windows.Forms.LinkLabel();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CarRental.Properties.Resources.CarRentalIcon_512;
            this.pictureBox1.Location = new System.Drawing.Point(88, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(529, 305);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblState
            // 
            this.lblState.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(29, 337);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(637, 38);
            this.lblState.TabIndex = 7;
            this.lblState.Text = "جاري الاتصال . . .";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(36, 378);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(664, 44);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6;
            // 
            // llSettingConnection
            // 
            this.llSettingConnection.AutoSize = true;
            this.llSettingConnection.Location = new System.Drawing.Point(423, 5);
            this.llSettingConnection.Name = "llSettingConnection";
            this.llSettingConnection.Size = new System.Drawing.Size(238, 32);
            this.llSettingConnection.TabIndex = 2;
            this.llSettingConnection.TabStop = true;
            this.llSettingConnection.Text = "تعديل إعدادات الاتصال";
            this.llSettingConnection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSettingConnection_LinkClicked);
            // 
            // llCloseProgram
            // 
            this.llCloseProgram.AutoSize = true;
            this.llCloseProgram.Location = new System.Drawing.Point(272, 5);
            this.llCloseProgram.Name = "llCloseProgram";
            this.llCloseProgram.Size = new System.Drawing.Size(145, 32);
            this.llCloseProgram.TabIndex = 3;
            this.llCloseProgram.TabStop = true;
            this.llCloseProgram.Text = "إغلاق البرنامج";
            this.llCloseProgram.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCloseProgram_LinkClicked);
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.llSettingConnection);
            this.panelSettings.Controls.Add(this.llCloseProgram);
            this.panelSettings.Location = new System.Drawing.Point(36, 428);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(664, 46);
            this.panelSettings.TabIndex = 9;
            this.panelSettings.Visible = false;
            // 
            // timerStart
            // 
            this.timerStart.Enabled = true;
            this.timerStart.Interval = 5000;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(732, 491);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panelSettings);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStart";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmStart";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.LinkLabel llSettingConnection;
        private System.Windows.Forms.LinkLabel llCloseProgram;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Timer timerStart;
    }
}