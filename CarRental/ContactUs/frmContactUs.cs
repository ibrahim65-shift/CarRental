using CarRental.Helper;
using CarRental_Buisness.Helpers;
using System;
using System.Windows.Forms;

namespace CarRental.ContactUs
{
    public partial class frmContactUs : Form
    {
        private readonly string _email;

        public frmContactUs(string email, string subject = "")
        {
            InitializeComponent();

            _email = email;

            lblEmail.Text = email;
            txtSubject.Text = subject;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                clsMessages.ShowInfo("الرجاء إدخال موضوع الرسالة.");
                txtSubject.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBody.Text))
            {
                clsMessages.ShowInfo("الرجاء كتابة الرسالة.");
                txtBody.Focus();
                return;
            }

            if (!clsCommunication.SendEmail(_email,txtSubject.Text.Trim(),txtBody.Text.Trim()))
            {
                clsMessages.ShowError("تعذر فتح برنامج البريد الإلكتروني.");
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
