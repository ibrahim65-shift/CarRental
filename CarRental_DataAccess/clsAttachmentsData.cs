using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CarRental_Entities;
using SharedClass;

namespace CarRental_DataAccess
{
    public static class clsAttachmentsData
    {
        public static async Task<clsAttachmentsEntities> GetAttachmentByIDAsync(int attchmentID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteReaderAsync(
                    "SP_Attachments_GetByID",
                     reader => _MapToAttachments(reader),
                     p =>
                     {
                         p.Add("@AttachmentID",SqlDbType.Int).Value= attchmentID;
                     }
                     );

                return result.SingleOrDefault();
            }
            catch(SqlException ex) 
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentByIDAsync (SQL)",ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentByIDAsync (General)", ex);
                return null;
            }


        }
        public static async Task<List<clsAttachmentsEntities>> GetAttachmentsByTableAndIDAsync(string TableName , int ID)
        {
            try
            {
                return await clsSQLHelper.ExecuteReaderAsync<clsAttachmentsEntities>(
                    "SP_Attachments_GetByRelatedTableAndID",
                    reader => _MapToAttachments(reader),
                    p =>
                    {
                        p.Add("@RelatedTable", SqlDbType.NVarChar, 128).Value = TableName;
                        p.Add("@RelatedID", SqlDbType.Int).Value = ID;

                    });
            }
            catch(SqlException ex )
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentsByTableAndID (SQL)", ex);
                return new List<clsAttachmentsEntities>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentsByTableAndID (General)", ex);
                return new List<clsAttachmentsEntities>();
            }

          
        }
        public static async Task<int?> AddNewAsync(clsAttachmentsEntities entity)
        {
            try
            {
                var newID = await clsSQLHelper.ExecuteScalarAsync(
                    "SP_Attachments_AddNew",
                    p =>
                    {
                        p.Add("@FileName", SqlDbType.NVarChar, 255).Value = entity.FileName;
                        p.Add("@RelatedTable", SqlDbType.NVarChar, 128).Value = entity.RelatedTable;
                        p.Add("@RelatedID", SqlDbType.Int).Value = entity.RelatedID;
                        p.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(entity.FilePath) ? (object)DBNull.Value : entity.FilePath;
                        p.Add("@MimeType", SqlDbType.NVarChar, 100).Value  = string.IsNullOrWhiteSpace(entity.MimeType) ? (object)DBNull.Value : entity.MimeType;
                        p.Add("@FileSizeKB", SqlDbType.Int).Value = entity.FileSizeKB ?? (object)DBNull.Value;
                        p.Add("@IsPrimary", SqlDbType.Bit).Value = entity.IsPrimary;
                        p.Add("@CreatedByUserID", SqlDbType.Int).Value = entity.CreatedByUserID;
                    });

                return clsSQLHelper.ToInt32Safe(newID);
            }
            catch(SqlException ex )
            {
                clsEventLogger.LogException("clsAttachmentsData.AddNewAsync (SQL)", ex);
                return null;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.AddNewAsync (General)", ex);
                return null;
            }

           
        }
        public static async Task<bool> UpdateAsync(clsAttachmentsEntities entity)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };
                var rowAffected =  await clsSQLHelper.ExecuteNonQueryAsync("SP_Attachments_Update", p =>
                {
                    p.Add("@AttachmentID", SqlDbType.Int).Value = entity.AttachmentID;
                    p.Add("@FileName", SqlDbType.NVarChar, 255).Value = entity.FileName;
                    p.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(entity.FilePath) ? (object)DBNull.Value : entity.FilePath;
                    p.Add("@MimeType", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(entity.MimeType) ? (object)DBNull.Value : entity.MimeType;
                    p.Add("@FileSizeKB", SqlDbType.Int).Value = entity.FileSizeKB ?? (object)DBNull.Value;
                    p.Add("@IsPrimary", SqlDbType.Bit).Value = entity.IsPrimary;
                    p.Add(isSuccessParam);
                });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch(SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.UpdateAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.UpdateAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> DeleteAsync(int AttachmentID)
        {
            try
            {
                SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output,
                };

                var rowAffected = await clsSQLHelper.ExecuteNonQueryAsync("SP_Attachments_Delete",
                    p =>
                    {
                        p.Add("@AttachmentID", SqlDbType.Int).Value = AttachmentID;
                        p.Add(isSuccessParam);
                    });

                return isSuccessParam.Value != DBNull.Value && (bool)isSuccessParam.Value;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.DeleteAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.DeleteAsync (General)", ex);
                return false;
            }
        }
        public static async Task<(List<clsAttachmentsEntities> list , int totalPages)> GetAttachmentsPageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            int totalCount = 0;
            try
            {
                var list = await clsSQLHelper.ExecuteReaderAsync<clsAttachmentsEntities>("SP_Attachments_GetPage",
                    reader =>
                    {
                        var item = _MapToAttachments(reader);

                        if (totalCount == 0)
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

                if (list.Count > 0)
                {
                    totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                }

                return (list, totalPages);
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentsPageAsync (SQL)", ex);
                return (new List<clsAttachmentsEntities>(),0);
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentsPageAsync (General)", ex);
                return (new List<clsAttachmentsEntities>(), 0);
            }
        }
        public static async Task<bool> IsRelatedRecordExistsAsync(string RelatedTable , int RelatedID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Attachments_IsRelatedRecordExists",
                   p =>
                   {
                       p.Add("@RelatedTable", SqlDbType.NVarChar, 128).Value = RelatedTable;
                       p.Add("@RelatedID", SqlDbType.Int).Value = RelatedID;
                   });

                return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.IsRelatedRecordExistsAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.IsRelatedRecordExistsAsync (General)", ex);
                return false;
            }
        }
        public static async Task<bool> ExistsPrimaryAsync(int attachmentID,string RelatedTable , int RelatedID)
        {
            try
            {
                var result = await clsSQLHelper.ExecuteScalarAsync("SP_Attachments_ExistsPrimary",
                    p =>
                    {
                        p.Add("@AttachmentID", SqlDbType.Int).Value = attachmentID;
                        p.Add("@RelatedTable", SqlDbType.NVarChar,128).Value = RelatedTable;
                        p.Add("@RelatedID", SqlDbType.Int).Value = RelatedID;
                    });

               return clsSQLHelper.ToInt32Safe(result) == 1;
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.ExistsPrimaryAsync (SQL)", ex);
                return false;
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.ExistsPrimaryAsync (General)", ex);
                return false;
            }
        }
        public static async Task<List<string>> GetAttachmentAllowedTablesAsync()
        {
           

            try
            {
                   return await clsSQLHelper.ExecuteReaderAsync<string>(
                    "SP_GetAttachmentAllowedTables",
                    reader => reader["TableName"].ToString(),
                    null
                     );
            }
            catch (SqlException ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentAllowedTablesAsync (SQL)", ex);
                return new List<string>();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsAttachmentsData.GetAttachmentAllowedTablesAsync (General)", ex);
                return new List<string>();
            }
        }
        private static clsAttachmentsEntities _MapToAttachments(SqlDataReader reader)
        {
            var cols = clsSQLHelper.GetOrdinal(reader,
                "AttachmentID",
                "RelatedTable",
                "RelatedID",
                "FileName",
                "FilePath",
                "MimeType",
                "FileSizeKB",
                "IsPrimary",
                "IsDeleted",
                "CreatedDate",
                "CreatedByUserID" 
                );


            return new clsAttachmentsEntities
            {
                AttachmentID = reader.GetInt32(cols["AttachmentID"]),
                RelatedTable = reader.GetString(cols["RelatedTable"]),
                RelatedID = reader.GetInt32(cols["RelatedID"]),
                FileName = reader.GetString(cols["FileName"]),
                FilePath = reader.GetStringOrNull(cols["FilePath"]),
                MimeType = reader.GetStringOrNull(cols["MimeType"]),
                FileSizeKB = reader.GetIntOrNull(cols["FileSizeKB"]),
                IsPrimary = reader.GetBoolean(cols["IsPrimary"]),
                IsDeleted = reader.GetBoolean(cols["IsDeleted"]),
                CreatedDate = reader.GetDateTime(cols["CreatedDate"]),
                CreatedByUserID = reader.GetInt32(cols["CreatedByUserID"])
            };
        }
    }
}
