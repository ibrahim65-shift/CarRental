using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsVehiclesEntities
    {
        public int VehicleID { get; set; }
        public int MakeID { get; set; }
        public int ModelID { get; set; }
        public int Year { get; set; }
        public int CurrentMileage { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public enFuelType FuelTypeID { get; set; }
        public enVehicleCategory CategoryID { get; set; }
        public string PlateNumber { get; set; }
        public enVehicleStatus StatusID { get; set; }
        public string VIN { get; set; } 
        public string Color { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
