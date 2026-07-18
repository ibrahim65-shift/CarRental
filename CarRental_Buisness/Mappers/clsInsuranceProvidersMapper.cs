using CarRental_Buisness.Models.InsuranceProviders;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsInsuranceProvidersMapper
    {
        public static clsInsuranceProvidersDto ToDto(clsInsuranceProvidersEntities entity)
        {
            return new clsInsuranceProvidersDto
            {
                ProviderName = entity.ProviderName
            };
        }
    }
}
