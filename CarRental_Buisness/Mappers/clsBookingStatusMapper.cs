using CarRental_Buisness.Models.BookingStatus;
using CarRental_Entities;


namespace CarRental_Buisness.Mappers
{
    public class clsBookingStatusMapper
    {
        public static clsBookingStatusDto ToDto(clsBookingStatusEntities entity)
        {
            return new clsBookingStatusDto
            { 
              BookingStatusID = entity.BookingStatusID,
              StatusName = entity.StatusName,
              Description   = entity.Description,
            };

        }
    }
}
