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
    public static class clsPermissionData
    {
        public static async Task<clsPermissionEntity> GetByIDAsync(int id)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Permissions_GetById", reader => _MapToPermission(reader),
                    p => p.Add("@permissionId", SqlDbType.Int).Value = id);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPermissionData.GetByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPermissionData.GetByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsPermissionEntity>> GetAllAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Permissions_GetAll", reader => _MapToPermission(reader));
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsPermissionData.GetAllAsync (SQL)", ex);
                return new List<clsPermissionEntity>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPermissionData.GetAllAsync (General)", ex);
                return new List<clsPermissionEntity>();
            }
        }
        public static async Task<bool> IsExistsAsync(int permissionId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Permissions_Exists", 
                    p => p.Add("@permissionId", SqlDbType.Int).Value = permissionId);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPermissionData.IsExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPermissionData.IsExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsPermissionEntity _MapToPermission(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "PermissionID", "PermissionCode", "PermissionName");

            return new clsPermissionEntity
            {
                PermissionID = reader.GetInt32(cols["PermissionID"]),
                PermissionCode = reader.GetString(cols["PermissionCode"]),
                PermissionName = reader.GetString(cols["PermissionName"]),
            };
        }
    }
}
