using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.VehicleStatus;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.VehicleStatus;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleStatus
{
    public class clsVehicleStatusService
    {
        private readonly clsVehicleStatusValidator _validator = new clsVehicleStatusValidator();

        public async Task<clsServiceResult<clsVehicleStatusDto>> GetVehicleStatusByIDAsync(int StatusID)
        {
            var entity = await clsVehicleStatusData.GetVehicleStatusByIDAsync(StatusID);
            if (entity == null)
                return clsServiceResult<clsVehicleStatusDto>.Fail("الحالة غير موجودة");

            return clsServiceResult<clsVehicleStatusDto>.OK(clsVehicleStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleStatusDto>> GetVehicleStatusByNameAsync(string StatusName)
        {
            var entity = await clsVehicleStatusData.GetVehicleStatusByNameAsync(StatusName);
            if (entity == null)
                return clsServiceResult<clsVehicleStatusDto>.Fail("الحالة غير موجودة");

            return clsServiceResult<clsVehicleStatusDto>.OK(clsVehicleStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsVehicleStatusDto>>>> GetVehicleStatusPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsVehicleStatusData.GetVehicleStatusPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.statusData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsVehicleStatusDto>>>.Fail("بيانات الحالات غير موجودة");

            var list = result.statusData.Select(clsVehicleStatusMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsVehicleStatusDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };

            return clsServiceResult<clsPagedResult<List<clsVehicleStatusDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsVehicleStatusDto>> AddNewAsync(clsVehicleStatusCreateUpdateModel model)
        {
            var validation = await _validator.ValidteVehicleStatusAsync(null, model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleStatusDto>.Invalid(validation);

            var entity = new clsVehicleStatusEntities
            {
                StatusName = model.StatusName,
            };

            var newID = await clsVehicleStatusData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleStatusDto>.Fail("فشل إضافة حالة جديدة");

            entity.StatusID = newID.Value;
            return clsServiceResult<clsVehicleStatusDto>.OK(clsVehicleStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int StatusID, clsVehicleStatusCreateUpdateModel model)
        {
            var entity = await clsVehicleStatusData.GetVehicleStatusByIDAsync(StatusID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الحالة غير موجودة");

            var validation = await _validator.ValidteVehicleStatusAsync(StatusID, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.StatusID = StatusID;
            entity.StatusName = model.StatusName;

            bool update = await clsVehicleStatusData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث بيانات الحالة");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int StatusID)
        {
            if (!await clsVehicleStatusData.IsStatusIDExistsAsync(StatusID))
                return clsServiceResult<bool>.Fail("الحالة غير موجودة");

            bool deleted = await clsVehicleStatusData.DeleteAsync(StatusID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الحالة");
        }
    }
}
