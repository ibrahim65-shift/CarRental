using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.ReturnStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.ReturnStatus
{
    public class clsReturnStatusService
    {
        private readonly clsReturnStatusValidator _validator = new clsReturnStatusValidator();

        public async Task<clsServiceResult<clsReturnStatusDto>> GetReturnStatusByIDAsync(int ReturnStatusID)
        {
            var entity = await clsReturnStatusData.GetReturnStatusByIDAsync(ReturnStatusID);
            if (entity == null)
                return clsServiceResult<clsReturnStatusDto>.Fail("حالة الإرجاع غير موجودة");

            return clsServiceResult<clsReturnStatusDto>.OK(clsReturnStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsReturnStatusDto>> GetReturnStatusByTypeAsync(string ReturnStatus)
        {
            var entity = await clsReturnStatusData.GetReturnStatusByNameAsync(ReturnStatus);
            if (entity == null)
                return clsServiceResult<clsReturnStatusDto>.Fail("حالة الإرجاع غير موجودة");

            return clsServiceResult<clsReturnStatusDto>.OK(clsReturnStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsReturnStatusDto>>>> GetReturnStatusPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsReturnStatusData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.ReturnStatusData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsReturnStatusDto>>>.Fail("لاتوجد بيانات");

            var list = result.ReturnStatusData.Select(clsReturnStatusMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsReturnStatusDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsReturnStatusDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsReturnStatusDto>> AddNewAsync(clsReturnStatusCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null, model);
            if (!validation.IsValid)
                return clsServiceResult<clsReturnStatusDto>.Invalid(validation);

            var entity = new clsReturnStatusEntities
            {
                StatusName = model.StatusName
            };

            var newID = await clsReturnStatusData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsReturnStatusDto>.Fail("فشل إضافة حالة الإرجاع");

            entity.ReturnStatusID = newID.Value;
            return clsServiceResult<clsReturnStatusDto>.OK(clsReturnStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsReturnStatusDto>> UpdateAsync(int ReturnStatusID, clsReturnStatusCreateUpdateModel model)
        {
            var entity = await clsReturnStatusData.GetReturnStatusByIDAsync(ReturnStatusID);
            if (entity == null)
                return clsServiceResult<clsReturnStatusDto>.Fail("حالة الإرجاع غير موجود");

            var validation = await _validator.ValidateCreateUpdateAsync(ReturnStatusID, model);
            if (!validation.IsValid)
                return clsServiceResult<clsReturnStatusDto>.Invalid(validation);

            entity.StatusName = model.StatusName;

            bool update = await clsReturnStatusData.UpdateAsync(entity);

            return update ? clsServiceResult<clsReturnStatusDto>.OK(clsReturnStatusMapper.ToDto(entity))
                : clsServiceResult<clsReturnStatusDto>.Fail("فشل تحديث حالة الإرجاع");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int ReturnStatusID)
        {
            if (!await clsReturnStatusData.IsReturnStatusExistsAsync(ReturnStatusID))
                return clsServiceResult<bool>.Fail("حالة الإرجاع غير موجود");


            bool deleted = await clsReturnStatusData.DeleteAsync(ReturnStatusID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف حالة الإرجاع");
        }
    }
}
