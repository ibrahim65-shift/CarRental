using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Entities;

namespace CarRental_Buisness.Mappers
{
    public class clsVehicleReturnMapper
    {
        public static clsVehicleReturnDto ToDto(clsVehicleReturnEntities entity)
        {
            return new clsVehicleReturnDto
            {
                ReturnID = entity.ReturnID,
                BookingID = entity.BookingID,
                ReturnStatusID = entity.ReturnStatusID,
                ActualReturnDate = entity.ActualReturnDate,
                MileageStart = entity.MileageStart,
                MileageEnd = entity.MileageEnd,
                ConsumedMileage = entity.ConsumedMileage,
                FinalCheckNotes = entity.FinalCheckNotes,
                AdditionalCharges = entity.AdditionalCharges,
            };
        }
    }
}
