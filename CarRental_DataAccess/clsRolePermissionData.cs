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
        public static async Task<clsRolePermissionEntity> GetRolePermissionsByIdAsync(int rolePermissionId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_RolePermissions_GetByID",
                    reader => _MapToRolePermission(reader),
                    p => p.Add("@rolePermissionID", SqlDbType.Int).Value = rolePermissionId);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetRolePermissionsByIdAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetRolePermissionsByIdAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsRolePermissionEntityView>> GetPermissionsForRoleAsync(int roleId)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_RolePermissions_GetPermissionsForRole", 
                    reader => _MapToRolePermissionView(reader),
                    p => p.Add("@roleId", SqlDbType.Int).Value = roleId);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetPermissionsForRoleAsync (SQL)", ex);
                return new List<clsRolePermissionEntityView>();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetPermissionsForRoleAsync (General)", ex);
                return new List<clsRolePermissionEntityView>();
            }
        }
        public static async Task<(DataTable dt, int TotalPages)> GetRolePermissionsPageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_RolePermissions_GetPage",
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
                clsEventLogger.LogException("clsRolePermissionData.GetRolePermissionsPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRolePermissionData.GetRolePermissionsPageAsync (General)", ex);
                return (new DataTable(), 0);
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
                "RoleID",
                "PermissionID",
                "IsAllowed"
                );

            return new clsRolePermissionEntity
            { 
               RolePermissionID = reader.GetInt32(cols["RolePermissionID"]),
               RoleID = reader.GetInt32(cols["RoleID"]),
               PermissionID = reader.GetInt32(cols["PermissionID"]),
               IsAllowed = reader.GetBoolean(cols["IsAllowed"]),
            };

        }
        private static clsRolePermissionEntityView _MapToRolePermissionView(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "RolePermissionID",
                "PermissionCode",
                "PermissionName",
                "IsAllowed",
                "PermissionID"
                );

            return new clsRolePermissionEntityView
            {
                RolePermissionID = reader.GetInt32(cols["RolePermissionID"]),
                PermissionCode = reader.GetString(cols["PermissionCode"]),
                PermissionName = reader.GetString(cols["PermissionName"]),
                IsAllowed = reader.GetBoolean(cols["IsAllowed"]),
                PermissionID = reader.GetInt32(cols["PermissionID"]),
            };

        }
    }
}
