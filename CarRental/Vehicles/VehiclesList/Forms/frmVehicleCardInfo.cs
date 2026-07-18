using CarRental_Buisness.Models.Vehicles;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehiclesList.Forms
{
    public partial class frmVehicleCardInfo : Form
    {
        private int _vehicleID;
        public frmVehicleCardInfo(int vehicleid)
        {
            InitializeComponent();
            _vehicleID = vehicleid;
        }

        private async void frmVehicleCardInfo_Load(object sender, EventArgs e)
        {
            await ctrlVehicleCard1.LoadAsync(()=> new clsVehicleService().GetVehicleByIDAsync(_vehicleID));
        }

    }
}
