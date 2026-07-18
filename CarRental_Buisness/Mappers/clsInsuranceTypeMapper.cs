using CarRental_Buisness.Models.InsuranceType;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsInsuranceTypeMapper
    {
        public static clsInsuranceTypeDto ToDto(clsInsuranceTypeEntities entity)
        {
            return new clsInsuranceTypeDto
            { 
                InsuranceTypeID = entity.InsuranceTypeID,
                InsuranceType = entity.InsuranceType
            };

        }
    }
}
