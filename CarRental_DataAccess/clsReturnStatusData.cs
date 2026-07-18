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
    public class clsReturnStatusData
    {
        public static async Task<clsReturnStatusEntities> GetReturnStatusByIDAsync(int ReturnStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_ReturnStatus_GetByID",
                    reader => _MapToReturnStatus(reader),
                    p => p.Add("@ReturnStatusID", SqlDbType.Int).Value = ReturnStatusID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.GetReturnStatusByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.GetReturnStatusByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsReturnStatusEntities> GetReturnStatusByNameAsync(string StatusName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_ReturnStatus_GetByName",
                    reader => _MapToReturnStatus(reader),
                    p => p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = StatusName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.GetReturnStatusByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.GetReturnStatusByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsReturnStatusEntities> ReturnStatusData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsReturnStatusEntities>("SP_ReturnStatus_GetPage",
                    reader =>
                    {
                        var item = _MapToReturnStatus(reader);

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
                clsEventLogger.LogException("clsReturnStatusData.GetPageAsync (SQL)", ex);
                return (new List<clsReturnStatusEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.GetPageAsync (General)", ex);
                return (new List<clsReturnStatusEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsReturnStatusEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_ReturnStatus_AddNew",
                    p =>
                    {
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsReturnStatusEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("",
                    p =>
                    {
                        p.Add("@ReturnStatusID", SqlDbType.Int).Value = entity.ReturnStatusID;
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = entity.StatusName;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int ReturnStatusID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_ReturnStatus_Delete",
                    p =>
                    {
                        p.Add("@ReturnStatusID", SqlDbType.Int).Value = ReturnStatusID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsReturnStatusExistsAsync(int ReturnStatusID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_ReturnStatus_Exists",
                    p => p.Add("@ReturnStatusID", SqlDbType.Int).Value = ReturnStatusID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.IsReturnStatusExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.IsReturnStatusExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsReturnStatusExistsByNameAsync(int? ReturnStatusID, string ReturnStatus)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_ReturnStatus_ExistsName",
                    p =>
                    {
                        p.Add("@ReturnStatusID", SqlDbType.Int).Value = ReturnStatusID ?? (object)DBNull.Value;
                        p.Add("@StatusName", SqlDbType.NVarChar, 100).Value = ReturnStatus;
                    }
                );

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.IsReturnStatusExistsByNameAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsReturnStatusData.IsReturnStatusExistsByNameAsync (General)", ex);
                return false;
            }
        }
        private static clsReturnStatusEntities _MapToReturnStatus(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "ReturnStatusID",
                "StatusName"
                );

            return new clsReturnStatusEntities
            {
                ReturnStatusID = reader.GetInt32(cols["ReturnStatusID"]),
                StatusName = reader.GetString(cols["StatusName"])
            };

        }
    }
}
