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
    public static class clsModelData
    {
        public static async Task<string> GetModelByModelIDAsync(int modelId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Models_GetModelByModelID",
                    p => p.Add("@modelId", SqlDbType.Int).Value = modelId);

                return result.ToString();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsModelData.GetMakeByMakeIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsModelData.GetMakeByMakeIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsModelsEntities>> GetModelsByMakeIDAsync(int makeId)
        {
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync("SP_Models_GetByMakeID",
                 reader => _MapToModels(reader),
                 p => p.Add("@MakeID", SqlDbType.Int).Value = makeId);

                return list;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsModelData.GetModelsByMakeIDAsync (SQL)", ex);
                return new List<clsModelsEntities>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsModelData.GetModelsByMakeIDAsync (General)", ex);
                return new List<clsModelsEntities>();
            }
        }
        public static async Task<bool> IsModelIDExistsAsync(int modelId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Models_IsExists",
                    p => p.Add("@ModelID", SqlDbType.Int).Value = modelId);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsModelData.IsModelIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsModelData.IsModelIDExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsModelsEntities _MapToModels(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal
                (
                    reader, "ModelID", "ModelName"
                );

            return new clsModelsEntities
            {
                ModelID = reader.GetInt32(cols["ModelID"]),
                ModelName = reader.GetString(cols["ModelName"])
            };
        }
    }
}
