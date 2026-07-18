using CarRental_Buisness.Services.People;
using System;
using System.Windows.Forms;

namespace CarRental.Customers.People.Forms
{
    public partial class frmPersonCardInfo : Form
    {
        private int _PersonID;
        public frmPersonCardInfo(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private async void frmPersonCardInfo_Load(object sender, EventArgs e)
        {
            await ctrlPersonCard1.LoadAsync(()=>new clsPersonService().GetPersonByIDAsync(_PersonID));
        }
    }
}
