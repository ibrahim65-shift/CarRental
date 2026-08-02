using CarRental_Entities.Dashboard;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public static class clsDashboardData
    {
        public static async Task<clsStatisticsEntity> GetStatisticsAsync()
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Dashboard_GetStatistics", reader => _MapToStatistics(reader));

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetStatisticsAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetStatisticsAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsLatestBookingsEntity>> GetLatestBookingsAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Dashboard_GetLatestBookings", reader => _MapToLatestBookings(reader));
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetLatestBookingsAsync (SQL)", ex);
                return new List<clsLatestBookingsEntity>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetLatestBookingsAsync (General)", ex);
                return new List<clsLatestBookingsEntity>();
            }
        }
        public static async Task<List<clsAlertsEntity>> GetAlertsAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Dashboard_GetAlerts", reader => _MapToAlerts(reader));
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetAlertsAsync (SQL)", ex);
                return new List<clsAlertsEntity>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsDashboardData.GetAlertsAsync (General)", ex);
                return new List<clsAlertsEntity>();
            }
        }

        private static clsStatisticsEntity _MapToStatistics(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                 "CustomersCount",
                 "VehiclesCount",
                 "ActiveBookings",
                 "MonthlyRevenue",
                 "UsersCount",
                 "AvailableVehicles",
                 "RentedVehicles",
                 "MaintenanceVehicles"
                );

           return new clsStatisticsEntity
           {
               CustomersCount = reader.GetInt32(cols["CustomersCount"]),
               VehiclesCount = reader.GetInt32(cols["VehiclesCount"]),
               ActiveBookings = reader.GetInt32(cols["ActiveBookings"]),
               MonthlyRevenue = reader.GetDecimal(cols["MonthlyRevenue"]),
               UsersCount = reader.GetInt32(cols["UsersCount"]),
               AvailableVehicles = reader.GetInt32(cols["AvailableVehicles"]),
               RentedVehicles = reader.GetInt32(cols["RentedVehicles"]),
               MaintenanceVehicles = reader.GetInt32(cols["MaintenanceVehicles"])
           };
        }
        private static clsLatestBookingsEntity _MapToLatestBookings(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "BookingID",
                "Customer",
                "Vehicle",
                "RentalStartDate",
                "RentalEndDate",
                "StatusName"
               );

            return new clsLatestBookingsEntity
            {
                BookingID = reader.GetInt32(cols["BookingID"]),
                Customer = reader.GetString(cols["Customer"]),
                Vehicle = reader.GetString(cols["Vehicle"]),
                RentalStartDate = reader.GetDateTime(cols["RentalStartDate"]),
                RentalEndDate = reader.GetDateTime(cols["RentalEndDate"]),
                StatusName = reader.GetString(cols["StatusName"]),
            };
        }
        private static clsAlertsEntity _MapToAlerts(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
               "AlertType",
               "Title",
               "Description",
               "Severity",
               "ReferenceID"
              );

            return new clsAlertsEntity
            {
                AlertType = (enAlertType)reader.GetByte(cols["AlertType"]),
                Title = reader.GetString(cols["Title"]),
                Description = reader.GetString(cols["Description"]),
                Severity =(enSeverityType) reader.GetByte(cols["Severity"]),
                ReferenceID = reader.GetInt32(cols["ReferenceID"]),
            };
        }

    }
}
