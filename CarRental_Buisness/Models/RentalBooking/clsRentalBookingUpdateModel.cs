using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.RentalBooking
{
    public class clsRentalBookingUpdateModel
    {
        public int VehicleID { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public int PickupLocationID { get; set; }
        public int DropOffLocationID { get; set; }
        public string InitialCheckNotes { get; set; } // allows null
      
    }
}
