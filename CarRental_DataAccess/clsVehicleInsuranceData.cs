using CarRental_Entities;
using SharedClass;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;

namespace CarRental_DataAccess
{
    public class clsVehicleInsuranceData
    {
        public static async Task<clsVehicleInsuranceEntities> GetVehicleInsuranceByIDAsync(int insuranceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_VehicleInsurance_GetByID", 
                    reader=>_MapToInsurance(reader),
                    p=>p.Add("@insuranceID", SqlDbType.Int).Value=insuranceID);

                return result.SingleOrDefault();
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetVehicleInsuranceByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetVehicleInsuranceByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsVehicleInsuranceEntities>> GetVehicleInsuranceByVehicleIDAsync(int vehicleID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync("SP_VehicleInsurance_GetByVehicleID",
                    reader => _MapToInsurance(reader),
                    p => p.Add("@vehicleID", SqlDbType.Int).Value = vehicleID);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetVehicleInsuranceByVehicleIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetVehicleInsuranceByVehicleIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<(DataTable insuranceData , int TotalPages)> GetPageAsync
            (int PageNumber , int PageSize , string FilterColumn=null, string FilterValue=null)
        {
            try
            {
                var dt = await clsSQLHelper.ExecuteDataTableAsync("SP_VehicleInsurance_GetPage",
                    p =>
                    {
                        p.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                        p.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                        p.Add("@FilterColumn", SqlDbType.NVarChar,128).Value = string.IsNullOrWhiteSpace(FilterColumn) ? (object)DBNull.Value : FilterColumn;
                        p.Add("@FilterValue", SqlDbType.NVarChar,200).Value = string.IsNullOrWhiteSpace(FilterValue) ? (object)DBNull.Value : FilterValue;
                    });

                int totalCount = (dt.Rows.Count > 0 )? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;
                int totalPages = (int) Math.Ceiling(totalCount/(double)PageSize);

                dt.Columns.Remove("TotalCount");

                return (dt, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetPageAsync (SQL)", ex);
                return (new DataTable(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.GetPageAsync (General)", ex);
                return (new DataTable(), 0);
            }
        }
        public static async Task<int?> AddNewAsync(clsVehicleInsuranceEntities entity)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleInsurance_AddNew", 
                    p=>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = entity.VehicleID;
                        p.Add("@PolicyNumber", SqlDbType.NVarChar,200).Value = entity.PolicyNumber;
                        p.Add("@ProviderID", SqlDbType.Int).Value = entity.ProviderID;
                        p.Add("@InsuranceTypeID", SqlDbType.Int).Value = entity.InsuranceTypeID;
                        p.Add("@InsuranceCost", SqlDbType.Decimal).Value = entity.InsuranceCost;
                        p.Add("@StartDate", SqlDbType.Date).Value = entity.StartDate;
                        p.Add("@EndDate", SqlDbType.Date).Value = entity.EndDate;
                        p.Add("@Notes", SqlDbType.NVarChar,500).Value = string.IsNullOrWhiteSpace(entity.Notes) ? (object)DBNull.Value : entity.Notes;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(result);
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsVehicleInsuranceEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleInsurance_Update", 
                    p=>
                    {
                        p.Add("@insuranceID", SqlDbType.Int).Value = entity.InsuranceID;
                        p.Add("@InsuranceTypeID", SqlDbType.Int).Value = entity.InsuranceTypeID;
                        p.Add("@InsuranceCost", SqlDbType.Decimal).Value = entity.InsuranceCost;
                        p.Add("@StartDate", SqlDbType.Date).Value = entity.StartDate;
                        p.Add("@EndDate", SqlDbType.Date).Value = entity.EndDate;
                        p.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(entity.Notes) ? (object)DBNull.Value : entity.Notes;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int insuranceID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var result = await clsSQLHelper.ExecuteNonQueryAsync("SP_VehicleInsurance_Delete", 
                    p=>
                    {
                        p.Add("@insuranceID", SqlDbType.Int).Value = insuranceID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsInsuranceExistsAsync(int insuranceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleInsurance_ExistsInsurance",
                    p => p.Add("@insuranceID", SqlDbType.Int).Value = insuranceID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.IsInsuranceExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.IsInsuranceExistsAsync (General)", ex);
                return false;
            }
        }
        
        public static async Task<bool> ExistsByPolicyNumberAsync(string policyNumber)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleInsurance_ExistsByPolicyNumber",
                    p => p.Add("@PolicyNumber", SqlDbType.NVarChar,200).Value = policyNumber);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.ExistsByPolicyNumberAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.ExistsByPolicyNumberAsync (General)", ex);
                return false;
            }
        }
        
        public static async Task<bool> VehicleHasActiveInsuranceAsync(int vehicleID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleInsurance_HasActiveInsurance",
                    p => p.Add("@VehicleID", SqlDbType.Int).Value = vehicleID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.VehicleHasActiveInsuranceAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.VehicleHasActiveInsuranceAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> IsOverLapsAsync( int? insuranceID, int vehicleID,DateTime startDate , DateTime endDate)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_VehicleInsurance_OverLaps",
                    p =>
                    {
                        p.Add("@VehicleID", SqlDbType.Int).Value = vehicleID;
                        p.Add("@InsuranceID", SqlDbType.Int).Value = insuranceID??(object)DBNull.Value;
                        p.Add("@StartDate", SqlDbType.Date).Value = startDate;
                        p.Add("@EndDate", SqlDbType.Date).Value = endDate;
                    });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.SP_VehicleInsurance_OverLaps (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsVehicleInsuranceData.SP_VehicleInsurance_OverLaps (General)", ex);
                return false;
            }
        }

        private static clsVehicleInsuranceEntities _MapToInsurance(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "InsuranceID",
                "VehicleID",
                "PolicyNumber",
                "ProviderID",
                "InsuranceTypeID",
                "InsuranceCost",
                "StartDate",
                "EndDate",
                "IsActive",
                "Notes",
                "CreatedDate",
                "CreatedByUserID"
                );

            return new clsVehicleInsuranceEntities
            { 
                 InsuranceID = reader.GetInt32(cols["InsuranceID"]),
                 VehicleID = reader.GetInt32(cols["VehicleID"]),
                 PolicyNumber = reader.GetString(cols["PolicyNumber"]),
                 ProviderID = (enInsuranceProviders)reader.GetInt32(cols["ProviderID"]),
                 InsuranceTypeID = (enInsuranceType)reader.GetInt32(cols["InsuranceTypeID"]),
                 InsuranceCost = reader.GetDecimal(cols["InsuranceCost"]),
                 StartDate = reader.GetDateTime(cols["StartDate"]),
                 EndDate = reader.GetDateTime(cols["EndDate"]),
                 IsActive = Convert.ToBoolean(reader.GetInt32(cols["IsActive"])),
                 Notes = reader.GetStringOrNull(cols["Notes"]),
                 CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                 CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"])
            };

        }
    }
}
