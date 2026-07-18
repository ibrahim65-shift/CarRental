using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.VehicleCategory;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Vehicles;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleCategory
{
    public class clsVehicleCategoryService
    {
        private readonly clsVehicleCategoryValidator _validator = new clsVehicleCategoryValidator();

        public async Task<clsServiceResult<clsVehicleCategoryDto>> GetVehicleCategoryByIDAsync(int categoryID)
        {
            var entity = await clsVehicleCategoryData.GetVehicleCategoryByIDAsync(categoryID);
            if (entity == null)
                return clsServiceResult<clsVehicleCategoryDto>.Fail("الفئة غير موجودة");

            return clsServiceResult<clsVehicleCategoryDto>.OK(clsVehicleCategoryMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleCategoryDto>> GetVehicleCategoryByNameAsync(string categoryName)
        {
            var entity = await clsVehicleCategoryData.GetVehicleCategoryByNameAsync(categoryName);
            if (entity == null)
                return clsServiceResult<clsVehicleCategoryDto>.Fail("الفئة غير موجودة");

            return clsServiceResult<clsVehicleCategoryDto>.OK(clsVehicleCategoryMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsVehicleCategoryDto>>>> GetVehicleCategoryPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsVehicleCategoryData.GetVehicleCategoryPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.categoryData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsVehicleCategoryDto>>>.Fail("بيانات الفئات غير موجودة");

            var list = result.categoryData.Select(clsVehicleCategoryMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsVehicleCategoryDto>>
            {
                Data = list,
                TotalPages =result.TotalPages
            };

            return clsServiceResult<clsPagedResult<List<clsVehicleCategoryDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsVehicleCategoryDto>> AddNewAsync(clsVehicleCategoryCreateUpdateModel model)
        {
            var validation = await _validator.ValidteVehicleCategoryAsync(null,model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleCategoryDto>.Invalid(validation);

            var entity = new clsVehicleCategoryEntities
            {
                CategoryName = model.CategoryName,
            };

            var newID = await clsVehicleCategoryData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleCategoryDto>.Fail("فشل إضافة فئة جديدة");

            entity.CategoryID = newID.Value;
            return clsServiceResult<clsVehicleCategoryDto>.OK(clsVehicleCategoryMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int categoryID , clsVehicleCategoryCreateUpdateModel model)
        {
            var entity = await clsVehicleCategoryData.GetVehicleCategoryByIDAsync(categoryID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الفئة غير موجودة");

            var validation = await _validator.ValidteVehicleCategoryAsync(categoryID, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.CategoryID = categoryID;
            entity.CategoryName = model.CategoryName;

            bool update = await clsVehicleCategoryData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث بيانات الفئة");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int categoryID)
        {
            if(!await clsVehicleCategoryData.IsCategoryIDExistsAsync(categoryID))
                return clsServiceResult<bool>.Fail("الفئة غير موجودة");

            bool deleted = await clsVehicleCategoryData.DeleteAsync(categoryID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الفئة");
        }
    }
}
