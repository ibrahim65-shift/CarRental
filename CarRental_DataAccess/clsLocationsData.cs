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
    public class clsLocationsData
    {
        public static async Task<clsLocationsEntities> GetLocationByIDAsync(int locationID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Locations_GetByID",
                    reader=>_MapToLocations(reader),
                    p => p.Add("@LocationID", SqlDbType.Int).Value = locationID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationByIDAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsLocationsEntities>> GetLocationsByNameAsync(string Name)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Locations_GetByName",
                    reader => _MapToLocations(reader),
                    p => p.Add("@Name", SqlDbType.NVarChar,200).Value = Name);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationByNameAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationByNameAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsLocationsEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Locations_AddNew",
                    p =>
                    {
                        p.Add("@Name", SqlDbType.NVarChar,200).Value = entity.Name;
                        p.Add("@Address", SqlDbType.NVarChar, 400).Value = string.IsNullOrWhiteSpace(entity.Address) ? (object)DBNull.Value : entity.Address;
                        p.Add("@Phone", SqlDbType.NVarChar,50).Value = string.IsNullOrWhiteSpace(entity.Phone) ? (object)DBNull.Value : entity.Phone;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsLocationsEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Locations_Update",
                    p =>
                    {

                        p.Add("@LocationID", SqlDbType.Int).Value = entity.LocationID;
                        p.Add("@Name", SqlDbType.NVarChar,200).Value = entity.Name;
                        p.Add("@Address", SqlDbType.NVarChar, 400).Value = string.IsNullOrWhiteSpace(entity.Address) ? (object)DBNull.Value : entity.Address;
                        p.Add("@Phone", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(entity.Phone) ? (object)DBNull.Value : entity.Phone;
                        p.Add("@IsActive", SqlDbType.Bit).Value = entity.IsActive;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int locationID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Locations_Delete",
                    p=>
                    {
                        p.Add("@LocationID", SqlDbType.Int).Value = locationID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<List<clsLocationsEntities>> GetAllLocationsAsync()
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_Locations_GetAll",
                    reader => _MapToSomeColumnsLocations(reader));
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetAllLocationsAsync (SQL)", ex);
                return new List<clsLocationsEntities>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetAllLocationsAsync (General)", ex);
                return new List<clsLocationsEntities>();
            }
        }
        public static async Task<(List<clsLocationsEntities> LocationsData, int TotalPages)> GetLocationsPageAsync
            (int PageNumber , int PageSize , string FilterColumn=null , string FilterValue = null)
        {
            int TotalCount = 0;

            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsLocationsEntities>("SP_Locations_GetPage",
                    reader =>
                    {
                        var item = _MapToLocations(reader);

                        if (TotalCount == 0)
                            TotalCount = Convert.ToInt32(reader["TotalCount"]);

                        return item;
                    },
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar,128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar,200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue; ;
                    });

                int totalPages = 0;
                if(list.Count > 0)
                {
                    totalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                }

                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationsPageAsync (SQL)", ex);
                return (new List<clsLocationsEntities>(),0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.GetLocationsPageAsync (General)", ex);
                return (new List<clsLocationsEntities>(), 0);
            }

        }
        public static async Task<bool> IsAddressExistsAsync(int? locationID,string address)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Locations_ExistsAddress",
                    p=>
                    {
                        p.Add("@LocationID", SqlDbType.Int).Value = locationID ?? (object)DBNull.Value;
                        p.Add("@Address", SqlDbType.NVarChar, 400).Value = address;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsAddressExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsAddressExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsPhoneExistsAsync(int? locationID ,string phone)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Locations_ExistsPhone",
                    p =>
                    {
                        p.Add("@LocationID", SqlDbType.Int).Value = locationID?? (object)DBNull.Value;
                        p.Add("@Phone", SqlDbType.NVarChar, 50).Value = phone;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsPhoneExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsPhoneExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsLocationIDExistsAsync(int locationID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Locations_ExistsLocationID",
                    p => p.Add("@LocationID", SqlDbType.Int).Value = locationID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsLocationIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsLocationsData.IsLocationIDExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsLocationsEntities _MapToLocations(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "LocationID",
                "Name",
                "Address",
                "Phone",
                "IsActive",
                "IsDeleted",
                "CreatedDate");

            return new clsLocationsEntities
            {
               LocationID = reader.GetInt32(cols["LocationID"]),
               Name = reader.GetString(cols["Name"]),
               Address = reader.GetStringOrNull(cols["Address"]),
               Phone = reader.GetStringOrNull(cols["Phone"]),
               IsActive = reader.GetBoolean(cols["IsActive"]),
               IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
               CreatedDate = reader.GetDateTime(cols["CreatedDate"])
            };

        }
        private static clsLocationsEntities _MapToSomeColumnsLocations(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "LocationID",
                "Name",
                "Address");

            return new clsLocationsEntities
            {
                LocationID = reader.GetInt32(cols["LocationID"]),
                Name = reader.GetString(cols["Name"]),
                Address = reader.GetStringOrNull(cols["Address"])
            };

        }
    }
}
