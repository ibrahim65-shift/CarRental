using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsRatePlansEntities
    {
        public int RatePlanID { get; set; }
        public enRatePlaneScope RatePlanScope { get; set; }
        public enVehicleCategory? CategoryID { get; set; }
        public int? VehicleID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PricePerDay { get; set; }
        public int? MinDays { get; set; }
        public string Notes { get; set; } // allows null
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
