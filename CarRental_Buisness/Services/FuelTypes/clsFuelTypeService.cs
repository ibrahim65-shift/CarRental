using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.FuelTypes;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.FuelTypes
{
    public class clsFuelTypeService
    {
        private readonly clsFuelTypeValidator _validator = new clsFuelTypeValidator();

        public async Task<clsServiceResult<clsFuelTypeDto>> GetFuelTypeByTypeIDAsync(int fuelTypeID)
        {
            var entity = await clsFuelTypesData.GetFuelTypeByIDAsync(fuelTypeID);
            if (entity == null)
                return clsServiceResult<clsFuelTypeDto>.Fail("نوع الوقود غير موجود");

            return clsServiceResult<clsFuelTypeDto>.OK(clsFuelTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsFuelTypeDto>> GetFuelTypeByFuelTypeNameAsync(string fuelTypeName)
        {
            var entity = await clsFuelTypesData.GetFuelTypeByFuelTypeNameAsync(fuelTypeName);
            if (entity == null)
                return clsServiceResult<clsFuelTypeDto>.Fail("نوع الوقود غير موجود");

            return clsServiceResult<clsFuelTypeDto>.OK(clsFuelTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsFuelTypeDto>>>> GetFuelTypePageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsFuelTypesData.GetFuelTypePageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.fuelTypesData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsFuelTypeDto>>>.Fail("لاتوجد أنواع وقود");

            var list = result.fuelTypesData.Select(clsFuelTypeMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsFuelTypeDto>>
            { 
               Data = list,
               TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsFuelTypeDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsFuelTypeDto>> AddNewAsync(clsFuelTypeCreateUpdateModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsFuelTypeDto>.Invalid(validation);

            var entity = new clsFuelTypesEntities
            {
                FuelTypeName=model.FuelTypeName
            };

            var newID = await clsFuelTypesData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsFuelTypeDto>.Fail("فشل إضافة نوع الوقود");

            entity.FuelTypeID = newID.Value;
            return clsServiceResult<clsFuelTypeDto>.OK(clsFuelTypeMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsFuelTypeDto>> UpdateAsync(int fuelTypeID,clsFuelTypeCreateUpdateModel model)
        {
            var entity = await clsFuelTypesData.GetFuelTypeByIDAsync(fuelTypeID);
            if (entity == null)
                return clsServiceResult<clsFuelTypeDto>.Fail("نوع الوقود غير موجود");

            var validation = await _validator.ValidateUpdateAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsFuelTypeDto>.Invalid(validation);

            entity.FuelTypeName = model.FuelTypeName;

            bool update = await clsFuelTypesData.UpdateAsync(entity);
            
            return update? clsServiceResult<clsFuelTypeDto>.OK(clsFuelTypeMapper.ToDto(entity)) 
                : clsServiceResult<clsFuelTypeDto>.Fail("فشل تحديث نوع الوقود");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int fuelTypeID)
        {
            if (!await clsFuelTypesData.IsFuelTypeIDExistsAsync(fuelTypeID))
                return clsServiceResult<bool>.Fail("نوع الوقود غير موجود");


            bool deleted = await clsFuelTypesData.DeleteAsync(fuelTypeID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف نوع الوقود");
        }
       
    }
}
