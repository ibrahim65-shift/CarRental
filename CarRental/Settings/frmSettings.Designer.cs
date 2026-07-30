namespace CarRental.Settings
{
    partial class frmSettings
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.GeneralSettingPage = new System.Windows.Forms.TabPage();
            this.numericUpDownTaxRate = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSaveGeneral = new System.Windows.Forms.Button();
            this.numericUpDownAutoRefresh = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownItems = new System.Windows.Forms.NumericUpDown();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ConnectionSettingsPage = new System.Windows.Forms.TabPage();
            this.numericUpDownConnection = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDataBase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rbNetwork = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.btnSaveConnection = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl1.SuspendLayout();
            this.GeneralSettingPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTaxRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItems)).BeginInit();
            this.ConnectionSettingsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.GeneralSettingPage);
            this.tabControl1.Controls.Add(this.ConnectionSettingsPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(809, 678);
            this.tabControl1.TabIndex = 0;
            // 
            // GeneralSettingPage
            // 
            this.GeneralSettingPage.BackColor = System.Drawing.Color.White;
            this.GeneralSettingPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GeneralSettingPage.Controls.Add(this.numericUpDownTaxRate);
            this.GeneralSettingPage.Controls.Add(this.label9);
            this.GeneralSettingPage.Controls.Add(this.btnSaveGeneral);
            this.GeneralSettingPage.Controls.Add(this.numericUpDownAutoRefresh);
            this.GeneralSettingPage.Controls.Add(this.numericUpDownItems);
            this.GeneralSettingPage.Controls.Add(this.txtCompanyName);
            this.GeneralSettingPage.Controls.Add(this.label3);
            this.GeneralSettingPage.Controls.Add(this.label2);
            this.GeneralSettingPage.Controls.Add(this.label1);
            this.GeneralSettingPage.Location = new System.Drawing.Point(4, 41);
            this.GeneralSettingPage.Name = "GeneralSettingPage";
            this.GeneralSettingPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralSettingPage.Size = new System.Drawing.Size(801, 633);
            this.GeneralSettingPage.TabIndex = 0;
            this.GeneralSettingPage.Text = "الإعدادات العامة";
            // 
            // numericUpDownTaxRate
            // 
            this.numericUpDownTaxRate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownTaxRate.Location = new System.Drawing.Point(107, 438);
            this.numericUpDownTaxRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTaxRate.Name = "numericUpDownTaxRate";
            this.numericUpDownTaxRate.Size = new System.Drawing.Size(626, 45);
            this.numericUpDownTaxRate.TabIndex = 8;
            this.numericUpDownTaxRate.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Image = global::CarRental.Properties.Resources.tax_32;
            this.label9.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label9.Location = new System.Drawing.Point(547, 378);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(224, 38);
            this.label9.TabIndex = 7;
            this.label9.Text = "معدل الضريبة:     ";
            // 
            // btnSaveGeneral
            // 
            this.btnSaveGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveGeneral.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveGeneral.Image = global::CarRental.Properties.Resources.Save_32;
            this.btnSaveGeneral.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveGeneral.Location = new System.Drawing.Point(298, 537);
            this.btnSaveGeneral.Name = "btnSaveGeneral";
            this.btnSaveGeneral.Size = new System.Drawing.Size(227, 58);
            this.btnSaveGeneral.TabIndex = 6;
            this.btnSaveGeneral.Text = "حفظ";
            this.btnSaveGeneral.UseVisualStyleBackColor = true;
            this.btnSaveGeneral.Click += new System.EventHandler(this.btnSaveGeneral_Click);
            // 
            // numericUpDownAutoRefresh
            // 
            this.numericUpDownAutoRefresh.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownAutoRefresh.Location = new System.Drawing.Point(107, 315);
            this.numericUpDownAutoRefresh.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownAutoRefresh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAutoRefresh.Name = "numericUpDownAutoRefresh";
            this.numericUpDownAutoRefresh.Size = new System.Drawing.Size(626, 45);
            this.numericUpDownAutoRefresh.TabIndex = 5;
            this.numericUpDownAutoRefresh.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownItems
            // 
            this.numericUpDownItems.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownItems.Location = new System.Drawing.Point(107, 193);
            this.numericUpDownItems.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownItems.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownItems.Name = "numericUpDownItems";
            this.numericUpDownItems.Size = new System.Drawing.Size(626, 45);
            this.numericUpDownItems.TabIndex = 4;
            this.numericUpDownItems.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyName.Location = new System.Drawing.Point(107, 75);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(626, 39);
            this.txtCompanyName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::CarRental.Properties.Resources.counter_32;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label3.Location = new System.Drawing.Point(456, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(315, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "عدد العناصر المعروضة:    ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.Refresh;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label2.Location = new System.Drawing.Point(345, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(426, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "الوقت التلقائي لتحديث الصفحات:    ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.company_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label1.Location = new System.Drawing.Point(579, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "اسم الشركة:     ";
            // 
            // ConnectionSettingsPage
            // 
            this.ConnectionSettingsPage.BackColor = System.Drawing.Color.White;
            this.ConnectionSettingsPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConnectionSettingsPage.Controls.Add(this.numericUpDownConnection);
            this.ConnectionSettingsPage.Controls.Add(this.label7);
            this.ConnectionSettingsPage.Controls.Add(this.txtPassword);
            this.ConnectionSettingsPage.Controls.Add(this.label6);
            this.ConnectionSettingsPage.Controls.Add(this.txtUserName);
            this.ConnectionSettingsPage.Controls.Add(this.label5);
            this.ConnectionSettingsPage.Controls.Add(this.txtDataBase);
            this.ConnectionSettingsPage.Controls.Add(this.label4);
            this.ConnectionSettingsPage.Controls.Add(this.txtServer);
            this.ConnectionSettingsPage.Controls.Add(this.label8);
            this.ConnectionSettingsPage.Controls.Add(this.rbNetwork);
            this.ConnectionSettingsPage.Controls.Add(this.rbLocal);
            this.ConnectionSettingsPage.Controls.Add(this.btnSaveConnection);
            this.ConnectionSettingsPage.Location = new System.Drawing.Point(4, 41);
            this.ConnectionSettingsPage.Name = "ConnectionSettingsPage";
            this.ConnectionSettingsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConnectionSettingsPage.Size = new System.Drawing.Size(801, 633);
            this.ConnectionSettingsPage.TabIndex = 1;
            this.ConnectionSettingsPage.Text = "إعدادات الاتصال";
            // 
            // numericUpDownConnection
            // 
            this.numericUpDownConnection.Location = new System.Drawing.Point(29, 405);
            this.numericUpDownConnection.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownConnection.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownConnection.Name = "numericUpDownConnection";
            this.numericUpDownConnection.Size = new System.Drawing.Size(456, 39);
            this.numericUpDownConnection.TabIndex = 23;
            this.numericUpDownConnection.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = global::CarRental.Properties.Resources.time_32;
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Location = new System.Drawing.Point(507, 406);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(284, 38);
            this.label7.TabIndex = 22;
            this.label7.Text = "فترة الاتصال : (ثانية)     ";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(29, 328);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(456, 39);
            this.txtPassword.TabIndex = 21;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = global::CarRental.Properties.Resources.Name_32;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(507, 252);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(235, 38);
            this.label6.TabIndex = 20;
            this.label6.Text = "اسم المستخدم :     ";
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Location = new System.Drawing.Point(29, 251);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.PasswordChar = '*';
            this.txtUserName.Size = new System.Drawing.Size(456, 39);
            this.txtUserName.TabIndex = 19;
            this.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::CarRental.Properties.Resources.database_32;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(507, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 38);
            this.label5.TabIndex = 18;
            this.label5.Text = "قاعدة البيانات :     ";
            // 
            // txtDataBase
            // 
            this.txtDataBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataBase.Location = new System.Drawing.Point(29, 175);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtDataBase.Size = new System.Drawing.Size(456, 39);
            this.txtDataBase.TabIndex = 17;
            this.txtDataBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::CarRental.Properties.Resources.password_32;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(507, 329);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 38);
            this.label4.TabIndex = 16;
            this.label4.Text = "كلمة المرور :     ";
            // 
            // txtServer
            // 
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Location = new System.Drawing.Point(29, 101);
            this.txtServer.Name = "txtServer";
            this.txtServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtServer.Size = new System.Drawing.Size(456, 39);
            this.txtServer.TabIndex = 15;
            this.txtServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Image = global::CarRental.Properties.Resources.server_32;
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label8.Location = new System.Drawing.Point(507, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 38);
            this.label8.TabIndex = 14;
            this.label8.Text = "السيرفر:     ";
            // 
            // rbNetwork
            // 
            this.rbNetwork.AutoSize = true;
            this.rbNetwork.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.rbNetwork.Location = new System.Drawing.Point(245, 18);
            this.rbNetwork.Name = "rbNetwork";
            this.rbNetwork.Size = new System.Drawing.Size(126, 49);
            this.rbNetwork.TabIndex = 13;
            this.rbNetwork.Text = "شبكي";
            this.rbNetwork.UseVisualStyleBackColor = true;
            this.rbNetwork.CheckedChanged += new System.EventHandler(this.rbNetwork_CheckedChanged);
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Checked = true;
            this.rbLocal.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.rbLocal.Location = new System.Drawing.Point(423, 18);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(121, 49);
            this.rbLocal.TabIndex = 12;
            this.rbLocal.TabStop = true;
            this.rbLocal.Text = "محلي";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbNetwork.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
            // 
            // btnSaveConnection
            // 
            this.btnSaveConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveConnection.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnSaveConnection.Image = global::CarRental.Properties.Resources.Save_32;
            this.btnSaveConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveConnection.Location = new System.Drawing.Point(245, 526);
            this.btnSaveConnection.Name = "btnSaveConnection";
            this.btnSaveConnection.Size = new System.Drawing.Size(227, 58);
            this.btnSaveConnection.TabIndex = 7;
            this.btnSaveConnection.Text = "حفظ";
            this.btnSaveConnection.UseVisualStyleBackColor = true;
            this.btnSaveConnection.Click += new System.EventHandler(this.btnSaveConnection_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(809, 678);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "الإعدادات";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.GeneralSettingPage.ResumeLayout(false);
            this.GeneralSettingPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTaxRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownItems)).EndInit();
            this.ConnectionSettingsPage.ResumeLayout(false);
            this.ConnectionSettingsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage GeneralSettingPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage ConnectionSettingsPage;
        private System.Windows.Forms.Button btnSaveGeneral;
        private System.Windows.Forms.NumericUpDown numericUpDownAutoRefresh;
        private System.Windows.Forms.NumericUpDown numericUpDownItems;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownConnection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDataBase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbNetwork;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.Button btnSaveConnection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownTaxRate;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}