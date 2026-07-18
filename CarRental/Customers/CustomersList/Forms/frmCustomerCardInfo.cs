using CarRental_Buisness.Services.Customers;
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
    public partial class frmCustomerCardInfo : Form
    {
        private int _customerID;
        public frmCustomerCardInfo(int customerID)
        {
            InitializeComponent();
            _customerID = customerID;
        }

        private async void frmCustomerCardInfo_Load(object sender, EventArgs e)
        {
            await ctrlCustomerCard1.LoadAsync(() => new clsCustomerService().GetCustomerByCustomerIDAsync(_customerID));
        }
    }
}
