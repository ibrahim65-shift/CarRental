using CarRental_Buisness.Models.VehicleStatus;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleStatusMapper
    {
        public static clsVehicleStatusDto ToDto(clsVehicleStatusEntities entity)
        {
            return new clsVehicleStatusDto
            {
                StatusID = entity.StatusID,
                StatusName = entity.StatusName,
            };
        }
    }
}
