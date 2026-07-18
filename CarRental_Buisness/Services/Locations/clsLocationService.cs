using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Locations
{
    public class clsLocationService
    {
        private readonly clsLocationValidator _validator  = new clsLocationValidator();

        public async Task<clsServiceResult<clsLocationDto>> GetLocationByIDAsync(int locationID)
        {
            var entity = await clsLocationsData.GetLocationByIDAsync(locationID);
            if (entity == null)
                return clsServiceResult<clsLocationDto>.Fail("الفرع غير موجود");

            return clsServiceResult<clsLocationDto>.OK(clsLocationMapper.ToDto(entity));    
        }
        public async Task<clsServiceResult<List<clsLocationDto>>> GetLocationsByNameAsync(string Name)
        {
            var entities = await clsLocationsData.GetLocationsByNameAsync(Name);
            if (entities == null)
                return clsServiceResult<List<clsLocationDto>>.Fail("حدث خطأ أثناء جلب البيانات");

            if (!entities.Any())
                return clsServiceResult<List<clsLocationDto>>.OK(new List<clsLocationDto>());

            var list = entities.Select(e=> clsLocationMapper.ToDto(e)).ToList();
            return clsServiceResult<List<clsLocationDto>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsLocationDto>>>> GetLocationsPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsLocationsData.GetLocationsPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.LocationsData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsLocationDto>>>.Fail("لاتوجد فروع");

            var list = result.LocationsData.Select(clsLocationMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsLocationDto>>
            {
                Data =list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsLocationDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsLocationDto>> AddNewAsync(clsLocationAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsLocationDto>.Invalid(validation);

            var entity = new clsLocationsEntities
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                IsActive =true,
            };

            var newID = await clsLocationsData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsLocationDto>.Fail("فشل إضافة فرع جديد");

            entity.LocationID = newID.Value;
            return clsServiceResult<clsLocationDto>.OK(clsLocationMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsLocationDto>> UpdateAsync(int locationID , clsLocationUpdateModel model)
        {
            var entity = await clsLocationsData.GetLocationByIDAsync(locationID);
            if(entity == null)
                return clsServiceResult<clsLocationDto>.Fail("الفرع غير موجود");

            var validation = await _validator.ValidateUpdateAsync(locationID, model);
            if(!validation.IsValid)
                return clsServiceResult<clsLocationDto>.Invalid(validation);

            entity.Name= model.Name;
            entity.Address= model.Address;
            entity.Phone= model.Phone;
            entity.IsActive = model.IsActive;

            bool update = await clsLocationsData.UpdateAsync(entity);

            return update ? clsServiceResult<clsLocationDto>.OK(clsLocationMapper.ToDto(entity)) 
                : clsServiceResult<clsLocationDto>.Fail("فشل تحديث بيانات الفرع"); 
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int locationID)
        {
            if (!await clsLocationsData.IsLocationIDExistsAsync(locationID))
                return clsServiceResult<bool>.Fail("الفرع غير موجود");

            bool deleted = await clsLocationsData.DeleteAsync(locationID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الفرع");
        }
    }
}
