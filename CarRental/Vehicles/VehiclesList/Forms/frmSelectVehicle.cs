using CarRental.Vehicles.VehiclesList.Controls;
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
    public partial class frmSelectVehicle : Form
    {
        public int? SelectedvehicleId {  get; private set; }
        public frmSelectVehicle()
        {
            InitializeComponent();

            var ctrl = new ctrlVehicles(null, ctrlVehicles.enMode.Selection);
            ctrl.Dock = DockStyle.Fill;

            ctrl.VehicleSelectedId += Ctrl_VehicleSelectedId;

            Controls.Add(ctrl);
        }

        private void Ctrl_VehicleSelectedId(int vehicleId)
        {
            SelectedvehicleId = vehicleId;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
