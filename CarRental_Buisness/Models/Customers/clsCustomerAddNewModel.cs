using System;


namespace CarRental_Buisness.Models.Customers
{
    public class clsCustomerAddNewModel
    {
        public int PersonID { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DateTime DriverLicenseExpiry { get; set; }
    }
}
