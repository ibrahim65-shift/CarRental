using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsVehicleDamageEntities
    {
        public int DamageID { get; set; }
        public int VehicleID { get; set; }
        public int? BookingID { get; set; }
        public string Description { get; set; }
        public decimal? EstimatedCost { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
