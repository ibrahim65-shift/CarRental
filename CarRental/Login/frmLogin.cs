using CarRental.Helper;
using CarRental_Buisness;
using CarRental_Buisness.Helpers;
using SharedClass;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Login
{
    public partial class frmLogin : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd,int Msg,IntPtr wParam,string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _InitializeControls();
            LoadRememberedCredentials();
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
           await LoginAsync();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsMessages.ShowInfo("يرجى التواصل مع مدير النظام");
        }

        // ================= METHODS ============
        private async Task LoginAsync()
        {

            if (!ValidateInput())
                return;

            btnLogin.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {

                string userName = txtUserName.Text.Trim();
                string password = txtPassword.Text;

                var result = await clsLoginService.LoginAsync(userName, password);

                if (!result.Success)
                {

                    clsMessages.ShowError(result.ErrorMessage);

                    txtPassword.Clear();
                    txtPassword.Focus();

                    return;
                }

                SaveRememberMe(userName, password);
                Hide();

                using (frmMain frm = new frmMain(this))
                    frm.ShowDialog();

                Close();

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmLogin.LoginAsync", ex);
                clsMessages.ShowError("حدث خطأ أثناء تسجيل الدخول.");
            }
            finally
            {
                Cursor = Cursors.Default;
                btnLogin.Enabled = true;
            }

        }
        private bool ValidateInput()
        {

            errorProvider1.Clear();

            bool valid = true;
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "اسم المستخدم مطلوب");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "كلمة المرور مطلوبة");
                valid = false;
            }

            return valid;
        }
        private void SaveRememberMe(string userName, string password)
        {

            if (chkRememberMe.Checked)
                clsUtil.RemeberUserNameAndPassword(userName, password);
            else
                clsUtil.DeleteCredentialsFromRegistry();

        }
        private void LoadRememberedCredentials()
        {

            string userName = string.Empty;
            string password = string.Empty;

            if (clsUtil.GetStoredCredential(ref userName, ref password))
            {
                txtUserName.Text = userName;
                txtPassword.Text = password;
                chkRememberMe.Checked = true;
            }

        }
        private void _InitializeControls()
        {
            txtPassword.UseSystemPasswordChar = true;

            SendMessage(txtUserName.Handle,EM_SETCUEBANNER,IntPtr.Zero,"اسم المستخدم");
            SendMessage(txtPassword.Handle,EM_SETCUEBANNER,IntPtr.Zero,"كلمة المرور");

            txtUserName.Focus();
        }
    }
}