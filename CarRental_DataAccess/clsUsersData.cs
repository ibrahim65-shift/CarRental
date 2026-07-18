using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SharedClass;
using System.Runtime.InteropServices;

namespace CarRental_DataAccess
{
    public class clsUsersData
    {
        public static async Task<clsUsersEntities> GetUserByUserIDAsync(int userID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Users_GetByUserID",
                    reader=>_MapToUser(reader),
                    p=>p.Add("@UserID", SqlDbType.Int).Value=userID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUserByUserIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUserByUserIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsUsersEntities> GetUserByPersonIDAsync(int personID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Users_GetByPersonID",
                    reader => _MapToUser(reader),
                    p => p.Add("@PersonID", SqlDbType.Int).Value = personID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUserByPersonIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUserByPersonIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsUsersEntities> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Users_GetByUserName", 
                    reader=> _MapToUser(reader),
                    p=>p.Add("@UserName", SqlDbType.NVarChar,150).Value= userName);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUserByUserNameAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {

                clsEventLogger.LogException("clsUsersData.GetUserByUserNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsUsersEntities>> GetUsersByRoleIDAsync(int roleID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync<clsUsersEntities>("SP_Users_GetByRoleID",
                    reader => _MapToUser(reader),
                    p=>p.Add("@RoleID" , SqlDbType.Int).Value = roleID);
            }
            catch( SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUsersByRoleIDAsync (SQL)", ex);
                return new List<clsUsersEntities>();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUsersByRoleIDAsync (General)", ex);
                return new List<clsUsersEntities>();
            }
        }
        public static async Task<int?> AddNewAsync(clsUsersEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Users_AddNew",
                    p =>
                    {
                        p.Add("@PersonID", SqlDbType.Int).Value = entity.PersonID;
                        p.Add("@RoleID", SqlDbType.Int).Value = entity.RoleID;
                        p.Add("@UserName", SqlDbType.NVarChar,150).Value = entity.UserName;
                        p.Add("@Password", SqlDbType.NVarChar,500).Value = entity.Password;
                        p.Add("@IsActive", SqlDbType.Bit).Value = entity.IsActive;
                        p.Add("@IsDeleted", SqlDbType.Bit).Value = entity.IsDeleted;
                        p.Add("@IsLockedOut", SqlDbType.Bit).Value = entity.IsLockedOut;
                        p.Add("@FailedLoginAttempts", SqlDbType.Int).Value = entity.FailedLoginAttempts;
                        p.Add("@CreatedDate", SqlDbType.DateTime2).Value = entity.CreatedDate;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsUsersEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Users_Update",
                    p =>
                    {
                        p.Add("@UserID", SqlDbType.Int).Value = entity.UserID;
                        p.Add("@RoleID", SqlDbType.Int).Value = entity.RoleID;
                        p.Add("@UserName", SqlDbType.NVarChar,150).Value = entity.UserName;
                        p.Add("@IsActive", SqlDbType.Bit).Value = entity.IsActive;
                        p.Add("@IsDeleted", SqlDbType.Bit).Value = entity.IsDeleted;
                        p.Add("@IsLockedOut", SqlDbType.Bit).Value = entity.IsLockedOut;
                        p.Add("@FaildLoginAttempts", SqlDbType.Int).Value = entity.FailedLoginAttempts;
                        p.Add("@LastFaildLoginDate", SqlDbType.DateTime).Value = (object)entity.LastFailedLoginDate ?? DBNull.Value;
                        p.Add("@EditedDate", SqlDbType.DateTime2).Value = (object)entity.EditedDate ?? DBNull.Value;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = (object)entity.EditedByUserID ?? DBNull.Value;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int userID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Users_Delete",
                    p =>
                    {
                        p.Add("@UserID", SqlDbType.Int).Value = userID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(DataTable data, int TotalPages)> GetUsersPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_Users_GetPage",
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
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUsersPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.GetUsersPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<bool> IsUserNameExistsAsync(string userName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Users_IsUserNameExists",
                    p=>p.Add("@UserName",SqlDbType.NVarChar,150).Value=userName);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.IsUserNameExistsAsync (SQL)", ex);
                return false;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.IsUserNameExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPersonIDExistsAsync(int personID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Users_IsPersonIDExists",
                    p => p.Add("@PersonID", SqlDbType.Int).Value = personID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> UpdateUserPasswordAsync(int UserID, string Password)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Users_UpdateUserPassword",
                    p =>
                    {
                        p.Add("@UserID", SqlDbType.Int).Value = UserID;
                        p.Add("@Password", SqlDbType.NVarChar, 500).Value = Password;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.UpdateUserPasswordAsync (SQL)", ex);
                return false;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.UpdateUserPasswordAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPersonIDExistsInSystemAsync(int PersonID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_PersonExists",
                    p=>p.Add("@PersonID", SqlDbType.Int).Value=PersonID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsInSystemAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsInSystemAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsRoleIDExistsInSystemAsync(int roleId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Users_IsRoleIDExistsInSystem",
                    p => p.Add("@RoleID", SqlDbType.Int).Value = roleId);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsInSystemAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUsersData.IsPersonIDExistsInSystemAsync (General)", ex);
                return false;
            }
        }
        private static clsUsersEntities _MapToUser(SqlDataReader reader)
        {
            var cols1 = clsSQLHelper.GetOrdinal(reader,
                "UserID",
                "PersonID",
                "RoleID",
                "UserName",
                "Password",
                "IsActive",
                "IsDeleted",
                "IsLockedOut",
                "FailedLoginAttempts",
                "LastFailedLoginDate",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID");

            var cols2 = clsSQLHelper.GetOrdinal(reader,
              "FirstName",
              "SecondName",
              "ThirdName",
              "LastName",
              "BirthDate",
              "Gender",
              "Email",
              "Phone",
              "Address"
            );

            return new clsUsersEntities
            {
                UserID = reader.GetInt32(cols1["UserID"]),
                PersonID = reader.GetInt32(cols1["PersonID"]),
                RoleID = reader.GetInt32(cols1["RoleID"]),
                UserName = reader.GetString(cols1["UserName"]),
                Password = reader.GetString(cols1["Password"]),
                IsActive = reader.GetBoolean(cols1["IsActive"]),
                IsDeleted = reader.GetBoolean(cols1["IsDeleted"]),
                IsLockedOut = reader.GetBoolean(cols1["IsLockedOut"]),
                FailedLoginAttempts = reader.GetInt32(cols1["FailedLoginAttempts"]),
                LastFailedLoginDate = reader.GetDateTimeOrNull(cols1["LastFailedLoginDate"]),
                CreatedDate = reader.GetDateTime(cols1["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols1["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols1["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols1["EditedByUserID"]),

                Person = new clsPersonEntities
                {
                    FirstName = reader.GetString(cols2["FirstName"]),
                    SecondName = reader.GetString(cols2["SecondName"]),
                    ThirdName = reader.GetStringOrNull(cols2["ThirdName"]),
                    LastName = reader.GetString(cols2["LastName"]),
                    BirthDate = reader.GetDateTime(cols2["BirthDate"]),
                    Gender = reader.GetBoolean(cols2["Gender"]) ? enGenderType.Male : enGenderType.Female,
                    Email = reader.GetStringOrNull(cols2["Email"]),
                    Phone = reader.GetStringOrNull(cols2["Phone"]),
                    Address = reader.GetStringOrNull(cols2["Address"]),
                }
            };

        }
    }
}
