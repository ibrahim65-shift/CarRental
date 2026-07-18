using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.VehicleDamage
{
    public class clsVehicleDamageAddNewModel
    {
        public int VehicleID { get; set; }
        public int? BookingID { get; set; }
        public string Description { get; set; }
        public decimal? EstimatedCost { get; set; }
    }
}
