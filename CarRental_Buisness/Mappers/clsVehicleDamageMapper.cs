using CarRental_Buisness.Models.VehicleDamage;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleDamageMapper
    {
        public static clsVehicleDamageDto ToDto(clsVehicleDamageEntities entity)
        {
            return new clsVehicleDamageDto
            {
                DamageID = entity.DamageID,
                VehicleID = entity.VehicleID,
                BookingID = entity.BookingID,
                Description = entity.Description,
                EstimatedCost  = entity.EstimatedCost
            };

        }
    }
}
