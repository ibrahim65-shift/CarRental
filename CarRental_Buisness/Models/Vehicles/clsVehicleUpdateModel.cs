using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Vehicles
{
    public class clsVehicleUpdateModel
    {
        public int CurrentMileage { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public enVehicleStatus StatusID { get; set; }
    }
}
