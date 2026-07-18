using CarRental_Buisness.Models.Vehicles;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleMapper
    {
        public static async Task<clsVehicleDto> ToDto(clsVehiclesEntities entity)
        {
            return new clsVehicleDto
            { 
                VehicleID = entity.VehicleID,
                MakeID = entity.MakeID,
                ModelID = entity.ModelID,
                Year = entity.Year,
                CurrentMileage = entity.CurrentMileage,
                RentalPricePerDay = entity.RentalPricePerDay,
                FuelTypeID = entity.FuelTypeID,
                CategoryID = entity.CategoryID,
                PlateNumber = entity.PlateNumber,
                StatusID = entity.StatusID,
                VIN = entity.VIN,
                Color = entity.Color,

                MakeName = await clsMakeData.GetMakeByMakeIDAsync(entity.MakeID),
                ModelName = await clsModelData.GetModelByModelIDAsync(entity.ModelID)
            };

        }
    }
}
