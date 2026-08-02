using CarRental_Buisness.Models.Dashboard;
using CarRental_Entities.Dashboard;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsDashboardMapper
    {
        public static clsStatisticsDto ToStatisticsDto(clsStatisticsEntity entity)
        {
            return new clsStatisticsDto
            {
                CustomersCount      = entity. CustomersCount    ,
                VehiclesCount       = entity. VehiclesCount     ,
                ActiveBookings      = entity. ActiveBookings    ,
                MonthlyRevenue      = entity. MonthlyRevenue    ,
                UsersCount          = entity. UsersCount        ,
                AvailableVehicles   = entity. AvailableVehicles ,
                RentedVehicles      = entity. RentedVehicles    ,
                MaintenanceVehicles = entity.MaintenanceVehicles
            };
        }
        public static clsLatestBookingsDto ToLatestBookingsDto(clsLatestBookingsEntity entity)
        {
            return new clsLatestBookingsDto
            {
                BookingID       = entity.BookingID      ,
                Customer        = entity.Customer       ,
                Vehicle         = entity.Vehicle        ,
                RentalStartDate = entity.RentalStartDate,
                RentalEndDate   = entity.RentalEndDate  ,
                StatusName      = entity.StatusName     
            };
        }
        public static clsAlertsDto ToAlertsDto(clsAlertsEntity entity)
        {
            return new clsAlertsDto
            {
                AlertType   = entity. AlertType  ,
                Title       = entity. Title      ,
                Description = entity. Description,
                Severity    = entity. Severity   ,
                ReferenceID = entity.ReferenceID 
            };
        }
    }
}
