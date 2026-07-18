using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Maintenance
{
    public class clsMaintenanceAddNewModel
    {
        public int VehicleID { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Vendor { get; set; }
    }
}
