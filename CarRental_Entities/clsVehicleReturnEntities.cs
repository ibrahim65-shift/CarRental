using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsVehicleReturnEntities
    {
        public int ReturnID { get; set; }
        public int BookingID { get; set; }
        public enReturnStatus ReturnStatusID { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public int MileageStart { get; set; }
        public int? MileageEnd { get; set; }
        public int? ConsumedMileage { get; set; }
        public string FinalCheckNotes { get; set; } // allows null
        public decimal? AdditionalCharges { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
