using System;


namespace CarRental_Entities
{
    public class clsCustomersEntities
    {
        public int CustomerID { get; set; }
        public int PersonID { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DateTime DriverLicenseExpiry { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }

        public clsPersonEntities Person { get; set; } // Composition
    }
}
