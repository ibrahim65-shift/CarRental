using CarRental_Entities;
using SharedClass;
using System;

using System.Data;
using System.Data.SqlClient;
using System.Linq;

using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public class clsVehicleReturnData
    {
        public static async Task<clsVehicleReturnEntities> GetVehicleReturnByIDAsync(int returnID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleReturn_GetByID",
                    reader => _MapToVehicleReturn(reader),
                    p => p.Add("@ReturnID", SqlDbType.Int).Value = returnID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetVehicleReturnByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetVehicleReturnByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehicleReturnEntities> GetVehicleReturnByBookingIDAsync(int bookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleReturn_GetByBookingID",
                    reader => _MapToVehicleReturn(reader),
                    p => p.Add("@BookingID", SqlDbType.Int).Value = bookingID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetVehicleReturnByBookingIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetVehicleReturnByBookingIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable returnData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_VehicleReturn_GetPage",
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue;
                    });

                int totalCount = (dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;
                int totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

                dt.Columns.Remove("TotalCount");

                return (dt, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<bool> IsVehicleReturnExistsAsync(int returnID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleReturn_Exists",
                    p => p.Add("@ReturnID", SqlDbType.Int).Value = returnID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.IsVehicleReturnExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.IsVehicleReturnExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsVehicleReturnExistsByBookingIDAsync(int bookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleReturn_ExistsBookingID",
                    p => p.Add("@BookingID", SqlDbType.Int).Value = bookingID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.IsBookingIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.IsBookingIDExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<int?> StartInspectionAsync(clsVehicleReturnEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleReturn_StartInspection",
                    p =>
                    {
                        p.Add("@BookingID", SqlDbType.Int).Value = entity.BookingID;
                        p.Add("@ActualReturnDate", SqlDbType.DateTime2).Value = entity.ActualReturnDate;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                     
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.StartInspectionAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.StartInspectionAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateInspectionAsync(clsVehicleReturnEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleReturn_UpdateInspection",
                    p =>
                    {
                        p.Add("@ReturnID", SqlDbType.Int).Value = entity.ReturnID;
                        p.Add("@MileageEnd", SqlDbType.Int).Value = entity.MileageEnd;
                        p.Add("@FinalCheckNotes", SqlDbType.NVarChar,500).Value = string.IsNullOrWhiteSpace(entity.FinalCheckNotes) ? (object)DBNull.Value : entity.FinalCheckNotes;
                        p.Add("@AdditionalCharges", SqlDbType.Decimal).Value = entity.AdditionalCharges ?? (object)DBNull.Value;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.UpdateInspectionAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.UpdateInspectionAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> FinalizeAsync(clsVehicleReturnEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleReturn_Finalize",
                    p =>
                    {
                        p.Add("@ReturnID", SqlDbType.Int).Value = entity.ReturnID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);

                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.FinalizeAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.FinalizeAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> MarkAsInvoicedAsync(clsVehicleReturnEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleReturn_MarkAsInvoiced",
                    p =>
                    {
                        p.Add("@ReturnID", SqlDbType.Int).Value = entity.ReturnID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.MarkAsInvoicedAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.MarkAsInvoicedAsync (General)", ex);
                return false;
            }
        }
        public static async Task<enReturnStatus?> GetReturnStatusAsync(int returnID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleReturn_GetStatus",
                    p => p.Add("@ReturnID", SqlDbType.Int).Value = returnID );

                if (result == null || result==DBNull.Value)
                    return null;

                return (enReturnStatus)Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetReturnStatusAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleReturnData.GetReturnStatusAsync (General)", ex);
                return null;
            }
        }
        private static clsVehicleReturnEntities _MapToVehicleReturn(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "ReturnID",
                "BookingID",
                "ReturnStatusID",
                "ActualReturnDate",
                "MileageStart",
                "MileageEnd",
                "ConsumedMileage",
                "FinalCheckNotes",
                "AdditionalCharges",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
                );

            return new clsVehicleReturnEntities
            {
                ReturnID = reader.GetInt32(cols["ReturnID"]),
                BookingID = reader.GetInt32(cols["BookingID"]),
                ReturnStatusID = (enReturnStatus)reader.GetInt32(cols["ReturnStatusID"]),
                ActualReturnDate = reader.GetDateTime(cols["ActualReturnDate"]),
                MileageStart = reader.GetInt32(cols["MileageStart"]),
                MileageEnd = reader.GetIntOrNull(cols["MileageEnd"]),
                ConsumedMileage = reader.GetIntOrNull(cols["ConsumedMileage"]),
                FinalCheckNotes = reader.GetStringOrNull(cols["FinalCheckNotes"]),
                AdditionalCharges = reader.GetDecimalOrNull(cols["AdditionalCharges"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"]),
            };

        }
    }
}
