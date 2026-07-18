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
    public class clsPaymentTransactionsData
    {
        public static async Task<clsPaymentTransactionsEntities> GetPaymentTransactionsByIDAsync(int PaymentID)
        {
            try
            {
                var result  = await clsSQLHelper.ExecuteReaderAsync("SP_PaymentTransactions_GetByPaymentID",
                   reader=> _MapToPaymentTransactions(reader),
                   p=>
                   {
                       p.Add("@PaymentID", SqlDbType.Int).Value = PaymentID;
                   });

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPaymentTransactionsByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPaymentTransactionsByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsPaymentTransactionsEntities>> GetPaymentTransactionsByInvoiceIDAsync(int InvoiceID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_PaymentTransactions_GetByInvoiceID",
                   reader => _MapToPaymentTransactions(reader),
                   p =>
                   {
                       p.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;
                   });

            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPaymentTransactionsByInvoiceIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPaymentTransactionsByInvoiceIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable paymentData, int TotalPage)> GetPageAsync
                 (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_PaymentTransactions_GetPage",
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
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsPaymentTransactionsEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentTransactions_AddNew",
                    p =>
                    {
                        p.Add("@InvoiceID", SqlDbType.Int).Value = entity.InvoiceID;
                        p.Add("@PaymentMethodID", SqlDbType.Int).Value = entity.PaymentMethodID;
                        p.Add("@PaidAmount", SqlDbType.Decimal).Value = entity.PaidAmount;
                        p.Add("@Reference", SqlDbType.NVarChar,200).Value = string.IsNullOrWhiteSpace(entity.Reference)?(object)DBNull.Value: entity.Reference;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                      
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddRefundAsync(int paymentId,enPaymentMethod paymentMethodId  , decimal refundAmount , string reference , int userId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentTransactions_AddRefund",
                    p =>
                    {
                        p.Add("@PaymentID", SqlDbType.Int).Value = paymentId;
                        p.Add("@PaymentMethodID", SqlDbType.Int).Value = paymentMethodId;
                        p.Add("@RefundAmount", SqlDbType.Decimal).Value = refundAmount;
                        p.Add("@Reference", SqlDbType.NVarChar,200).Value = string.IsNullOrWhiteSpace(reference) ?(object)DBNull.Value: reference;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = userId;
                      
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.AddRefundAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.AddRefundAsync (General)", ex);
                return null;
            }
        } 
        public static async Task<decimal?> GetTotalRefundedAmountAsync(int paymentId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentTransactions_GetRemainingRefundAmount",
                    p =>
                    {
                        p.Add("@PaymentID", SqlDbType.Int).Value = paymentId;
                    });

                return clsSQLHelper.ToDecimalSafe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetTotalRefundedAmountAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.GetTotalRefundedAmountAsync (General)", ex);
                return null;
            }
        } 

        public static async Task<bool> UpdateStatusAsync(clsPaymentTransactionsEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_PaymentTransactions_UpdateStatus",
                    p =>
                    {
                        p.Add("@PaymentID", SqlDbType.Int).Value = entity.PaymentID;
                        p.Add("@PaymentStatusID", SqlDbType.Int).Value = entity.PaymentStatusID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.UpdateStatusAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.UpdateStatusAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPaymentTransactionExistsAsync(int PaymentID)
        {
            try
            {

                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentTransactions_Exists",
                    p =>
                    {
                        p.Add("@PaymentID", SqlDbType.Int).Value = PaymentID;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.IsPaymentTransactionExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.IsPaymentTransactionExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsInvoicePaidAsync(int InvoiceID)
        {
            try
            {
                SqlParameter IsPaid = new SqlParameter("@IsPaid", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };


                await clsSQLHelper.ExecuteScalarAsync("SP_PaymentTransactions_IsInvoicePaid",
                    p =>
                    {
                        p.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;

                        p.Add(IsPaid);
                    });

                return IsPaid.Value != DBNull.Value && (bool)IsPaid.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.IsInvoicePaidAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentTransactionsData.IsInvoicePaidAsync (General)", ex);
                return false;
            }
        }
        private static clsPaymentTransactionsEntities _MapToPaymentTransactions(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "PaymentID",
                "InvoiceID",
                "PaymentMethodID",
                "PaymentStatusID",
                "PaymentDate",
                "PaidAmount",
                "Reference",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
                );

            return new clsPaymentTransactionsEntities
            {
                PaymentID = reader.GetInt32(cols["PaymentID"]),
                InvoiceID = reader.GetInt32(cols["InvoiceID"]),
                PaymentMethodID = (enPaymentMethod)reader.GetInt32(cols["PaymentMethodID"]),
                PaymentStatusID =(enPaymentStatus) reader.GetInt32(cols["PaymentStatusID"]),
                PaymentDate = reader.GetDateTime(cols["PaymentDate"]),
                PaidAmount = reader.GetDecimal(cols["PaidAmount"]),
                Reference = reader.GetStringOrNull(cols["Reference"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };
        }
    }
}
