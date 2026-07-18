using SharedClass;
using System;


namespace CarRental_Buisness.Models.VehicleInsurance
{
    public class clsVehicleInsuranceUpdateModel
    {

        public enInsuranceType InsuranceTypeID { get; set; }
        public decimal InsuranceCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
}
