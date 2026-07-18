using CarRental_Buisness.Models.Vehicles;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsModelMapper
    {
        public static clsModelsDto ToDto(clsModelsEntities entity)
        {
            return new clsModelsDto
            {
                 ModelID = entity.ModelID,
                 ModelName = entity.ModelName
            };

        }
    }
}
