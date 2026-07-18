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
    public class clsVehiclesData
    {
        public static async Task<clsVehiclesEntities> GetVehicleByIDAsync(int vehicleID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Vehicles_GetByID",
                    reader => _MapToVehicle(reader),
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = vehicleID);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByID (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByID (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehiclesEntities> GetVehicleByPlateNumberAsync(string plateNumber)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Vehicles_GetByPlateNumber",
                  reader => _MapToVehicle(reader),
                  p => p.Add("@PlateNumber", SqlDbType.NVarChar, 20).Value = plateNumber);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByPlateNumberAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByPlateNumberAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsVehiclesEntities> GetVehicleByVINAsync(string VIN)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Vehicles_GetByVIN",
                 reader => _MapToVehicle(reader),
                 p => p.Add("@VIN", SqlDbType.NVarChar, 50).Value = VIN);

                return result.SingleOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByVINAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetVehicleByVINAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable vehiclesData, int TotalPage)> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_Vehicles_GetPage",
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
                clsEventLogger.LogException("clsVehiclesData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
      

        public static async Task<int?> AddNewAsync(clsVehiclesEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Vehicles_AddNew",
                    p =>
                    {
                        p.Add("@MakeID", SqlDbType.Int).Value = entity.MakeID;
                        p.Add("@ModelID", SqlDbType.Int).Value = entity.ModelID;
                        p.Add("@Year", SqlDbType.Int).Value = entity.Year;
                        p.Add("@CurrentMileage", SqlDbType.Int).Value = entity.CurrentMileage;
                        p.Add("@RentalPricePerDay", SqlDbType.Decimal).Value = entity.RentalPricePerDay;
                        p.Add("@FuelTypeID", SqlDbType.Int).Value = entity.FuelTypeID;
                        p.Add("@CategoryID", SqlDbType.Int).Value = entity.CategoryID;
                        p.Add("@PlateNumber", SqlDbType.NVarChar, 20).Value = entity.PlateNumber;
                        p.Add("@VIN", SqlDbType.NVarChar, 50).Value =  entity.VIN;
                        p.Add("@Color", SqlDbType.NVarChar, 50).Value =  entity.Color;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsVehiclesEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Vehicles_Update",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@CurrentMileage", SqlDbType.Int).Value = entity.CurrentMileage;
                        p.Add("@RentalPricePerDay", SqlDbType.Decimal).Value = entity.RentalPricePerDay;
                        p.Add("@StatusID", SqlDbType.Int).Value = entity.StatusID;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int VehicleID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_Vehicles_Delete",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsDuplicatedPlateNumberAsync(int? VehicleID, string PlateNumber)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Vehicles_IsDuplicatedPlateNumber",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID ?? (object)DBNull.Value;
                        p.Add("@PlateNumber", SqlDbType.NVarChar, 20).Value = PlateNumber;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsDuplicatedPlateNumberAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsDuplicatedPlateNumberAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsDuplicatedVINAsync(int? VehicleID, string VIN)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Vehicles_IsDuplicatedVIN",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID ?? (object)DBNull.Value;
                        p.Add("@VIN", SqlDbType.NVarChar, 50).Value = VIN;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsDuplicatedVINAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsDuplicatedVINAsync (General)", ex);
                return false;
            }
        }
        public static async Task<int?> GetCurrentMileageAsync(int VehicleID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Vehicles_GetCurrentMileage",
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID);

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetCurrentMileageAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.GetCurrentMileageAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> IsVehicleExistsAsync(int VehicleID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Vehicles_ExistsVehicle",
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = VehicleID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsVehicleExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehiclesData.IsVehicleExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsVehiclesEntities _MapToVehicle(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "VehicleID",
                "MakeID",
                "ModelID",
                "Year",
                "CurrentMileage",
                "RentalPricePerDay",
                "FuelTypeID",
                "CategoryID",
                "PlateNumber",
                "StatusID",
                "VIN",
                "Color",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID",
                "EditedDate",
                "EditedByUserID");

            return new clsVehiclesEntities
            {
                VehicleID = reader.GetInt32(cols["VehicleID"]),
                MakeID = reader.GetInt32(cols["MakeID"]),
                ModelID = reader.GetInt32(cols["ModelID"]),
                Year = reader.GetInt32(cols["Year"]),
                CurrentMileage = reader.GetInt32(cols["CurrentMileage"]),
                RentalPricePerDay = reader.GetDecimal(cols["RentalPricePerDay"]),
                FuelTypeID = (enFuelType)reader.GetInt32(cols["FuelTypeID"]),
                CategoryID = (enVehicleCategory)reader.GetInt32(cols["CategoryID"]),
                PlateNumber = reader.GetString(cols["PlateNumber"]),
                StatusID = (enVehicleStatus)reader.GetInt32(cols["StatusID"]),
                VIN = reader.GetString(cols["VIN"]),
                Color = reader.GetString(cols["Color"]),
                IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };

        }

    }
}