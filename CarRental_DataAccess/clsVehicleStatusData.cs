using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarRental_DataAccess
{
    public class clsVehicleStatusData
    {
        public static async Task<clsVehicleStatusEntities> GetVehicleStatusByIDAsync(int statusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleStatus_GetByID",
                    reader=>_MapToVehcileStatus(reader) , 
                    p=>p.Add("@StatusID", SqlDbType.Int).Value=statusID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusByIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehicleStatusEntities> GetVehicleStatusByNameAsync(string statusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleStatus_GetByName",
                   reader => _MapToVehcileStatus(reader),
                   p => p.Add("@StatusName", SqlDbType.NVarChar,100).Value = statusName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsVehicleStatusEntities> statusData , int TotalPages)> GetVehicleStatusPageAsync
            (int PageNumber , int PageSize , string FilterColumn=null, string FilterValue=null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsVehicleStatusEntities>("SP_VehicleStatus_GetPage",
                    reader =>
                    {
                        var item = _MapToVehcileStatus(reader);
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
                if(list.Count> 0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }

                return (list,totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusPageAsync (SQL)", ex);
                return (new List<clsVehicleStatusEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.GetVehicleStatusPageAsync (General)", ex);
                return (new List<clsVehicleStatusEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsVehicleStatusEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleStatus_AddNew",
                    p => p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName);

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsVehicleStatusEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleStatus_Update",
                    p =>
                    {
                        p.Add("@StatusID", SqlDbType.Int).Value = entity.StatusID;
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int statusID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleStatus_Delete",
                   p =>
                   {
                       p.Add("@StatusID", SqlDbType.Int).Value = statusID;
                       p.Add(isSuccessParam);
                   });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsStatusIDExistsAsync(int statusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleStatus_ExistsStatusID",
                   p =>
                   {
                       p.Add("@StatusID", SqlDbType.Int).Value = statusID;
                   });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.IsStatusIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.IsStatusIDExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsStatusNameExistsAsync(int? statusID , string statusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleStatus_ExistsStatusName",
                   p =>
                   {
                       p.Add("@StatusID", SqlDbType.Int).Value = statusID ?? (object)DBNull.Value;
                       p.Add("@StatusName", SqlDbType.NVarChar,100).Value = statusName;
                   });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.IsStatusNameExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleStatusData.IsStatusNameExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsVehicleStatusEntities _MapToVehcileStatus(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "StatusID",
                "StatusName"
                );

            return new clsVehicleStatusEntities
            {
               StatusID = reader.GetInt32(cols["StatusID"]),
               StatusName = reader.GetString(cols["StatusName"])
            };

        }
    }
}
