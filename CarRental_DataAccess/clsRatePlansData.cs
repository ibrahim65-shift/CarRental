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
    public class clsRatePlansData
    {

        public static async Task<clsRatePlansEntities> GetRatePlansByIDAsync(int RatePlanID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_RatePlans_GetByID",
                    reader => _MapToRatePlans(reader),
                    p => p.Add("@RatePlanID", SqlDbType.Int).Value = RatePlanID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsRatePlansEntities>> GetRatePlansByCategoryIDAsync(int CategoryID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_RatePlans_GetByCategoryID",
                    reader => _MapToRatePlans(reader),
                    p => p.Add("@CategoryID", SqlDbType.Int).Value = CategoryID);

            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByCategoryIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByCategoryIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsRatePlansEntities>> GetRatePlansByVehicleIDAsync(int VehicleID)
        {
            try
            {
               return await clsSQLHelper.ExecuteReaderAsync("SP_RatePlans_GetByVehicleID",
                    reader => _MapToRatePlans(reader),
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID);

              
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByVehicleIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRatePlansByVehicleIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable ratePlanesData, int TotalPage)> GetPageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_RatePlans_GetPage",
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
                clsEventLogger.LogException("clsRatePlansData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsRatePlansEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RatePlans_AddNew",
                    p =>
                    {
                        p.Add("@RatePlanScope", SqlDbType.NVarChar,20).Value = entity.RatePlanScope;
                        p.Add("@CategoryID", SqlDbType.Int).Value = entity.CategoryID??(object)DBNull.Value;
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID ?? (object)DBNull.Value;
                        p.Add("@StartDate", SqlDbType.Date).Value = entity.StartDate;
                        p.Add("@EndDate", SqlDbType.Date).Value = entity.EndDate;
                        p.Add("@PricePerDay", SqlDbType.Decimal).Value = entity.PricePerDay;
                        p.Add("@MinDays", SqlDbType.Int).Value = entity.MinDays ?? (object)DBNull.Value;
                        p.Add("@Notes", SqlDbType.NVarChar,500).Value = string.IsNullOrWhiteSpace(entity.Notes) ? (object)DBNull.Value:entity.Notes;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsRatePlansEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RatePlans_Update",
                    p =>
                    {
                        p.Add("@RatePlanID", SqlDbType.Int).Value = entity.RatePlanID;
                        p.Add("@RatePlanScope", SqlDbType.NVarChar, 20).Value = entity.RatePlanScope;
                        p.Add("@CategoryID", SqlDbType.Int).Value = entity.CategoryID ?? (object)DBNull.Value;
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID ?? (object)DBNull.Value;
                        p.Add("@StartDate", SqlDbType.Date).Value = entity.StartDate;
                        p.Add("@EndDate", SqlDbType.Date).Value = entity.EndDate;
                        p.Add("@PricePerDay", SqlDbType.Decimal).Value = entity.PricePerDay;
                        p.Add("@MinDays", SqlDbType.Int).Value = entity.MinDays ?? (object)DBNull.Value;
                        p.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(entity.Notes) ? (object)DBNull.Value : entity.Notes;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> SetActiveAsync(int RatePlanID, bool isActive, int editedByUserID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_RatePlans_SetActive",
                    p =>
                    {
                        p.Add("@RatePlanID", SqlDbType.Int).Value = RatePlanID;
                        p.Add("@IsActive", SqlDbType.Bit).Value = isActive;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = editedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.SetActiveAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.SetActiveAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsRatePlansExistsAsync(int RatePlanID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RatePlans_Exists",
                    p => p.Add("@RatePlanID", SqlDbType.Int).Value = RatePlanID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.IsRatePlansExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.IsRatePlansExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> OverLapsAsync(int? ratePlanId,int? CategoryID , int? VehicleID , DateTime StartDate , DateTime EndDate)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RatePlans_OverLaps",
                    p =>
                    {
                        p.Add("@RatePlanID", SqlDbType.Int).Value = ratePlanId ?? (object)DBNull.Value;
                        p.Add("@CategoryID", SqlDbType.Int).Value = CategoryID ?? (object)DBNull.Value;
                        p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID ?? (object)DBNull.Value;
                        p.Add("@StartDate", SqlDbType.Date).Value =StartDate ;
                        p.Add("@EndDate", SqlDbType.Date).Value =EndDate ;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.OverLapsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.OverLapsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<decimal?> GetRentalPricePerDayAsync(int vehicleId , DateTime rentalStartDate , int rentalDays)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_RatePlans_GetRentalPricePerDay",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = vehicleId;
                        p.Add("@RentalStartDate", SqlDbType.DateTime2).Value = rentalStartDate;
                        p.Add("@RentalDays", SqlDbType.Int).Value = rentalDays;
                    });


                return clsSQLHelper.ToDecimalSafe(result);
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRentalPricePerDayAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsRatePlansData.GetRentalPricePerDayAsync (General)", ex);
                return null;
            }

        }
        private static clsRatePlansEntities _MapToRatePlans(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "RatePlanID",
                "RatePlanScope",
                "CategoryID",
                "VehicleID",
                "StartDate",
                "EndDate",
                "PricePerDay",
                "MinDays",
                "Notes",
                "IsActive",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID"
            );

            var scope = reader.GetString(cols["RatePlanScope"]).Trim().ToLower();

            enRatePlaneScope ratePlanScope;

            if (scope == "category")
            {
                ratePlanScope = enRatePlaneScope.Category;
            }
            else if (scope == "vehicle")
            {
                ratePlanScope = enRatePlaneScope.Vehicle;
            }
            else
            {
                throw new Exception("Invalid RatePlanScope value: " + scope);
            }

            return new clsRatePlansEntities
            {
                RatePlanID = reader.GetInt32(cols["RatePlanID"]),
                RatePlanScope = ratePlanScope,

                CategoryID = reader.IsDBNull(cols["CategoryID"])? (enVehicleCategory?)null
                           : (enVehicleCategory)reader.GetInt32(cols["CategoryID"]),

                VehicleID = reader.GetIntOrNull(cols["VehicleID"]),

                StartDate = reader.GetDateTime(cols["StartDate"]),
                EndDate = reader.GetDateTime(cols["EndDate"]),
                PricePerDay = reader.GetDecimal(cols["PricePerDay"]),
                MinDays = reader.GetIntOrNull(cols["MinDays"]),
                Notes = reader.GetStringOrNull(cols["Notes"]),
                IsActive = reader.GetBoolean(cols["IsActive"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"]),
            };
        }
    }
}
