using CarRental_Buisness.Models.Vehicles;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsMakesMapper
    {
        public static clsMakesDto ToDto(clsMakesEntities entity)
        {
            return new clsMakesDto
            {
                MakeID = entity.MakeID,
                MakeName = entity.MakeName,
            };
        }
    }
}
