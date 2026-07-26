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
    public static class clsRolePermissionData
    {
        public static async Task<List<clsRolePermissionEntity>> GetPermissionsForRoleAsync(int roleId)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_RolePermissions_GetPermissionsForRole", 
                    reader => _MapToRolePermission(reader),
                    p => p.Add("@roleId", SqlDbType.Int).Value = roleId);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetPermissionsForRoleAsync (SQL)", ex);
                return new List<clsRolePermissionEntity>();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetPermissionsForRoleAsync (General)", ex);
                return new List<clsRolePermissionEntity>();
            }
        }
        public static async Task<bool> SaveRolePermissionsAsync(int roleId , List<clsRolePermissionItem> permissions)
        {
            try
            {
                DataTable permissionsTable = new DataTable();

                permissionsTable.Columns.Add("PermissionID",  typeof(int));
                permissionsTable.Columns.Add("IsAllowed",  typeof(bool));

                permissionsTable.BeginLoadData();

                foreach(var item in permissions)
                {
                    permissionsTable.Rows.Add(item.PermissionID,item.IsAllowed);
                }

                permissionsTable.EndLoadData();

                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RolePermissions_Save",
                    p =>
                    {
                        p.Add("@roleId" , SqlDbType.Int).Value = roleId;
                        p.Add("@permissions", SqlDbType.Structured).Value = permissionsTable;
                        p["@permissions"].TypeName = "dbo.RolePermissionListType";

                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.SaveRolePermissionsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.SaveRolePermissionsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> HasPermissionAsync(int roleId , int permissionId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RolePermissions_RoleHasPermission",
                    p =>
                    {
                        p.Add("@roleId", SqlDbType.Int).Value = roleId;
                        p.Add("@permissionId", SqlDbType.Int).Value = permissionId;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.HasPermissionAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.HasPermissionAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> HasPermissionAsync(int roleId , string permissionCode)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RolePermissions_HasPermission",
                   p =>
                   {
                       p.Add("@roleId", SqlDbType.Int).Value = roleId;
                       p.Add("@permissionCode", SqlDbType.NVarChar , 150).Value = permissionCode;
                   });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.HasPermissionAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.HasPermissionAsync (General)", ex);
                return false;
            }
        }
        private static clsRolePermissionEntity _MapToRolePermission(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "RolePermissionID",
                "PermissionCode",
                "PermissionName",
                "IsAllowed"
                );

            return new clsRolePermissionEntity
            { 
               RolePermissionID = reader.GetInt32(cols["RolePermissionID"]),
               PermissionCode = reader.GetString(cols["PermissionCode"]),
               PermissionName = reader.GetString(cols["PermissionName"]),
               IsAllowed = reader.GetBoolean(cols["IsAllowed"]),
            };

        }
    }
}
