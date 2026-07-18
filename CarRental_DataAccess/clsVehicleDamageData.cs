using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CarRental_Entities;
using System.Data.SqlClient;
using SharedClass;

namespace CarRental_DataAccess
{
    public class clsVehicleDamageData
    {
        public static async Task<clsVehicleDamageEntities> GetVehicleDamageByIDAsync(int damageID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleDamage_GetByID",
                    reader => _MapToVehicleDamage(reader),
                    p => p.Add("@DamageID", SqlDbType.Int).Value = damageID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsVehicleDamageEntities>> GetVehicleDamageByVehilceIDAsync(int vehilceID)
        {
            try
            {
               return await clsSQLHelper.ExecuteReaderAsync("SP_VehicleDamage_GetByVehicleID",
                    reader => _MapToVehicleDamage(reader),
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = vehilceID);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByVehilceIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByVehilceIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehicleDamageEntities> GetVehicleDamageByBookingIDAsync(int bookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleDamage_GetByBookingID",
                    reader => _MapToVehicleDamage(reader),
                    p => p.Add("@BookingID", SqlDbType.Int).Value = bookingID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByBookingIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamageByBookingIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable DamageData , int TotalPages)> GetVehicleDamagePageAsync
            (int PageNumber , int PageSize , string FilterColumn=null , string FilterValue=null)
        {

            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_VehicleDamage_GetPage",
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue; ;
                    });

                int totalCount = (dt.Rows.Count>0) ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;
                int totalPages = (int)Math.Ceiling(totalCount/(double)PageSize);

                dt.Columns.Remove("TotalCount");

                return (dt, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamagePageAsync (SQL)", ex);
                return (new DataTable() , 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.GetVehicleDamagePageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsVehicleDamageEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleDamage_AddNew",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@BookingID", SqlDbType.Int).Value = entity.BookingID??(object)DBNull.Value;
                        p.Add("@Description", SqlDbType.NVarChar,500).Value = entity.Description;
                        p.Add("@EstimatedCost", SqlDbType.Decimal).Value = entity.EstimatedCost??(object)DBNull.Value;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsVehicleDamageEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleDamage_Update",
                    p =>
                    {
                        p.Add("@DamageID", SqlDbType.Int).Value = entity.DamageID;
                        p.Add("@Description", SqlDbType.NVarChar, 500).Value = entity.Description;
                        p.Add("@EstimatedCost", SqlDbType.Decimal).Value = entity.EstimatedCost ?? (object)DBNull.Value;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int damageID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleDamage_Delete",
                   p =>
                   {
                       p.Add("@DamageID", SqlDbType.Int).Value = damageID;
                       p.Add(isSuccessParam);
                   });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsDamageIDExistsAsync(int damageID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleDamage_ExistsDamageID",
                    p=>p.Add("@DamageID", SqlDbType.Int).Value=damageID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.IsDamageIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.IsDamageIDExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsDuplicateBookingIDAsync(int? damageID , int? bookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleDamage_IsDuplicateBookingID",
                    p=>
                    {
                        p.Add("@DamageID", SqlDbType.Int).Value = damageID ?? (object)DBNull.Value;
                        p.Add("@BookingID", SqlDbType.Int).Value = bookingID ?? (object)DBNull.Value;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.IsDuplicateBookingIDAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleDamageData.IsDuplicateBookingIDAsync (General)", ex);
                return false;
            }
        }
        
        private static clsVehicleDamageEntities _MapToVehicleDamage(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "DamageID",
                "VehicleID",
                "BookingID",
                "Description",
                "EstimatedCost",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
                );

            return new clsVehicleDamageEntities
            {
                DamageID = reader.GetInt32(cols["DamageID"]),
                VehicleID = reader.GetInt32(cols["VehicleID"]),
                BookingID = reader.GetIntOrNull(cols["BookingID"]),
                Description = reader.GetString(cols["Description"]),
                EstimatedCost = reader.GetDecimalOrNull(cols["EstimatedCost"]),
                IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };
        }
    }
}
