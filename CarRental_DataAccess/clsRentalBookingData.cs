using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public class clsRentalBookingData
    {
        public static async Task<clsRentalBookingEntities> GetRentalBookingByIDAsync(int BookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_RentalBooking_GetByID",
                    reader => _MapToRentalBooking(reader),
                    p => p.Add("@BookingID", SqlDbType.Int).Value =BookingID );

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsRentalBookingEntities>> GetRentalBookingByCustomerIDAsync(int CustomerID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_RentalBooking_GetByCustomerID",
                    reader => _MapToRentalBooking(reader),
                    p => p.Add("@CustomerID", SqlDbType.Int).Value = CustomerID);

                
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByCustomerIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByCustomerIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsRentalBookingEntities>> GetRentalBookingByVehicleIDAsync(int VehicleID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_RentalBooking_GetByVehicleID",
                    reader => _MapToRentalBooking(reader),
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID);

               
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByVehicleIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetRentalBookingByVehicleIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable rentalBookingData, int TotalPage)> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_RentalBooking_GetPage",
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue;
                    });

                int TotalCount = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;
                int TotalPage = (int)Math.Ceiling(TotalCount / (double)PageSize);

                dt.Columns.Remove("TotalCount");

                return (dt, TotalPage);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsRentalBookingEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RentalBooking_AddNew",
                    p =>
                    {
                        p.Add("@CustomerID", SqlDbType.Int).Value = entity.CustomerID;
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@RentalPricePerDay", SqlDbType.Decimal).Value = entity.RentalPricePerDay;
                        p.Add("@RentalStartDate", SqlDbType.DateTime2).Value = entity.RentalStartDate;
                        p.Add("@RentalEndDate", SqlDbType.DateTime2).Value = entity.RentalEndDate;
                        p.Add("@PickupLocationID", SqlDbType.Int).Value = entity.PickupLocationID;
                        p.Add("@DropOffLocationID", SqlDbType.Int).Value = entity.DropOffLocationID;
                        p.Add("@InitialCheckNotes", SqlDbType.NVarChar,500).Value = string.IsNullOrWhiteSpace(entity.InitialCheckNotes)?(object)DBNull.Value : entity.InitialCheckNotes;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsRentalBookingEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RentalBooking_Update",
                    p =>
                    {
                        p.Add("@BookingID", SqlDbType.Int).Value = entity.BookingID;
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@RentalPricePerDay", SqlDbType.Decimal).Value = entity.RentalPricePerDay;
                        p.Add("@RentalStartDate", SqlDbType.DateTime2).Value = entity.RentalStartDate;
                        p.Add("@RentalEndDate", SqlDbType.DateTime2).Value = entity.RentalEndDate;
                        p.Add("@PickupLocationID", SqlDbType.Int).Value = entity.PickupLocationID;
                        p.Add("@DropOffLocationID", SqlDbType.Int).Value = entity.DropOffLocationID;
                        p.Add("@InitialCheckNotes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(entity.InitialCheckNotes) ? (object)DBNull.Value : entity.InitialCheckNotes;
                        p.Add("@BookingStatusID", SqlDbType.Int).Value = entity.BookingStatusID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> UpdateStatusAsync(int BookingID , int NewStatusID , int EditedByUserID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RentalBooking_UpdateStatus",
                    p =>
                    {
                        p.Add("@BookingID", SqlDbType.Int).Value=BookingID;
                        p.Add("@NewStatusID", SqlDbType.Int).Value=NewStatusID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value=EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.UpdateStatusAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.UpdateStatusAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int BookingID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RentalBooking_Delete",
                    p =>
                    {
                        p.Add("@BookingID", SqlDbType.Int).Value = BookingID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsRentalBookingExistsAsync(int BookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RentalBooking_Exists",
                    p => p.Add("@BookingID", SqlDbType.Int).Value = BookingID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.IsRentalBookingExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.IsRentalBookingExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> OverLapsAsync(int? BookingID ,int VehicleID, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RentalBooking_OverLaps",
                    p =>
                    {
                        p.Add("@BookingID", SqlDbType.Int).Value = BookingID?? (object)DBNull.Value;
                        p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
                        p.Add("@StartDate", SqlDbType.DateTime2).Value = startDate;
                        p.Add("@EndDate", SqlDbType.DateTime2).Value = endDate;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.OverLapsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.OverLapsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> HasVehicleReturnAsync(int BookingID)
        { 
            try
            {
                return clsSQLHelper.ToInt32Safe(await clsSQLHelper.ExecuteScalarAsync("SP_RentalBooking_HasVehicleReturn",
                    p => p.Add("@BookingID", SqlDbType.Int).Value = BookingID))==1;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.HasVehicleReturnAsync (SQL)", ex);
                return false;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsRentalBookingData.HasVehicleReturnAsync (General)", ex);
                return false;
            }
        }
        private static clsRentalBookingEntities _MapToRentalBooking(SqlDataReader reader)
        {
            var cols1 = clsSQLHelper.GetOrdinal(reader,
                "BookingID",
                "CustomerID",
                "VehicleID",
                "RentalStartDate",
                "RentalEndDate",
                "PickupLocationID",
                "DropOffLocationID",
                "RentalPricePerDay",
                "InitialRentalDays",
                "InitialTotalDueAmount",
                "InitialCheckNotes",
                "BookingStatusID",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
                );


            return new clsRentalBookingEntities
            {
                BookingID = reader.GetInt32(cols1["BookingID"]),
                CustomerID = reader.GetInt32(cols1["CustomerID"]),
                VehicleID = reader.GetInt32(cols1["VehicleID"]),
                RentalStartDate = reader.GetDateTime(cols1["RentalStartDate"]),
                RentalEndDate = reader.GetDateTime(cols1["RentalEndDate"]),
                PickupLocationID = reader.GetInt32(cols1["PickupLocationID"]),
                DropOffLocationID = reader.GetInt32(cols1["DropOffLocationID"]),
                RentalPricePerDay = reader.GetDecimal(cols1["RentalPricePerDay"]),
                InitialRentalDays = reader.GetInt32(cols1["InitialRentalDays"]),
                InitialTotalDueAmount = reader.GetDecimal(cols1["InitialTotalDueAmount"]),
                InitialCheckNotes = reader.GetStringOrNull(cols1["InitialCheckNotes"]),
                BookingStatusID = (enBookingStatus)reader.GetInt32(cols1["BookingStatusID"]),
                IsDeleted = reader.GetBoolean(cols1["IsDeleted"]),
                CreatedDate = reader.GetDateTime(cols1["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols1["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols1["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols1["EditedByUserID"]),
            };

        }
    }
}
