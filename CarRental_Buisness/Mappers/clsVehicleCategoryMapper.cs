using CarRental_Buisness.Models.VehicleCategory;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleCategoryMapper
    {
        public static clsVehicleCategoryDto ToDto(clsVehicleCategoryEntities entity)
        {
            return new clsVehicleCategoryDto
            {
                CategoryID = entity.CategoryID,
                CategoryName = entity.CategoryName,
            };
        }
    }
}
