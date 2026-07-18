using CarRental.Customers.CustomersList.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Customers.CustomersList.Forms
{
    public partial class frmSelectCustomer : Form
    {
        public int? SelectedCustomerId { get; private set; }
        public DateTime DriverLicenseExpiry { get; private set; }
        public frmSelectCustomer()
        {
            InitializeComponent();
            var ctrl = new ctrlCustomers(null, ctrlCustomers.enMode.Selection);
            ctrl.Dock = DockStyle.Fill;

            ctrl.CustomerSelectedId += Ctrl_CustomerSelectedId;
            ctrl.DriverLicenseExpiry += Ctrl_DriverLicenseExpiry;

            Controls.Add(ctrl);
        }

        private void Ctrl_CustomerSelectedId(int customerId)
        {
            SelectedCustomerId = customerId;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Ctrl_DriverLicenseExpiry(DateTime driverLicenseExpiry)
        {
            DriverLicenseExpiry = driverLicenseExpiry;
            DialogResult = DialogResult.OK;
            Close();
        }


   }
}
