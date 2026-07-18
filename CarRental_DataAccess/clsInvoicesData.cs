using CarRental_Entities.Invoices;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public static class clsInvoicesData
    {
        public static async Task<clsInvoiceEntities> GetInvoiceByIDAsync(int invoiceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Invoices_GetByID",
                        reader => _MapToInvoice(reader),
                        p => p.Add("@InvoiceID", SqlDbType.Int).Value = invoiceID);

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsInvoiceEntities> GetInvoiceByBookingIDAsync(int bookingID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Invoices_GetByBookingID",
                        reader => _MapToInvoice(reader),
                        p => p.Add("@BookingID", SqlDbType.Int).Value = bookingID);

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByBookingIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByBookingIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsInvoiceEntities> GetInvoiceByMaintenanceIDAsync(int maintenanceId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Invoices_GetByMaintenanceID",
                        reader => _MapToInvoice(reader),
                        p => p.Add("@MaintenacneID", SqlDbType.Int).Value = maintenanceId);

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByMaintenanceIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByMaintenanceIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<clsInvoiceEntities> GetInvoiceByDamageIDAsync(int damageId)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync("SP_Invoices_GetByDamageID",
                        reader => _MapToInvoice(reader),
                        p => p.Add("@DamageID", SqlDbType.Int).Value = damageId);

                return result.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByDamageIDAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetInvoiceByDamageIDAsync (General)", ex);
                return null;
            }
        }
        public static async Task<int?> AddNewAsync(clsInvoiceEntities entity)
        {
            try
            {
                SqlParameter newInvoiceID = new SqlParameter("@NewInvoiceID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteScalarAsync("SP_Invoices_AddNew",
                    p =>
                    {
                        p.Add("@InvoiceTypeID", SqlDbType.Int).Value = entity.InvoiceTypeID;
                        p.Add("@BookingID", SqlDbType.Int).Value = entity.BookingID ?? (object)DBNull.Value;
                        p.Add("@MaintenanceID", SqlDbType.Int).Value = entity.MaintenanceID ?? (object)DBNull.Value;
                        p.Add("@DamageID", SqlDbType.Int).Value = entity.DamageID ?? (object)DBNull.Value;
                        p.Add(clsSQLHelper.CreateDecimalParameter("@BaseAmount",entity.BaseAmount));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@AdditionalCharges",entity.AdditionalCharges));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@LateFees",entity.LateFees));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@TaxAmount",entity.TaxAmount));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@DiscountAmount", entity.DiscountAmount));
                        p.Add("@CurrencyCode", SqlDbType.NVarChar, 10).Value = entity.CurrencyCode;
                        p.Add("@Notes", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(entity.Notes)? (object)DBNull.Value: entity.Notes;
                        p.Add("@CreatedByUserID", SqlDbType.Int) .Value = entity.CreatedByUserID;

                        p.Add(newInvoiceID);
                    });

                return clsSQLHelper.ToInt32Safe(newInvoiceID.Value);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.AddNewAsync (General)", ex);
                return null;
            }
        }
        public static async Task<bool> UpdateAsync(clsInvoiceEntities entity)
        {
            try
            {
                SqlParameter isSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_Invoices_Update",
                    p =>
                    {
                        p.Add("@InvoiceID", SqlDbType.Int).Value = entity.InvoiceID;
                        p.Add(clsSQLHelper.CreateDecimalParameter("@AdditionalCharges",entity.AdditionalCharges));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@LateFees",entity.LateFees));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@DiscountAmount",entity.DiscountAmount));
                        p.Add("@Notes", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(entity.Notes)? (object)DBNull.Value: entity.Notes;
                        p.Add("@EditedByUserID",SqlDbType.Int).Value = entity.EditedByUserID.HasValue ? (object)entity.EditedByUserID.Value: DBNull.Value;

                        p.Add(isSuccess);
                    });

                return isSuccess.Value != DBNull.Value
                    && (bool)isSuccess.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> UpdateLinkedInvoiceAsync(int entityId ,clsInvoiceEntities entity)
        {
            try
            {
                SqlParameter isSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                await clsSQLHelper.ExecuteNonQueryAsync("SP_Invoices_UpdateLinkedInvoice",
                    p =>
                    {
                        p.Add("@EntityID", SqlDbType.Int).Value = entityId;
                        p.Add("@InvoiceTypeID", SqlDbType.Int).Value = entity.InvoiceTypeID;
                        p.Add(clsSQLHelper.CreateDecimalParameter("@BaseAmount", entity.BaseAmount));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@AdditionalCharges", entity.AdditionalCharges));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@LateFees", entity.LateFees));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@TaxAmount", entity.TaxAmount));
                        p.Add(clsSQLHelper.CreateDecimalParameter("@DiscountAmount", entity.DiscountAmount));
                        p.Add("@CurrencyCode", SqlDbType.NVarChar, 10).Value = entity.CurrencyCode;
                        p.Add("@Notes", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(entity.Notes) ? (object)DBNull.Value : entity.Notes;
                        p.Add("@EditedByUserID", SqlDbType.Int).Value = entity.EditedByUserID.HasValue ? (object)entity.EditedByUserID.Value : DBNull.Value;

                        p.Add(isSuccess);
                    });

                return isSuccess.Value != DBNull.Value
                    && (bool)isSuccess.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.UpdateLinkedInvoiceAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.UpdateLinkedInvoiceAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(List<clsInvoicesViewEntities> InvoicesData, int TotalPages)> GetPageAsync(int pageNumber,
            int pageSize, string filterColumn = null,string filterValue = null)
        {
            int totalCount = 0;

            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsInvoicesViewEntities>("SP_Invoices_GetPage",

                reader =>
                {
                    var item = _MapToInvoiceView(reader);

                    if (totalCount == 0)
                    {
                        totalCount = Convert.ToInt32(reader["TotalCount"]);
                    }

                    return item;
                },

                p =>
                {
                    p.Add("@PageNumber",SqlDbType.Int).Value = pageNumber;
                    p.Add("@PageSize",SqlDbType.Int).Value = pageSize;
                    p.Add("@FilterColumn",SqlDbType.NVarChar, 128).Value = string.IsNullOrWhiteSpace(filterColumn)? (object)DBNull.Value : filterColumn;
                    p.Add("@FilterValue",SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(filterValue) ? (object)DBNull.Value: filterValue;
                });

                int totalPages = 0;

                if (list.Count > 0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                }

                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetPageAsync (SQL)", ex);
                return (new List<clsInvoicesViewEntities>(), 0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.GetPageAsync (General)", ex);
                return (new List<clsInvoicesViewEntities>(), 0);
            }
        }

        public static async Task<bool> ExistsAsync(int invoiceID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Invoices_IsExists", 
                    p => p.Add("@InvoiceID", SqlDbType.Int).Value = invoiceID);

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsInvoicesData.ExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsInvoicesData.ExistsAsync (General)", ex);
                return false;
            }
        }
        private static clsInvoiceEntities _MapToInvoice(SqlDataReader reader )
        {
            var cols = clsSQLHelper.GetOrdinal(
              reader,
              
              "InvoiceID",
              "InvoiceNumber",
              "InvoiceTypeID",
              "BookingID",
              "MaintenanceID",
              "DamageID",
              "InvoiceDate",
              "BaseAmount",
              "AdditionalCharges",
              "LateFees",
              "TaxAmount",
              "DiscountAmount",
              "TotalAmount",
              "CurrencyCode",
              "Notes",
              "CreatedDate",
              "CreatedByUserID",
              "EditedDate",
              "EditedByUserID"
            );

            return new clsInvoiceEntities
            {
                InvoiceID = reader.GetInt32(cols["InvoiceID"]),
                InvoiceNumber = reader.GetString(cols["InvoiceNumber"]),
                InvoiceTypeID = (enInvoiceTypes)reader.GetInt32(cols["InvoiceTypeID"]),
                BookingID = reader.GetIntOrNull(cols["BookingID"]),
                MaintenanceID = reader.GetIntOrNull(cols["MaintenanceID"]),
                DamageID = reader.GetIntOrNull(cols["DamageID"]),
                InvoiceDate = reader.GetDateTime(cols["InvoiceDate"]),
                BaseAmount = reader.GetDecimal(cols["BaseAmount"]),
                AdditionalCharges = reader.GetDecimal(cols["AdditionalCharges"]),
                LateFees = reader.GetDecimal(cols["LateFees"]),
                TaxAmount = reader.GetDecimal(cols["TaxAmount"]),
                DiscountAmount = reader.GetDecimal(cols["DiscountAmount"]),
                TotalAmount = reader.GetDecimal(cols["TotalAmount"]),
                CurrencyCode = reader.GetString(cols["CurrencyCode"]),
                Notes = reader.GetStringOrNull(cols["Notes"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"])
            };
        }
        private static clsInvoicesViewEntities _MapToInvoiceView(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(
             reader,

             "InvoiceID",
             "InvoiceNumber",
             "TypeName",
             "BookingID",
             "MaintenanceID",
             "DamageID",
             "InvoiceDate",
             "BaseAmount",
             "AdditionalCharges",
             "LateFees",
             "TaxAmount",
             "DiscountAmount",
             "TotalAmount",
             "CurrencyCode",
             "Notes",
             "CreatedDate",
             "CreatedByUserID",
             "EditedDate",
             "EditedByUserID",
             "CustomerID",
             "VehicleID"
           );

            return new clsInvoicesViewEntities
            {
                InvoiceID = reader.GetInt32(cols["InvoiceID"]),
                InvoiceNumber = reader.GetString(cols["InvoiceNumber"]),
                BookingID = reader.GetIntOrNull(cols["BookingID"]),
                MaintenanceID = reader.GetIntOrNull(cols["MaintenanceID"]),
                DamageID = reader.GetIntOrNull(cols["DamageID"]),
                InvoiceDate = reader.GetDateTime(cols["InvoiceDate"]),
                BaseAmount = reader.GetDecimal(cols["BaseAmount"]),
                AdditionalCharges = reader.GetDecimal(cols["AdditionalCharges"]),
                LateFees = reader.GetDecimal(cols["LateFees"]),
                TaxAmount = reader.GetDecimal(cols["TaxAmount"]),
                DiscountAmount = reader.GetDecimal(cols["DiscountAmount"]),
                TotalAmount = reader.GetDecimal(cols["TotalAmount"]),
                CurrencyCode = reader.GetString(cols["CurrencyCode"]),
                Notes = reader.GetStringOrNull(cols["Notes"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"]),
                EditedDate = reader.GetDateTimeOrNull(cols["EditedDate"]),
                EditedByUserID = reader.GetIntOrNull(cols["EditedByUserID"]),

                TypeName = reader.GetString(cols["TypeName"]),
                CustomerID = reader.GetIntOrNull(cols["CustomerID"]),
                VehicleID = reader.GetIntOrNull(cols["VehicleID"])
            };
        }
    }
}