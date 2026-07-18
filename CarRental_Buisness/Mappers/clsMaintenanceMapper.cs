using CarRental_Buisness.Models.Maintenance;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsMaintenanceMapper
    {
        public static clsMaintenanceDto ToDto(clsMaintenanceEntities entity)
        {
            return new clsMaintenanceDto
            { 
                MaintenanceID = entity.MaintenanceID,
                VehicleID = entity.VehicleID,
                Description = entity.Description,
                Cost = entity.Cost,
                Vendor = entity.Vendor,
            };

        }
    }
}
