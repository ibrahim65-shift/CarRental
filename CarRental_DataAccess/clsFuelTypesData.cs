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
    public class clsFuelTypesData
    {
        public static async Task<clsFuelTypesEntities> GetFuelTypeByIDAsync(int fuelTypeID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_FuelTypes_GetByID",
                    reader => _MapToFuelType(reader),
                    p => p.Add("@FuelTypeID", SqlDbType.Int).Value= fuelTypeID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypeByIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypeByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsFuelTypesEntities> GetFuelTypeByFuelTypeNameAsync(string fuelTypeName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_FuelTypes_GetByName",
                    reader => _MapToFuelType(reader),
                    p => p.Add("@FuelTypeName", SqlDbType.NVarChar,100).Value = fuelTypeName);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypeByFuelTypeNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypeByFuelTypeNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsFuelTypesEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_FuelTypes_AddNew",
                    p =>
                    {
                        p.Add("@FuelTypeName", SqlDbType.NVarChar, 100).Value = entity.FuelTypeName;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsFuelTypesEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_FuelTypes_Update",
                    p =>
                    {
                        p.Add("@FuelTypeID", SqlDbType.Int).Value = entity.FuelTypeID;
                        p.Add("@FuelTypeName", SqlDbType.NVarChar, 100).Value = entity.FuelTypeName;
                    });

                return clsSQLHelper.ToInt32Safe(result) > 0;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int fuelTypeID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_FuelTypes_Delete",
                    p => p.Add("@FuelTypeID", SqlDbType.Int).Value = fuelTypeID);

                return clsSQLHelper.ToInt32Safe(result) > 0;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(List<clsFuelTypesEntities> fuelTypesData , int TotalPages)>GetFuelTypePageAsync
            (int PageNumber , int PageSize , string FilterColumn=null,string FilterValue=null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsFuelTypesEntities>("SP_FuelTypes_GetPage",
                    reader =>
                    {
                        var item = _MapToFuelType(reader);

                        if(totalCount==0)
                        totalCount = Convert.ToInt32(reader["TotalCount"]);

                        return item;
                    },
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue; ;
                    });

                int totalPages = 0;

                if(list.Count>0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }

                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypePageAsync (SQL)", ex);
                return (new List<clsFuelTypesEntities>(),0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.GetFuelTypePageAsync (General)", ex);
                return (new List<clsFuelTypesEntities>(), 0);
            }
        }
        public static async Task<bool> IsFuelTypeNameExistsAsync(string fuelTypeName)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_FuelTypes_ExistsFuelTypeName",
                    p=>p.Add("@FuelTypeName" , SqlDbType.NVarChar,100).Value= fuelTypeName);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.IsFuleTypeNameExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.IsFuleTypeNameExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsFuelTypeIDExistsAsync(int fuelTypeID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_FuelTypes_ExistsFuelTypeID",
                    p => p.Add("@FuelTypeID", SqlDbType.Int).Value = fuelTypeID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.IsFuleTypeIDExists (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsFuelTypesData.IsFuleTypeIDExists (General)", ex);
                return false;
            }
        }
        private static clsFuelTypesEntities _MapToFuelType(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader, "FuelTypeID", "FuelTypeName");

            return new clsFuelTypesEntities
            {
                FuelTypeID = reader.GetInt32(cols["FuelTypeID"]),
                FuelTypeName = reader.GetString(cols["FuelTypeName"])
            };
        }
    }
}
