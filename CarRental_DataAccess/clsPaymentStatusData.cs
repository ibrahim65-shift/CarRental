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
    public class clsPaymentStatusData
    {
        public static async Task<clsPaymentStatusEntities> GetPaymentStatusByIDAsync(int PaymentStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_PaymentStatus_GetByID",
                    reader => _MapToPaymentStatus(reader),
                    p => p.Add("@PaymentStatusID", SqlDbType.Int).Value = PaymentStatusID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.GetPaymentStatusByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.GetPaymentStatusByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsPaymentStatusEntities> GetPaymentStatusByNameAsync(string StatusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_PaymentStatus_GetByName",
                    reader => _MapToPaymentStatus(reader),
                    p => p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = StatusName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.GetPaymentStatusByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.GetPaymentStatusByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsPaymentStatusEntities> PaymentStatusData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsPaymentStatusEntities>("SP_PaymentStatus_GetPage",
                    reader =>
                    {
                        var item = _MapToPaymentStatus(reader);

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
                clsEventLogger.LogException("clsPaymentStatusData.GetPageAsync (SQL)", ex);
                return (new List<clsPaymentStatusEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.GetPageAsync (General)", ex);
                return (new List<clsPaymentStatusEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsPaymentStatusEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentStatus_AddNew",
                    p =>
                    {
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsPaymentStatusEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_PaymentStatus_Update",
                    p =>
                    {
                        p.Add("@PaymentStatusID", SqlDbType.Int).Value = entity.PaymentStatusID;
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int PaymentStatusID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_PaymentStatus_Delete",
                    p =>
                    {
                        p.Add("@PaymentStatusID", SqlDbType.Int).Value = PaymentStatusID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPaymentStatusExistsByIDAsync(int PaymentStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_PaymentStatus_ExistsPaymentStatusID",
                    p => p.Add("@PaymentStatusID", SqlDbType.Int).Value = PaymentStatusID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.IsPaymentStatusExistsByIDAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.IsPaymentStatusExistsByIDAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPaymentStatusExistsByNameAsync(int? PaymentStatusID, string PaymentStatus)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_PaymentStatus_ExistsStatusName",
                    p =>
                    {
                        p.Add("@PaymentStatusID", SqlDbType.Int).Value = PaymentStatus ?? (object)DBNull.Value;
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = PaymentStatus;
                    }
                );

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.IsPaymentStatusExistsByNameAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsPaymentStatusData.IsPaymentStatusExistsByNameAsync (General)", ex);
                return false;
            }
        }
        private static clsPaymentStatusEntities _MapToPaymentStatus(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "PaymentStatusID",
                "StatusName"
                );

            return new clsPaymentStatusEntities
            {
                PaymentStatusID = reader.GetInt32(cols["PaymentStatusID"]),
                StatusName = reader.GetString(cols["StatusName"])
            };

        }
    }
}
