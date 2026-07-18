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
    public class clsPaymentMethodsData
    {
        public static async Task<clsPaymentMethodsEntities> GetPaymentMethodsByIDAsync(int PaymentMethodsID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_PaymentMethods_GetByID",
                    reader => _MapToPaymentMethods(reader),
                    p => p.Add("@PaymentMethodID", SqlDbType.Int).Value = PaymentMethodsID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPaymentMethodsByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPaymentMethodsByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsPaymentMethodsEntities> GetPaymentMethodsByNameAsync(string MethodeName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_PaymentMethods_GetByName",
                    reader => _MapToPaymentMethods(reader),
                    p => p.Add("@MethodName", SqlDbType.NVarChar, 100).Value = MethodeName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPaymentMethodsByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPaymentMethodsByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsPaymentMethodsEntities> PaymentMethodsData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsPaymentMethodsEntities>("SP_PaymentMethods_GetPage",
                    reader =>
                    {
                        var item = _MapToPaymentMethods(reader);

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

                if (list.Count > 0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }


                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPageAsync (SQL)", ex);
                return (new List<clsPaymentMethodsEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.GetPageAsync (General)", ex);
                return (new List<clsPaymentMethodsEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsPaymentMethodsEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentMethods_AddNew",
                    p =>
                    {
                        p.Add("@MethodName", SqlDbType.NVarChar, 100).Value = entity.MethodName;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsPaymentMethodsEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_PaymentMethods_Update",
                    p =>
                    {
                        p.Add("@PaymentMethodID", SqlDbType.Int).Value = entity.PaymentMethodID;
                        p.Add("@MethodName", SqlDbType.NVarChar, 100).Value = entity.MethodName;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int PaymentMethodID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_PaymentMethods_Delete",
                    p =>
                    {
                        p.Add("@PaymentMethodID", SqlDbType.Int).Value = PaymentMethodID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPaymentMethodsExistsByIDAsync(int PaymentMethodID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentMethods_Exists",
                    p => p.Add("@PaymentMethodID", SqlDbType.Int).Value = PaymentMethodID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.IsPaymentMethodsExistsByIDAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.IsPaymentMethodsExistsByIDAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPaymentMethodsExistsByNameAsync(int? PaymentMethodID, string PaymentMethods)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_PaymentMethods_ExistsName",
                    p =>
                    {
                        p.Add("@PaymentMethodID", SqlDbType.Int).Value = PaymentMethods ?? (object)DBNull.Value;
                        p.Add("@MethodName", SqlDbType.NVarChar, 100).Value = PaymentMethods;
                    }
                );

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.IsPaymentMethodsExistsByNameAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentMethodsData.IsPaymentMethodsExistsByNameAsync (General)", ex);
                return false;
            }
        }
        private static clsPaymentMethodsEntities _MapToPaymentMethods(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "PaymentMethodID",
                "MethodName"
                );

            return new clsPaymentMethodsEntities
            {
                PaymentMethodID = reader.GetInt32(cols["PaymentMethodID"]),
                MethodName = reader.GetString(cols["MethodName"])
            };

        }
    }
}
