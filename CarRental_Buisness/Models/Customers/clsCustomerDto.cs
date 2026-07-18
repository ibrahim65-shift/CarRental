using CarRental_Buisness.Models.People;
using SharedClass;
using System;


namespace CarRental_Buisness.Models.Customers
{
    public class clsCustomerDto
    {
        public int CustomerID { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DateTime DriverLicenseExpiry { get; set; }
        public int PersonID { get; set; }

        public clsPersonDto PersonInfo { get; set; }
    }
}
