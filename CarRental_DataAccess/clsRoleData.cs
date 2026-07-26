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
    public static class clsRoleData
    {
        public static async Task<clsRoleEntity> GetByIDAsync(int roleId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Roles_GetByID",
                    reader => _MapToRole(reader),
                    p => p.Add("@RoleID", SqlDbType.Int).Value = roleId);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.GetByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.GetByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsRoleEntity role)
        {
            try
            {
                SqlParameter newRoleId = new SqlParameter("@NewRoleID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_Roles_AddNew",
                    p =>
                    {
                        p.Add("@RoleName", SqlDbType.NVarChar, 150).Value = role.RoleName;
                        p.Add("@Description", SqlDbType.NVarChar, 200).Value =string.IsNullOrWhiteSpace(role.Description)? DBNull.Value : (object)role.Description;
                        p.Add("@IsActive", SqlDbType.Bit).Value = role.IsActive;

                        p.Add(newRoleId);
                    });

                return newRoleId.Value == DBNull.Value ? null : (int?)newRoleId.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(int roleId,clsRoleEntity role)
        {
            try
            {
                SqlParameter isSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_Roles_Update",
                    p =>
                    {
                        p.Add("@RoleID", SqlDbType.Int).Value = roleId;
                        p.Add("@RoleName", SqlDbType.NVarChar, 150).Value = role.RoleName;
                        p.Add("@Description", SqlDbType.NVarChar, 200).Value =string.IsNullOrWhiteSpace(role.Description) ? DBNull.Value : (object)role.Description;
                        p.Add("@IsActive", SqlDbType.Bit).Value = role.IsActive;

                        p.Add(isSuccess);
                    });

                return isSuccess.Value != DBNull.Value &&
                       (bool)isSuccess.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int roleId)
        {
            try
            {
                SqlParameter isSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_Roles_Delete",
                    p =>
                    {
                        p.Add("@RoleID", SqlDbType.Int).Value = roleId;
                        p.Add(isSuccess);
                    });

                return isSuccess.Value != DBNull.Value &&
                       (bool)isSuccess.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<List<clsRoleEntity>> GetAllAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Roles_GetAll",reader => _MapToRole(reader));
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.GetAllAsync (SQL)", ex);
                return new List<clsRoleEntity>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.GetAllAsync (General)", ex);
                return new List<clsRoleEntity>();
            }
        }
        public static async Task<List<clsRoleEntity>> GetActiveRolesAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Roles_GetActiveRoles",reader => _MapToRole(reader));
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.GetActiveRolesAsync (SQL)", ex);
                return new List<clsRoleEntity>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.GetActiveRolesAsync (General)", ex);
                return new List<clsRoleEntity>();
            }
        }
        public static async Task<bool> IsExistsAsync(int roleId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Roles_Exists", p => p.Add("@RoleID", SqlDbType.Int).Value = roleId);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.IsExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.IsExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsRoleNameExistsAsync(string roleName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Roles_RoleNameExists",
                    p => p.Add("@RoleName", SqlDbType.NVarChar, 150).Value = roleName);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.IsRoleNameExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.IsRoleNameExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsRoleNameExistsExceptAsync(int roleId, string roleName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Roles_RoleNameExistsExcept",
                    p =>
                    {
                        p.Add("@RoleID", SqlDbType.Int).Value = roleId;
                        p.Add("@RoleName", SqlDbType.NVarChar, 150).Value = roleName;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRoleData.IsRoleNameExistsExceptAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRoleData.IsRoleNameExistsExceptAsync (General)", ex);
                return false;
            }
        }
        private static clsRoleEntity _MapToRole(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(
                reader,
                "RoleID",
                "RoleName",
                "Description",
                "IsActive");

            return new clsRoleEntity
            {
                RoleID = reader.GetInt32(cols["RoleID"]),
                RoleName = reader.GetString(cols["RoleName"]),
                Description = reader.IsDBNull(cols["Description"])? null: reader.GetString(cols["Description"]),
                IsActive = reader.GetBoolean(cols["IsActive"])
            };
        }
    }
}