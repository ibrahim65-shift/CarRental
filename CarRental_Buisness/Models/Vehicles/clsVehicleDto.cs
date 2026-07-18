
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass;

namespace CarRental_Buisness.Models.Vehicles
{
    public class clsVehicleDto
    {
        public int VehicleID { get; set; }
        public int MakeID { get; set; }
        public string MakeName { get; set; }
        public int ModelID { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public int CurrentMileage { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public enFuelType FuelTypeID { get; set; }
        public enVehicleCategory CategoryID { get; set; }
        public string PlateNumber { get; set; }
        public enVehicleStatus StatusID { get; set; }
        public string VIN { get; set; } 
        public string Color { get; set; } 
    }
}
