using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public class clsInsuranceProvidersData
    {
        public static async Task<List<clsInsuranceProvidersEntities>> GetAllInsuranceProvidersAsync()
        {
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync("SP_InsuranceProviders_GetAll",
                 reader => _MapToInsuranceProviders(reader));

                return list;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInsuranceProvidersData.GetAllInsuranceProvidersAsync (SQL)", ex);
                return new List<clsInsuranceProvidersEntities>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInsuranceProvidersData.GetAllInsuranceProvidersAsync (General)", ex);
                return new List<clsInsuranceProvidersEntities>();
            }
        }
        private static clsInsuranceProvidersEntities _MapToInsuranceProviders(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal
                (
                    reader, "ProviderName"
                );

            return new clsInsuranceProvidersEntities
            {
                ProviderName = reader.GetString(cols["ProviderName"])
            };
        }
    }
}
