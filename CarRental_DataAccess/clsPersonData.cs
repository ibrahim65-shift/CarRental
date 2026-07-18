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
    public class clsPersonData
    {
        public static async Task<clsPersonEntities> GetPersonByPersonIDAsync(int personID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_People_GetByPersonID", 
                    reader=>_MapToPerson(reader),
                    p=>p.Add("@PersonID" , SqlDbType.Int).Value=personID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.GetPersonByPersonIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.GetPersonByPersonIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsPersonEntities> GetPersonByNationalNoAsync(string NationalNo)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_People_GetByNationalNo", 
                    reader=>_MapToPerson(reader),
                    p=>p.Add("@NationalNo", SqlDbType.NVarChar,50).Value=NationalNo);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.GetPersonByNationalNoAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.GetPersonByNationalNoAsync (General)", ex);
                return null;
            }
        }

        public static async Task<int?> AddNewAsync(clsPersonEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_AddNew",
                    p =>
                    {
                        p.Add("@NationalNo", SqlDbType.NVarChar,59).Value = entity.NationalNo;
                        p.Add("@FirstName", SqlDbType.NVarChar,100).Value = entity.FirstName;
                        p.Add("@SecondName", SqlDbType.NVarChar,100).Value = entity.SecondName;
                        p.Add("@ThirdName", SqlDbType.NVarChar, 100).Value = (object)entity.ThirdName ?? DBNull.Value;
                        p.Add("@LastName", SqlDbType.NVarChar,100).Value = entity.LastName;
                        p.Add("@BirthDate", SqlDbType.Date).Value = entity.BirthDate;
                        p.Add("@Gender", SqlDbType.Bit).Value = entity.Gender;
                        p.Add("@Email", SqlDbType.NVarChar, 200).Value   = string.IsNullOrWhiteSpace(entity.Email) ? (object)DBNull.Value : entity.Email;
                        p.Add("@Phone", SqlDbType.NVarChar,20).Value     = string.IsNullOrWhiteSpace(entity.Phone) ? (object)DBNull.Value : entity.Phone;
                        p.Add("@Address", SqlDbType.NVarChar, 250).Value = string.IsNullOrWhiteSpace(entity.Address) ? (object)DBNull.Value : entity.Address;
                        p.Add("@CreatedDate", SqlDbType.DateTime2).Value = entity.CreatedDate;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;

                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsPersonEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_People_Update",
                    p =>
                    {
                        p.Add("@PersonID", SqlDbType.Int).Value = entity.PersonID;
                        p.Add("@NationalNo", SqlDbType.NVarChar, 50).Value = entity.NationalNo;
                        p.Add("@FirstName", SqlDbType.NVarChar, 100).Value = entity.FirstName;
                        p.Add("@SecondName", SqlDbType.NVarChar, 100).Value = entity.SecondName;
                        p.Add("@ThirdName", SqlDbType.NVarChar, 100).Value = (object)entity.ThirdName ?? DBNull.Value;
                        p.Add("@LastName", SqlDbType.NVarChar, 100).Value = entity.LastName;
                        p.Add("@BirthDate", SqlDbType.Date).Value = entity.BirthDate;
                        p.Add("@Gender", SqlDbType.Bit).Value = entity.Gender;
                        p.Add("@Email", SqlDbType.NVarChar, 200).Value   = string.IsNullOrWhiteSpace(entity.Email) ? (object)DBNull.Value : entity.Email;
                        p.Add("@Phone", SqlDbType.NVarChar, 20).Value    = string.IsNullOrWhiteSpace(entity.Phone) ? (object)DBNull.Value : entity.Phone;
                        p.Add("@Address", SqlDbType.NVarChar, 250).Value = string.IsNullOrWhiteSpace(entity.Address) ? (object)DBNull.Value : entity.Address;
                        p.Add("@EditedDate", SqlDbType.DateTime2).Value = (object)entity.EditedDate ?? DBNull.Value;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = (object)entity.EditedByUserID ?? DBNull.Value;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int personID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };


                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_People_Delete", 
                    p=>
                    {
                        p.Add("@PersonID", SqlDbType.Int).Value = personID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(DataTable peopleData , int TotalPages)> GetPeoplePageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_People_GetPage",
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
                clsEventLogger.LogException("clsPersonData.GetPeoplePageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.GetPeoplePageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<bool> IsDuplicateEmailAsync(int? personID ,string email)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_IsDuplicatedEmail",
                    p =>
                    {
                        p.Add("@PersonID", SqlDbType.Int).Value = personID ?? (object)DBNull.Value;
                        p.Add("@Email", SqlDbType.NVarChar, 200).Value = email;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.IsDuplicateEmail (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.IsDuplicateEmail (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsDuplicatePhoneAsync(int? personID, string phone)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_IsDuplicatedPhone",
                    p =>
                    {
                        p.Add("@PersonID" , SqlDbType.Int).Value = personID ?? (object)DBNull.Value;
                        p.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.IsDuplicateEmail (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.IsDuplicateEmail (General)", ex);
                return false;
            }
        }
        public static async Task<bool> PersonExists(int PersonID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_PersonExists",
                    p => p.Add("@PersonID", SqlDbType.Int).Value = PersonID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.PersonExists (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.PersonExists (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsNationalNoExistsAsync(string nationalNo)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_People_IsNationalNoExists",
                    p => p.Add("@NationalNo", SqlDbType.NVarChar,50).Value = nationalNo);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPersonData.IsNationalNoExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPersonData.IsNationalNoExistsAsync (General)", ex);
                return false;
            }
        }

        private static clsPersonEntities _MapToPerson(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "PersonID",
                "NationalNo",
                "FirstName",
                "SecondName",
                "ThirdName",
                "LastName",
                "BirthDate",
                "Gender",
                "Email",
                "Phone",
                "Address",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID");

            return new clsPersonEntities
            {
                PersonID = reader.GetInt32(cols["PersonID"]),
                NationalNo = reader.GetString(cols["NationalNo"]),
                FirstName = reader.GetString(cols["FirstName"]),
                SecondName = reader.GetString(cols["SecondName"]),
                ThirdName = reader.GetStringOrNull(cols["ThirdName"]),
                LastName = reader.GetString(cols["LastName"]),
                BirthDate = reader.GetDateTime(cols["BirthDate"]),
                Gender = reader.GetBoolean(cols["Gender"])?enGenderType.Male:enGenderType.Female,
                Email = reader.GetStringOrNull(cols["Email"]),
                Phone = reader.GetStringOrNull(cols["Phone"]),
                Address = reader.GetStringOrNull(cols["Address"]),
                IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };
        }
    }
}
