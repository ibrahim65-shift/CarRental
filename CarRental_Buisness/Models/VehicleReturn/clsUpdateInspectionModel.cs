using System;


namespace CarRental_Buisness.Models.VehicleReturn
{
    public class clsUpdateInspectionModel
    {
        public int? MileageEnd { get; set; }
        public string FinalCheckNotes { get; set; } 
        public decimal? AdditionalCharges { get; set; }
    }
}
