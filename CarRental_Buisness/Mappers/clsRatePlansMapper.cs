using CarRental_Buisness.Models.RatePlans;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsRatePlansMapper
    {
        public static clsRatePlansDto ToDto(clsRatePlansEntities entity)
        {
            return new clsRatePlansDto
            {
                RatePlanID = entity.RatePlanID,
                RatePlanScope = entity.RatePlanScope,
                CategoryID = entity.CategoryID,
                VehicleID = entity.VehicleID,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                MinDays = entity.MinDays,
                Notes = entity.Notes,
                PricePerDay = entity.PricePerDay,
                IsActive = entity.IsActive
            };
        }
    }
}
