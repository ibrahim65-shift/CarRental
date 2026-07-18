using CarRental_Buisness.Models.RentalBooking;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsRentalBookingMapper
    {
        public static clsRentalBookingDto ToDto(clsRentalBookingEntities entity)
        {
            return new clsRentalBookingDto
            {
                BookingID = entity.BookingID,
                CustomerID = entity.CustomerID,
                VehicleID = entity.VehicleID,
                RentalStartDate = entity.RentalStartDate,
                RentalEndDate = entity.RentalEndDate,
                PickupLocationID = entity.PickupLocationID,
                DropOffLocationID = entity.DropOffLocationID,
                RentalPricePerDay = entity.RentalPricePerDay,
                InitialRentalDays = entity.InitialRentalDays,
                InitialTotalDueAmount = entity.InitialTotalDueAmount,
                InitialCheckNotes = entity.InitialCheckNotes,
                BookingStatusID = entity.BookingStatusID,

            };
        }
    }
}
