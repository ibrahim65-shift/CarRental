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
    public class clsMaintenanceData
    {
        public static async Task<clsMaintenanceEntities> GetMaintenanceByMaintenanceIDAsync(int maintenanceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Maintenance_GetByMaintenanceID",
                    reader=>_MapToMaintenance(reader),
                    p=>p.Add("@MaintenanceID", SqlDbType.Int).Value=maintenanceID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenanceByMaintenanceIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenanceByMaintenanceIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsMaintenanceEntities>> GetMaintenanceByVehicleIDAsync(int vehicleID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Maintenance_GetByVehicleID",
                    reader => _MapToMaintenance(reader),
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = vehicleID);

            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenanceByVehicleIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenanceByVehicleIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsMaintenanceEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Maintenance_AddNew",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@Description", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(entity.Description) ? (object)DBNull.Value : entity.Description;
                        p.Add("@Cost", SqlDbType.Decimal).Value= entity.Cost;
                        p.Add("@Vendor", SqlDbType.NVarChar, 200).Value = entity.Vendor.Trim();
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value= entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsMaintenanceEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Maintenance_Update",
                    p =>
                    {
                        p.Add("@MaintenanceID", SqlDbType.Int).Value = entity.MaintenanceID;
                        p.Add("@Description", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(entity.Description) ? (object)DBNull.Value : entity.Description;
                        p.Add("@Cost", SqlDbType.Decimal).Value = entity.Cost;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);

                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int MaintenanceID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Maintenance_Delete",
                    p =>
                    {
                        p.Add("@MaintenanceID", SqlDbType.Int).Value =MaintenanceID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(DataTable maintenanceData , int TotalPages)> GetMaintenancePageAsync
            (int PageNumber , int PageSize , string FilterColumn=null,string FilterValue=null)
        {
            try
            {
                DataTable dt = await clsSQLHelper.ExecuteDataTableAsync("SP_Maintenance_GetPage",
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value=PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value=PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar,128).Value= string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn; ;
                        p.Add("@FilterValue", SqlDbType.NVarChar,200).Value= string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue; ;
                    });

                int totalCount = dt.Rows.Count>0 ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;
                int totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

                dt.Columns.Remove("TotalCount");

                return (dt,totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenancePageAsync (SQL)", ex);
                return (new DataTable() , 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.GetMaintenancePageAsync (General)", ex);
                return (new DataTable(), 0);
            }

        }
        public static async Task<bool> IsMaintenanceExistsAsync(int MaintenanceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Maintenance_ExistsMaintenanceID",
                    p=>p.Add("@MaintenanceID" , SqlDbType.Int).Value= MaintenanceID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.IsMaintenanceExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.IsMaintenanceExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsVehicleIDExistsAsync(int VehicleID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Maintenance_IsVehicleIDExists",
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.IsVehicleIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMaintenanceData.IsVehicleIDExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsMaintenanceEntities _MapToMaintenance(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "MaintenanceID",
                "VehicleID",
                "Description",
                "Cost",
                "Vendor",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
                );

            return new clsMaintenanceEntities
            { 
               MaintenanceID = reader.GetInt32(cols["MaintenanceID"]),
               VehicleID = reader.GetInt32(cols["VehicleID"]),
               Description = reader.GetStringOrNull(cols["Description"]),
               Cost = reader.GetDecimal(cols["Cost"]),
               Vendor = reader.GetString(cols["Vendor"]),
               IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
               CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
               CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
               EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
               EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };

        }
    }
}
