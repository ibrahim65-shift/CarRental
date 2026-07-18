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
    public class clsInsuranceTypeData
    {
        public static async Task<clsInsuranceTypeEntities> GetInsuranceTypeByIDAsync(int InsuranceTypeID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_InsuranceType_GetByID",
                    reader => _MapToInsuranceType(reader),
                    p => p.Add("@InsuranceTypeID", SqlDbType.Int).Value = InsuranceTypeID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.GetInsuranceTypeByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.GetInsuranceTypeByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsInsuranceTypeEntities> GetInsuranceTypeByTypeAsync(string insuranceType)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_InsuranceType_GetByType",
                    reader => _MapToInsuranceType(reader),
                    p => p.Add("@InsuranceType", SqlDbType.NVarChar, 100).Value = insuranceType);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.GetInsuranceTypeByTypeAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.GetInsuranceTypeByTypeAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsInsuranceTypeEntities> InsuranceTypeData, int TotalPages)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsInsuranceTypeEntities>("SP_InsuranceType_GetPage",
                    reader =>
                    {
                        var item = _MapToInsuranceType(reader);

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
                clsEventLogger.LogException("clsInsuranceTypeData.GetPageAsync (SQL)", ex);
                return (new List<clsInsuranceTypeEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.GetPageAsync (General)", ex);
                return (new List<clsInsuranceTypeEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsInsuranceTypeEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_InsuranceType_AddNew",
                    p =>
                    {
                        p.Add("@InsuranceType", SqlDbType.NVarChar, 100).Value = entity.InsuranceType;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsInsuranceTypeEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_InsuranceType_Update",
                    p =>
                    {
                        p.Add("@InsuranceTypeID", SqlDbType.Int).Value = entity.InsuranceTypeID;
                        p.Add("@InsuranceType", SqlDbType.NVarChar, 100).Value = entity.InsuranceType;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int InsuranceTypeID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_InsuranceType_Delete",
                    p =>
                    {
                        p.Add("@InsuranceTypeID", SqlDbType.Int).Value = InsuranceTypeID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsInsuranceTypeExistsAsync(int InsuranceTypeID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_InsuranceType_Exists",
                    p => p.Add("@InsuranceTypeID", SqlDbType.Int).Value = InsuranceTypeID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.IsInsuranceExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.IsInsuranceExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsInsuranceTypeExistsByTypeAsync(int? InsuranceTypeID, string InsuranceType)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_InsuranceType_ExistsType",
                    p =>
                    {
                        p.Add("@@InsuranceTypeID", SqlDbType.Int).Value = InsuranceTypeID ?? (object)DBNull.Value;
                        p.Add("@@InsuranceType", SqlDbType.NVarChar, 100).Value = InsuranceType;
                    }
                );

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.IsInsuranceTypeExistsByTypeAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceTypeData.IsInsuranceTypeExistsByTypeAsync (General)", ex);
                return false;
            }
        }
        private static clsInsuranceTypeEntities _MapToInsuranceType(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "InsuranceTypeID",
                "InsuranceType"
                );

            return new clsInsuranceTypeEntities
            {
                InsuranceTypeID = reader.GetInt32(cols["InsuranceTypeID"]),
                InsuranceType = reader.GetString(cols["InsuranceType"])
            };

        }
    }
}
