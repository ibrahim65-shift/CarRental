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
    public class clsVehicleCategoryData
    {
        public static async Task<clsVehicleCategoryEntities> GetVehicleCategoryByIDAsync(int categoryID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleCategory_GetByID" , 
                    reader=>_MapToVehicleCategory(reader),
                    p=>p.Add("@CategoryID", SqlDbType.Int).Value=categoryID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryByIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehicleCategoryEntities> GetVehicleCategoryByNameAsync(string categoryName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleCategory_GetByName",
                    reader => _MapToVehicleCategory(reader),
                    p => p.Add("@CategoryName", SqlDbType.NVarChar,100).Value = categoryName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(List<clsVehicleCategoryEntities> categoryData , int TotalPages)> GetVehicleCategoryPageAsync
            (int PageNumber , int PageSize , string FilterColumn=null, string FilterValue=null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsVehicleCategoryEntities>("SP_VehicleCategory_GetPage",
                    reader =>
                    {
                        var item = _MapToVehicleCategory(reader);
                        if(totalCount == 0)
                          totalCount = Convert.ToInt32(reader["TotalCount"]);

                        return item;
                    },
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar,128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar,200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue;
                    });

                int TotalPages = 0;

                if(list.Count> 0)
                {
                    TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }

                return (list, TotalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryPageAsync (SQL)", ex);
                return (new List<clsVehicleCategoryEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.GetVehicleCategoryPageAsync (General)", ex);
                return (new List<clsVehicleCategoryEntities>(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsVehicleCategoryEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleCategory_AddNew",
                    p => p.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = entity.CategoryName);

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsVehicleCategoryEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleCategory_Update",
                    p =>
                    {
                        p.Add("@CategoryID", SqlDbType.Int).Value = entity.CategoryID;
                        p.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = entity.CategoryName;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int categoryID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleCategory_Delete",
                    p =>
                    {
                        p.Add("@CategoryID", SqlDbType.Int).Value = categoryID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsCategoryIDExistsAsync(int categoryID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleCategory_ExistsCategoryID",
                    p =>
                    {
                        p.Add("@CategoryID", SqlDbType.Int).Value = categoryID;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.IsCategoryIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.IsCategoryIDExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsCategoryNameExistsAsync(int? categoryID,string categoryName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleCategory_ExistsCategoryName",
                    p =>
                    {
                        p.Add("@CategoryID", SqlDbType.Int).Value = categoryID ?? (object)DBNull.Value;
                        p.Add("@CategoryName", SqlDbType.NVarChar,100).Value = categoryName;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.IsCategoryNameExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleCategoryData.IsCategoryNameExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsVehicleCategoryEntities _MapToVehicleCategory(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader , "CategoryID", "CategoryName");

            return new clsVehicleCategoryEntities
            {
                CategoryID = reader.GetInt32(cols["CategoryID"]),
                CategoryName = reader.GetString(cols["CategoryName"])
            };
        }
    }
}
