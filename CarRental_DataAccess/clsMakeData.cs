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
    public static class clsMakeData
    {
        public static async Task<string> GetMakeByMakeIDAsync(int makeId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Makes_GetMakeByMakeID",
                    p => p.Add("@makeId", SqlDbType.Int).Value = makeId);

                return result.ToString();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMakeData.GetMakeByMakeIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMakeData.GetMakeByMakeIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<List<clsMakesEntities>> GetAllMakesAsync()
        {
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync("SP_Makes_GetAll",
                 reader => _MapToMakes(reader));

                return list;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMakeData.GetAllMakesAsync (SQL)", ex);
                return new List<clsMakesEntities>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMakeData.GetAllMakesAsync (General)", ex);
                return new List<clsMakesEntities>();
            }
        }
        public static async Task<bool> IsMakeIDExistsAsync(int makeId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Makes_IsExists",
                    p => p.Add("@MakeID", SqlDbType.Int).Value = makeId);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsMakeData.IsMakeIDExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsMakeData.IsMakeIDExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsMakesEntities _MapToMakes(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal
                (
                    reader, "MakeID", "MakeName"
                );

            return new clsMakesEntities
            {
                MakeID = reader.GetInt32(cols["MakeID"]),
                MakeName = reader.GetString(cols["MakeName"])
            };
        }
    }
}
