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
    public static class clsCustomersData
    {
        public static async Task<clsCustomersEntities> GetCustomerByCustomerIDAsync(int customerID)
        {
            try
            {
                var result =  await clsSQLHelper.ExecuteReaderAsync<clsCustomersEntities>("SP_GetCustomerByCustomerID",
                    reader => _MapToCustomer(reader),
                    p =>
                    {
                        p.Add("@customerID", SqlDbType.Int).Value = customerID;
                    });

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetCustomerByCustomerIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetCustomerByCustomerIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsCustomersEntities> GetCustomerByPersonIDAsync(int personID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync<clsCustomersEntities>("SP_GetCustomerByPersonID",
                    reader => _MapToCustomer(reader),
                    p =>
                    {
                        p.Add("@personID", SqlDbType.Int).Value = personID;
                    });

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetCustomerByPersonIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetCustomerByPersonIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsCustomersEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_AddNewCustomer",
                    p =>
                    {
                        p.Add("@PersonID", SqlDbType.Int).Value = entity.PersonID;
                        p.Add("@DriverLicenseNumber", SqlDbType.NVarChar,50).Value = entity.DriverLicenseNumber;
                        p.Add("@DriverLicenseExpiry", SqlDbType.Date).Value = entity.DriverLicenseExpiry;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsCustomersEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_UpdateCustomer",
                    p =>
                    {
                        p.Add("@CustomerID", SqlDbType.Int).Value = entity.CustomerID;
                        p.Add("@PersonID", SqlDbType.Int).Value = entity.PersonID;
                        p.Add("@DriverLicenseExpiry", SqlDbType.Date).Value = entity.DriverLicenseExpiry;
                        p.Add("@IsDeleted", SqlDbType.Bit).Value = entity.IsDeleted;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = (object)entity.EditedByUserID ?? DBNull.Value;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync (int CustomerID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_DeleteCustomer",
                    p=>
                    {
                        p.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.DeleteAsync (General)", ex);
                return false;
            }

        }
        public static async Task<(DataTable dt , int TotalPages)> GetCustomersPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_Customers_GetPage",
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
                clsEventLogger.LogException("clsCustomersData.GetCustomersPageAsync (SQL)", ex);
                return (new DataTable(),0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetCustomersPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<bool> ExistsByPersonIDAsync(int PersonID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Customers_ExistsByPersonID",
                    p => p.Add("@PersonID", SqlDbType.Int).Value = PersonID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.ExistsByPersonIDAsync (SQL)", ex);
                return false;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.ExistsByPersonIDAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsLicenseExpiryAsync(DateTime LicenseExpiry)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_IsLicenseExpiry",
                    p => p.Add("@LicenseExpiry", SqlDbType.Date).Value = LicenseExpiry);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsLicenseExpiryAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsLicenseExpiryAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsLicenseNumberDuplicatedAsync(string LicenseNumber)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_IsLicenseNumberDuplicated",
                    p=>p.Add("@LicenseNumber", SqlDbType.NVarChar,50).Value= LicenseNumber);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsLicenseNumberDuplicatedAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsLicenseNumberDuplicatedAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPersonIdDuplicatedAsync(int PersonID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_IsPersonIdDuplicated",
                    p => p.Add("@PersonID", SqlDbType.NVarChar, 50).Value = PersonID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsPersonIdDuplicatedAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsPersonIdDuplicatedAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsCustomerExistsAsync(int customerID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Customers_Exists",
                    p => p.Add("@customerID", SqlDbType.Int).Value = customerID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsCustomerExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.IsCustomerExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsCustomersEntities _MapToCustomer(SqlDataReader reader)
        {
            var cols1 = clsSQLHelper.GetOrdinal(reader,
                "CustomerID",
                "PersonID",
                "DriverLicenseNumber",
                "DriverLicenseExpiry",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID");

            var cols2 = clsSQLHelper.GetOrdinal(reader,
                "NationalNo",
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

            return new clsCustomersEntities
            { 
               CustomerID = reader.GetInt32(cols1["CustomerID"]),
               PersonID = reader.GetInt32(cols1["PersonID"]),
               DriverLicenseNumber = reader.GetString(cols1["DriverLicenseNumber"]),
               DriverLicenseExpiry = reader.GetDateTime(cols1["DriverLicenseExpiry"]),
               IsDeleted = reader.GetBoolean(cols1["IsDeleted"]),
               CreatedDate = reader.GetDateTime(cols1["CreatedDate"]),
               CreatedByUserID = reader.GetInt32(cols1["CreatedByUserID"]),
               EditedDate = reader.GetDateTimeOrNull(cols1["EditedDate"]),
               EditedByUserID = reader.GetIntOrNull(cols1["EditedByUserID"]),

               Person = new clsPersonEntities
               {
                   NationalNo = reader.GetString(cols2["NationalNo"]),
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


        // ======================= Reports =======================

        public static async Task<DataTable> GetReportAllCustomersAsync(DateTime fromDate , DateTime toDate)
        {
            try
            {
               return await clsSQLHelper.ExecuteDataTableAsync("SP_Report_AllCustomers",
                   p=>
                   {
                       p.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                       p.Add("@ToDate", SqlDbType.Date).Value = toDate;
                   });
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportAllCustomers (SQL)", ex);
                return new DataTable();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportAllCustomers (General)", ex);
                return new DataTable();
            }
        }
        public static async Task<DataTable> GetReportTopCustomersAsync(int top)
        {
            try
            {
               return await clsSQLHelper.ExecuteDataTableAsync("SP_Report_TopCustomers",
                   p=>p.Add("@Top", SqlDbType.Int).Value = top);
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportTopCustomersAsync (SQL)", ex);
                return new DataTable();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportTopCustomersAsync (General)", ex);
                return new DataTable();
            }
        }
        public static async Task<DataTable> GetReportCustomerActivityAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteDataTableAsync("SP_Report_CustomerActivity");
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportCustomerActivityAsync (SQL)", ex);
                return new DataTable();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsCustomersData.GetReportCustomerActivityAsync (General)", ex);
                return new DataTable();
            }
        }

    }
}
