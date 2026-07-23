using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.FileStorge;
using CarRental_DataAccess;
using CarRental_Entities;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Attachments
{
    public class clsAttachmentService
    {
        private readonly clsAttachmentValidator _validator = new clsAttachmentValidator();

        public async Task<clsServiceResult<clsAttachmentDto>> GetAttachmentByIDAsync(int attchmentID)
        {
            var entity = await clsAttachmentsData.GetAttachmentByIDAsync(attchmentID);
            if (entity == null)
                return clsServiceResult<clsAttachmentDto>.Fail("المرفق غير موجود");

            return clsServiceResult<clsAttachmentDto>.OK(clsAttachmentMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>> GetAttachmentsByTableAndIDAsync(string RelatedTable, int RelateID)
        {
            var entities = await clsAttachmentsData.GetAttachmentsByTableAndIDAsync(RelatedTable, RelateID);
            if (entities == null || entities.Count==0)
                return clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>.Fail("لاتوجد بيانات");

            var list = new clsPagedResult<List<clsAttachmentDto>>
            {
                Data = entities.Select(clsAttachmentMapper.ToDto).ToList(),
            };

            return clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>> GetAttachmentsPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsAttachmentsData.GetAttachmentsPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.list.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>.Fail("لاتوجد بيانات");

           var dtoList = result.list.Select(clsAttachmentMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsAttachmentDto>>
            {
                Data = dtoList,
                TotalPages = result.totalPages
            };

            return clsServiceResult<clsPagedResult<List<clsAttachmentDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsAttachmentDto>> AddNewAsync(clsAttachmentAddNewModel model)
        {
            if (!File.Exists(model.SourceFilePath))
                return clsServiceResult<clsAttachmentDto>.Fail("الملف غير موجود");

            string mimeType = clsFileStorageService.GetMimeType(model.SourceFilePath);
            int fileSizeKB = clsFileStorageService.GetFileSizeKB(model.SourceFilePath);

            HashSet<string> allowedTables = new HashSet<string>(await clsAttachmentsData.GetAttachmentAllowedTablesAsync());
            bool recordExists = await clsAttachmentsData.IsRelatedRecordExistsAsync(model.RelatedTable, model.RelatedID);

            var validation = _validator.ValidateAddNewAsync(model,allowedTables, mimeType, fileSizeKB, recordExists);
            if (!validation.IsValid)
                return clsServiceResult<clsAttachmentDto>.Invalid(validation);

            string savedFilePath = clsFileStorageService.SaveFile(model.SourceFilePath);

            var entity = new clsAttachmentsEntities
            {
                RelatedTable = model.RelatedTable,
                RelatedID = model.RelatedID,
                FileName = model.FileName,
                FilePath = savedFilePath,
                MimeType = mimeType,
                FileSizeKB = fileSizeKB,
                IsPrimary = model.IsPrimary,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newId = await clsAttachmentsData.AddNewAsync(entity);

            if(newId == null)
            {
                clsFileStorageService.DeleteFile(savedFilePath);
                return clsServiceResult<clsAttachmentDto>.Fail("فشل إضافة المرفق");
            }

            entity.AttachmentID = newId.Value;

            return clsServiceResult<clsAttachmentDto>.OK(clsAttachmentMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int attachmentID, clsAttachmentUpdateModel model)
        {
            var entity = await clsAttachmentsData.GetAttachmentByIDAsync(attachmentID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("المرفق غير موجود");

            bool fileChanged = !string.IsNullOrWhiteSpace(model.SourceFilePath);
            string mimeType = entity.MimeType;
            int? fileSizeKB = entity.FileSizeKB;

            if (fileChanged)
            {
                if (!File.Exists(model.SourceFilePath))
                    return clsServiceResult<bool>.Fail("الملف غير موجود");

                mimeType = clsFileStorageService.GetMimeType(model.SourceFilePath);
                fileSizeKB = clsFileStorageService.GetFileSizeKB(model.SourceFilePath);
            }

            var validation =  _validator.ValidateUpdateAsync(model,mimeType,fileSizeKB);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            string oldFilePath = entity.FilePath;
            string newFilePath = null;
            

            if(fileChanged)
            {
                newFilePath = clsFileStorageService.SaveFile(model.SourceFilePath);
                entity.FilePath = newFilePath;
                entity.FileSizeKB = fileSizeKB;
                entity.MimeType = mimeType;
            }

            entity.FileName=model.FileName;
            entity.IsPrimary = model.IsPrimary;

            bool update = await clsAttachmentsData.UpdateAsync(entity);

            if(update)
            {
                if (fileChanged)
                    clsFileStorageService.DeleteFile(oldFilePath);

                return clsServiceResult<bool>.OK(true);
            }

            if (fileChanged)
                clsFileStorageService.DeleteFile(newFilePath);

            return  clsServiceResult<bool>.Fail("فشل تحديث المرفق");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int attachmentID)
        {
            var entity = await clsAttachmentsData.GetAttachmentByIDAsync(attachmentID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("المرفق غير موجود");

            bool deleted = await clsAttachmentsData.DeleteAsync(attachmentID);

            if(!deleted)
                return clsServiceResult<bool>.Fail("فشل حذف المرفق");

            clsFileStorageService.DeleteFile(entity.FilePath);
            return clsServiceResult<bool>.OK(true);
        }
    }
}
