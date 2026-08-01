using CarRental.ContactUs;
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

namespace CarRental.AboutMe
{
    public partial class ctrlAboutMe : UserControl
    {
        public ctrlAboutMe()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmContactUs frm = new frmContactUs("imoh0683@gmail.com", "استفسار بخصوص نظام إدارة تأجير السيارات"))
                frm.ShowDialog();
        }
    }
}
