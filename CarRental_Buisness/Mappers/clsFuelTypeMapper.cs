using CarRental_Buisness.Models.FuelTypes;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsFuelTypeMapper
    {
        public static clsFuelTypeDto ToDto(clsFuelTypesEntities entity)
        {
            return new clsFuelTypeDto
            { 
               FuelTypeID = entity.FuelTypeID,
               FuelTypeName = entity.FuelTypeName,
            };

        }
    }
}
