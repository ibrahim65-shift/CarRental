using CarRental_Buisness.Models.VehicleInsurance;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleInsuranceMapper
    {
        public static clsVehicleInsuranceDto ToDto(clsVehicleInsuranceEntities entity)
        {
            return new clsVehicleInsuranceDto
            {
                InsuranceID = entity.InsuranceID,
                VehicleID = entity.VehicleID,
                PolicyNumber = entity.PolicyNumber,
                ProviderID = entity.ProviderID,
                InsuranceTypeID = entity.InsuranceTypeID,
                InsuranceCost = entity.InsuranceCost,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                Notes = entity.Notes
            };
        }
    }
}
