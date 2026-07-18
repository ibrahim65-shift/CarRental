using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsMaintenanceEntities
    {
        public int MaintenanceID { get; set; }
        public int VehicleID { get; set; }
        public string Description { get; set; }// allows null
        public decimal Cost { get; set; }
        public string Vendor { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
