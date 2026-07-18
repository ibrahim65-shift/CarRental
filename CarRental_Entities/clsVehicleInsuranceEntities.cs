using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsVehicleInsuranceEntities
    {
        public int InsuranceID { get; set; }
        public int VehicleID { get; set; }
        public string PolicyNumber { get; set; }
        public enInsuranceProviders ProviderID { get; set; }
        public enInsuranceType InsuranceTypeID { get; set; }
        public decimal InsuranceCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
