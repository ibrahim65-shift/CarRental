using SharedClass;
using System;


namespace CarRental_Buisness.Models.VehicleReturn
{
    public class clsVehicleReturnDto
    {
        public int ReturnID { get; set; }
        public int BookingID { get; set; }
        public enReturnStatus ReturnStatusID { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public int? MileageStart { get; set; }
        public int? MileageEnd { get; set; }
        public int? ConsumedMileage { get; set; }
        public string FinalCheckNotes { get; set; } 
        public decimal? AdditionalCharges { get; set; }
    }
}
