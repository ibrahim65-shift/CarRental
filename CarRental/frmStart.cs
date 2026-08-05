using CarRental.Login;
using CarRental.Settings;
using CarRental_Buisness.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental
{
    public partial class frmStart : Form
    {

        private bool _isCheckingConnection;

        public frmStart()
        {
            InitializeComponent();
        }

        private void llCloseProgram_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }
        private void llSettingConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmSettings frm = new frmSettings())
                frm.ShowDialog();
        }
        private async void timerStart_Tick(object sender, EventArgs e)
        {
            await TryConnectAsync();
        }

        // ================= METHODS =================

        private async Task TryConnectAsync()
        {

            if (_isCheckingConnection)
                return;

            _isCheckingConnection = true;
            timerStart.Stop();

            try
            {

                lblState.Text = "جاري الاتصال بقاعدة البيانات...";

                bool connected = await clsUtil.CheckDatabaseConnection();
                if (connected)
                {
                    OpenLoginForm();
                    return;
                }

                ShowConnectionError();
                await Task.Delay(3000);

            }
            finally
            {

                _isCheckingConnection = false;
                if (!IsDisposed && Visible)
                    timerStart.Start();
            }

        }
        private void OpenLoginForm()
        {
            timerStart.Stop();

            using (frmLogin frm = new frmLogin())
            {
                Hide();
                frm.ShowDialog();
            }

            Close();
        }
        private void ShowConnectionError()
        {
            panelSettings.Visible = true;
            lblState.Text = "فشل الاتصال بقاعدة البيانات، سيتم إعادة المحاولة...";
        }

    }
}
