using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public class clsBookingStatusData
    {
        public static async Task<clsBookingStatusEntities> GetBookingStatusByIDAsync(int bookingStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_BookingStatus_GetByID",
                    reader => _MapToBookingStatus(reader),
                    p => p.Add("@BookingStatusID", SqlDbType.Int).Value = bookingStatusID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetBookingStatusByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetBookingStatusByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsBookingStatusEntities> GetBookingStatusByStatusNameAsync(string statusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_BookingStatus_GetByStatusName",
                    reader => _MapToBookingStatus(reader),
                    p => p.Add("@statusName", SqlDbType.NVarChar,50).Value = statusName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetBookingStatusByStatusNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetBookingStatusByStatusNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsBookingStatusEntities> bookingStatusData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsBookingStatusEntities>("SP_BookingStatus_GetPage",
                    reader=>
                    {
                        var item = _MapToBookingStatus(reader);

                        if (totalCount == 0)
                            totalCount = Convert.ToInt32(reader["TotalCount"]);

                        return item;
                    },
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue;
                    });

                int totalPages = 0;
                
                if(list.Count > 0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }


                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetPageAsync (SQL)", ex);
                return (new List<clsBookingStatusEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.GetPageAsync (General)", ex);
                return (new List<clsBookingStatusEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsBookingStatusEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_BookingStatus_AddNew",
                    p =>
                    {
                        p.Add("@statusName", SqlDbType.NVarChar, 50).Value = entity.StatusName;
                        p.Add("@Description", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(entity.Description) ? (object)DBNull.Value : entity.Description;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsBookingStatusEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                    await clsSQLHelper.ExecuteNonQueryAsync("SP_BookingStatus_Update",
                    p =>
                    {
                        p.Add("@BookingStatusID", SqlDbType.Int).Value = entity.BookingStatusID;
                        p.Add("@statusName", SqlDbType.NVarChar, 50).Value = entity.StatusName;
                        p.Add("@Description", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(entity.Description) ? (object)DBNull.Value : entity.Description;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int bookingStatusID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_BookingStatus_Delete",
                    p =>
                    {
                        p.Add("@BookingStatusID", SqlDbType.Int).Value = bookingStatusID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsBookingStatusExistsByIDAsync(int bookingStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_BookingStatus_ExistsStatus",
                    p => p.Add("@bookingStatusID", SqlDbType.Int).Value = bookingStatusID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.IsBookingStatusExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.IsBookingStatusExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsBookingStatusExistsByNameAsync(int? BookingStatusID, string statusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_BookingStatus_ExistsByName",
                    p =>
                    {
                        p.Add("@BookingStatusID", SqlDbType.Int).Value = BookingStatusID ?? (object)DBNull.Value;
                        p.Add("@StatusName", SqlDbType.NVarChar, 50).Value = statusName;
                    }
                );

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.IsBookingStatusExistsByNameAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsBookingStatusData.IsBookingStatusExistsByNameAsync (General)", ex);
                return false;
            }
        }
        private static clsBookingStatusEntities _MapToBookingStatus(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "BookingStatusID",
                "StatusName",
                "Description"
                );

            return new clsBookingStatusEntities
            {
                BookingStatusID = reader.GetInt32(cols["BookingStatusID"]),
                StatusName = reader.GetString(cols["StatusName"]),
                Description = reader.GetStringOrNull(cols["Description"])
               
            };

        }
    }
}
