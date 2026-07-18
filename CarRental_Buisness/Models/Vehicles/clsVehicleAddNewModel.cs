using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Vehicles
{
    public class clsVehicleAddNewModel
    {
        public int MakeID { get; set; }
        public int ModelID { get; set; }
        public int Year { get; set; }
        public int CurrentMileage { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public enFuelType FuelTypeID { get; set; }
        public enVehicleCategory CategoryID { get; set; }
        public string PlateNumber { get; set; }
        public string VIN { get; set; } 
        public string Color { get; set; } 
    }
}
