using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.InsuranceType;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.InsuranceType
{
    public class clsInsuranceTypeService
    {
        private readonly clsInsuranceTypeValidator _validator = new clsInsuranceTypeValidator();

        public async Task<clsServiceResult<clsInsuranceTypeDto>> GetInsuranceTypeByIDAsync(int InsuranceTypeID)
        {
            var entity = await clsInsuranceTypeData.GetInsuranceTypeByIDAsync(InsuranceTypeID);
            if (entity == null)
                return clsServiceResult<clsInsuranceTypeDto>.Fail("نوع التأمين غير موجود");

            return clsServiceResult<clsInsuranceTypeDto>.OK(clsInsuranceTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsInsuranceTypeDto>> GetInsuranceTypeByTypeAsync(string InsuranceType)
        {
            var entity = await clsInsuranceTypeData.GetInsuranceTypeByTypeAsync(InsuranceType);
            if (entity == null)
                return clsServiceResult<clsInsuranceTypeDto>.Fail("نوع التأمين غير موجود");

            return clsServiceResult<clsInsuranceTypeDto>.OK(clsInsuranceTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsInsuranceTypeDto>>>> GetInsuranceTypePageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsInsuranceTypeData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.InsuranceTypeData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsInsuranceTypeDto>>>.Fail("لاتوجد بيانات");

            var list = result.InsuranceTypeData.Select(clsInsuranceTypeMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsInsuranceTypeDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsInsuranceTypeDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsInsuranceTypeDto>> AddNewAsync(clsInsuranceTypeCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null,model);
            if (!validation.IsValid)
                return clsServiceResult<clsInsuranceTypeDto>.Invalid(validation);

            var entity = new clsInsuranceTypeEntities
            {
                InsuranceType = model.InsuranceType
            };

            var newID = await clsInsuranceTypeData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsInsuranceTypeDto>.Fail("فشل إضافة نوع التأمين");

            entity.InsuranceTypeID = newID.Value;
            return clsServiceResult<clsInsuranceTypeDto>.OK(clsInsuranceTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsInsuranceTypeDto>> UpdateAsync(int InsuranceTypeID, clsInsuranceTypeCreateUpdateModel model)
        {
            var entity = await clsInsuranceTypeData.GetInsuranceTypeByIDAsync(InsuranceTypeID);
            if (entity == null)
                return clsServiceResult<clsInsuranceTypeDto>.Fail("نوع التأمين غير موجود");

            var validation = await _validator.ValidateCreateUpdateAsync(InsuranceTypeID,model);
            if (!validation.IsValid)
                return clsServiceResult<clsInsuranceTypeDto>.Invalid(validation);

            entity.InsuranceType = model.InsuranceType;

            bool update = await clsInsuranceTypeData.UpdateAsync(entity);

            return update ? clsServiceResult<clsInsuranceTypeDto>.OK(clsInsuranceTypeMapper.ToDto(entity))
                : clsServiceResult<clsInsuranceTypeDto>.Fail("فشل تحديث نوع التأمين");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int InsuranceTypeID)
        {
            if (!await clsInsuranceTypeData.IsInsuranceTypeExistsAsync(InsuranceTypeID))
                return clsServiceResult<bool>.Fail("نوع التأمين غير موجود");


            bool deleted = await clsInsuranceTypeData.DeleteAsync(InsuranceTypeID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف نوع التأمين");
        }
    }
}
