using CarRental_Buisness.Models.Locations;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsLocationMapper
    {
        public static clsLocationDto ToDto(clsLocationsEntities entity)
        {
            return new clsLocationDto
            {
                LocationID = entity.LocationID,
                Name = entity.Name,
                Address = entity.Address,
                Phone = entity.Phone,
                IsActive = entity.IsActive,
            };

        }
        public static clsLocationComboBoxDto ToComboBoxDto(clsLocationsEntities entity)
        {
            return new clsLocationComboBoxDto
            {
                LocationID = entity.LocationID,
                DisplayName = $"{entity.Name} - {entity.Address}"
            };
        }
    }
}
