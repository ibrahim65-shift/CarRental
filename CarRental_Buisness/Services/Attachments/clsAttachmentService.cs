using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
            HashSet<string> allowedTables = new HashSet<string>(await clsAttachmentsData.GetAttachmentAllowedTablesAsync());
            bool recordExists = await clsAttachmentsData.IsRelatedRecordExistsAsync(model.RelatedTable, model.RelatedID);

            var validation =  _validator.ValidateAddNewAsync(model , allowedTables , recordExists);
            if (!validation.IsValid)
                return clsServiceResult<clsAttachmentDto>.Invalid(validation);

            var entity = new clsAttachmentsEntities
            {
                RelatedTable = model.RelatedTable,
                RelatedID = model.RelatedID,
                FileName = model.FileName,
                FilePath = model.FilePath,
                MimeType = model.MimeType,
                FileSizeKB = model.FileSizeKB,
                IsPrimary = model.IsPrimary,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsAttachmentsData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsAttachmentDto>.Fail("فشل إضافة مرفق");

            entity.AttachmentID = newID.Value;

            return clsServiceResult<clsAttachmentDto>.OK(clsAttachmentMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int attachmentID, clsAttachmentUpdateModel model)
        {
            var entity = await clsAttachmentsData.GetAttachmentByIDAsync(attachmentID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("المرفق غير موجود");

            var validation =  _validator.ValidateUpdateAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);


            entity.AttachmentID=attachmentID;
            entity.FileName=model.FileName;
            entity.FilePath = model.FilePath;
            entity.MimeType = model.MimeType;
            entity.FileSizeKB = model.FileSizeKB;
            entity.IsPrimary = model.IsPrimary;

            bool update = await clsAttachmentsData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث المرفق");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int attachmentID)
        {
            bool deleted = await clsAttachmentsData.DeleteAsync(attachmentID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف المرفق");
        }
    }
}
