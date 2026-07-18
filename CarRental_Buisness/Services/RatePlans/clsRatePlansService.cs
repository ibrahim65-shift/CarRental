using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.RatePlans;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.RatePlans
{
    public class clsRatePlansService
    {
        private readonly clsRatePlansValidator _validator = new clsRatePlansValidator();

        public async Task<clsServiceResult<clsRatePlansDto>> GetRatePlansByIDAsync(int RatePlanID)
        {
            var entity = await clsRatePlansData.GetRatePlansByIDAsync(RatePlanID);
            if (entity == null)
                return clsServiceResult<clsRatePlansDto>.Fail("لاتوجد خطة إيجار بهذا المعرف");

            return clsServiceResult<clsRatePlansDto>.OK(clsRatePlansMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>> GetRatePlansByCategoryIDAsync(int CategoryID)
        {
            var entities = await clsRatePlansData.GetRatePlansByCategoryIDAsync(CategoryID);
            if (entities == null)
                return clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>.Fail("لاتوجد خطط إيجار لهذه الفئة");

            var list = new clsPagedResult<List<clsRatePlansDto>>
            {
                Data = entities.Select(clsRatePlansMapper.ToDto).ToList(),
              
            };

            return clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>> GetRatePlansByVehicleIDAsync(int VehicleID)
        {
            var entities = await clsRatePlansData.GetRatePlansByVehicleIDAsync(VehicleID);
            if (entities == null)
                return clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>.Fail("لاتوجد خطط إيجار لهذه السيارة");

            var list = new clsPagedResult<List<clsRatePlansDto>>
            {
                Data = entities.Select(clsRatePlansMapper.ToDto).ToList(),

            };

            return clsServiceResult<clsPagedResult<List<clsRatePlansDto>>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
             (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var dt = await clsRatePlansData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.ratePlanesData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var paged = new clsPagedResult<DataTable>
            {
                Data = dt.ratePlanesData,
                TotalPages = dt.TotalPage
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public async Task<clsServiceResult<clsRatePlansDto>> AddNewAsync(clsRatePlansCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null,model);
            if (!validation.IsValid)
                return clsServiceResult<clsRatePlansDto>.Invalid(validation);

            var entity = new clsRatePlansEntities
            {
                RatePlanScope = model.RatePlanScope,
                CategoryID = model.CategoryID,
                VehicleID = model.VehicleID,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PricePerDay = model.PricePerDay,
                MinDays = model.MinDays,
                Notes = model.Notes,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newId = await clsRatePlansData.AddNewAsync(entity);
            if (newId == null)
                return clsServiceResult<clsRatePlansDto>.Fail("فشل إضافة خطة الإيجار");

            entity.RatePlanID = newId.Value;
            return clsServiceResult<clsRatePlansDto>.OK(clsRatePlansMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int RatePlanID,clsRatePlansCreateUpdateModel model)
        {
            var entity = await clsRatePlansData.GetRatePlansByIDAsync(RatePlanID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("لاتوجد خطة إيجار بهذا المعرف");

            var validation = await _validator.ValidateCreateUpdateAsync(RatePlanID,model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.RatePlanScope = model.RatePlanScope;
            entity.CategoryID = model.CategoryID;
            entity.VehicleID = model.VehicleID;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.PricePerDay = model.PricePerDay;
            entity.MinDays = model.MinDays;
            entity.Notes = model.Notes;
            entity.EditedByUserID = clsCurrentUser.UserID;


            bool update = await clsRatePlansData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث خطة الإيجار");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int RatePlaneID)
        {
            if (!await clsRatePlansData.IsRatePlansExistsAsync(RatePlaneID))
                return clsServiceResult<bool>.Fail("معرف الخطة غير صالح");

            bool deleted = await clsRatePlansData.SetActiveAsync(RatePlaneID,false,clsCurrentUser.UserID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف خطة الإيجار");
        }
        public async Task<clsServiceResult<bool>> SetActiveAsync(int RatePlaneID, bool IsActive)
        {
            if (!await clsRatePlansData.IsRatePlansExistsAsync(RatePlaneID))
                return clsServiceResult<bool>.Fail("معرف الخطة غير صالح");

            bool active = await clsRatePlansData.SetActiveAsync(RatePlaneID, IsActive, clsCurrentUser.UserID);
            return active ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تغيير نشاط خطة الإيجار");
        }
    }
}
