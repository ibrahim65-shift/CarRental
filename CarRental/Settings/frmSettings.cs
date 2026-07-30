using CarRental.Helper;
using CarRental_Buisness.Helpers;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Settings
{
    public partial class frmSettings : Form
    {
        private const string LOCAL_CONNECTION = "Local";
        private const string NETWORK_CONNECTION = "Network";
        private const string CONNECTION_NAME = "CarRentalDB";
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            _LoadGeneralSettings();
            _LoadConnectionSettings();
        }
        private void btnSaveGeneral_Click(object sender, EventArgs e)
        {
            _ExecuteSafe(_SaveGeneralSettings, "frmSettings.btnSaveGeneral_Click", "حدث خطأ أثناء حفظ الإعدادات العامة");
        }
        private void btnSaveConnection_Click(object sender, EventArgs e)
        {
            _ExecuteSafe(_SaveConnectionSettings, "frmSettings.btnSaveConnection_Click", "حدث خطأ أثناء حفظ إعدادات الاتصال");
        }
        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateConnectionControlState();
        }
        private void rbNetwork_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateConnectionControlState();
        }

        // =================== METHODS ===================

        private void _SaveConnectionSettings()
        {
            if (!_ValidateConnectionSettings())
                return;

            if(!_IsConnectionSettingsChanged())
            {
                clsMessages.ShowInfo("لاتوجد تغييرات");
                return;
            }

            var settings = Properties.Settings.Default;

            settings.ConnectionType = rbLocal.Checked ? LOCAL_CONNECTION : NETWORK_CONNECTION;
            settings.Server = txtServer.Text.Trim();
            settings.DataBase = txtDataBase.Text.Trim();
            settings.UserName = txtUserName.Text.Trim();
            settings.Password = clsConnectionSecurity.Encrypt(txtPassword.Text.Trim());
            settings.ConnectionDuration = (int)numericUpDownConnection.Value;

            settings.Save();

            _SaveConnectionStringToConfiguration();
            _ResetApplication("تم حفظ إعدادات الاتصال بنجاح، سيتم إعادة تشغيل التطبيق الآن لتطبيق التغييرات.");
        }
        private void _SaveGeneralSettings()
        {
            if (!_ValidateGeneralSettings())
                return;

            if (!_IsGeneralSettingsChanged())
            {
                clsMessages.ShowInfo("لاتوجد تغييرات");
                return;
            }

            var settings = Properties.Settings.Default;

            settings.CompanyName = txtCompanyName.Text.Trim();
            settings.NumberOfItems = (int)numericUpDownItems.Value;
            settings.AUTOREFRESH = (int)numericUpDownAutoRefresh.Value;
            settings.TaxRate = numericUpDownTaxRate.Value / 100M;

            Properties.Settings.Default.Save();

            _ResetApplication("تم حفظ إعدادات العامة بنجاح، سيتم إعادة تشغيل التطبيق الآن لتطبيق التغييرات.");
        }
        private void _SaveConnectionStringToConfiguration()
        {
            string connectionString = _BuildConnectionString();

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.ConnectionStrings.ConnectionStrings[CONNECTION_NAME] != null)
            {
                config.ConnectionStrings.ConnectionStrings[CONNECTION_NAME].ConnectionString = connectionString;
            }
            else
            {
                config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(CONNECTION_NAME, connectionString, "System.Data.SqlClient"));
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
        private void _LoadGeneralSettings()
        {
            txtCompanyName.Text = Properties.Settings.Default.CompanyName;
            numericUpDownItems.Value = _ClampNumericValue(Properties.Settings.Default.NumberOfItems, numericUpDownItems);
            numericUpDownAutoRefresh.Value = _ClampNumericValue(Properties.Settings.Default.AUTOREFRESH, numericUpDownAutoRefresh);
            numericUpDownTaxRate.Value = _ClampNumericValue(Properties.Settings.Default.TaxRate * 100, numericUpDownTaxRate);
        }
        private void _LoadConnectionSettings()
        {
            var settings = Properties.Settings.Default;
            string connectionType = settings.ConnectionType;

            if (connectionType == LOCAL_CONNECTION)
                rbLocal.Checked = true;
            else if (connectionType == NETWORK_CONNECTION)
                rbNetwork.Checked = true;
            else
                rbLocal.Checked = true;

            txtServer.Text   =settings.Server;
            txtDataBase.Text =settings.DataBase;
            txtUserName.Text =settings.UserName;
            txtPassword.Text = string.IsNullOrWhiteSpace(settings.Password) ? string.Empty : clsConnectionSecurity.Decrypt(settings.Password);
            numericUpDownConnection.Value = _ClampNumericValue(settings.ConnectionDuration, numericUpDownConnection);
            _UpdateConnectionControlState();
        }
        private string _BuildConnectionString()
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text.Trim(),
                InitialCatalog = txtDataBase.Text.Trim(),
                ConnectTimeout = (int)numericUpDownConnection.Value,
                TrustServerCertificate = true
            };

            if (rbLocal.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = Properties.Settings.Default.UserName;
                builder.Password = clsConnectionSecurity.Decrypt(Properties.Settings.Default.Password);
            }

            return builder.ConnectionString;
        }
        private void _UpdateConnectionControlState()
        {
            bool isNetwork = rbNetwork.Checked;

            txtUserName.Enabled = isNetwork;
            txtPassword.Enabled = isNetwork;
            numericUpDownConnection.Enabled = isNetwork;
        }
        private void _ExecuteSafe(Action action , string source , string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException(source, ex);
                clsMessages.ShowError(errorMessage);
            }
        }
        private decimal _ClampNumericValue(decimal value , NumericUpDown control)
        {
            if(value < control.Minimum)
                return control.Minimum;

            if (value > control.Maximum)
                return control.Maximum;

            return value;
        }
        private bool _ValidateConnectionSettings()
        {
            errorProvider1.Clear();

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                errorProvider1.SetError(txtServer, "اسم الخادم مطلوب");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtDataBase.Text))
            {
                errorProvider1.SetError(txtDataBase, "اسم قاعدة البيانات مطلوب");
                isValid = false;
            }

            if (rbNetwork.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    errorProvider1.SetError(txtUserName, "اسم المستخدم مطلوب");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    errorProvider1.SetError(txtPassword, "كلمة المرور مطلوبة");
                    isValid = false;
                }
            }

            return isValid;
        }
        private bool _ValidateGeneralSettings()
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                errorProvider1.SetError(txtCompanyName,"اسم الشركة مطلوب");
                return false;
            }

            return true;
        }
        private void _ResetApplication(string message)
        {
            clsMessages.ShowSuccess(message);
            Application.Restart();
        }
        private bool _IsConnectionSettingsChanged()
        {
            var settings = Properties.Settings.Default;

            string connectionType = rbLocal.Checked ? LOCAL_CONNECTION : NETWORK_CONNECTION;

            string currentPassword = txtPassword.Text.Trim();
            string savedPassword = string.IsNullOrWhiteSpace(settings.Password) 
                ? string.Empty : clsConnectionSecurity.Decrypt(settings.Password);

            if (settings.ConnectionType != connectionType)
                return true;

            if (settings.Server != txtServer.Text.Trim())
                return true;

            if (settings.DataBase != txtDataBase.Text.Trim())
                return true;

            if (settings.UserName != txtUserName.Text.Trim())
                return true;

            if (!string.Equals(savedPassword,currentPassword,StringComparison.Ordinal))
                return true;

            if (settings.ConnectionDuration != (int)numericUpDownConnection.Value)
                return true;


            return false;
        }
        private bool _IsGeneralSettingsChanged()
        {
            var settings = Properties.Settings.Default;

            if (settings.CompanyName != txtCompanyName.Text.Trim())
                return true;

            if (settings.NumberOfItems != (int)numericUpDownItems.Value)
                return true;

            if (settings.AUTOREFRESH != (int) numericUpDownAutoRefresh.Value)
                return true;

            if (settings.TaxRate != numericUpDownTaxRate.Value / 100M)
                return true;


            return false;
        }
    }
}
