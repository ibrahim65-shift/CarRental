using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass;

namespace CarRental_Entities
{
    public class clsRentalBookingEntities
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public int PickupLocationID { get; set; }
        public int DropOffLocationID { get; set; }
        public decimal RentalPricePerDay { get; set; }
        public int InitialRentalDays { get; set; }
        public decimal InitialTotalDueAmount { get; set; }
        public string InitialCheckNotes { get; set; } // allows null
        public enBookingStatus BookingStatusID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
