using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.VehicleDamage;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleDamage
{
    public class clsVehicleDamageService
    {
        private readonly clsVehicleDamageValidator _validator = new clsVehicleDamageValidator();

        public async Task<clsServiceResult<clsVehicleDamageDto>> GetVehicleDamageByIDAsync(int damageID)
        {
            var entity = await clsVehicleDamageData.GetVehicleDamageByIDAsync(damageID);
            if (entity == null)
                return clsServiceResult<clsVehicleDamageDto>.Fail("الضرر غير موجود");

            return clsServiceResult<clsVehicleDamageDto>.OK(clsVehicleDamageMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleDamageDto>> GetVehicleDamageByBookingIDAsync(int bookingID)
        {
            var entity = await clsVehicleDamageData.GetVehicleDamageByBookingIDAsync(bookingID);
            if (entity == null)
                return clsServiceResult<clsVehicleDamageDto>.Fail("الضرر غير موجود");

            return clsServiceResult<clsVehicleDamageDto>.OK(clsVehicleDamageMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsVehicleDamageDto>>> GetVehicleDamageByVehilceIDAsync(int vehilceID)
        {
            var list = await clsVehicleDamageData.GetVehicleDamageByVehilceIDAsync(vehilceID);
            if (list.Count == 0)
                return clsServiceResult<List<clsVehicleDamageDto>>.Fail("بيانات الضرر غير موجودة");

            var data = list.Select(clsVehicleDamageMapper.ToDto).ToList();
            
            return clsServiceResult < List < clsVehicleDamageDto >>.OK(data);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetVehicleDamagePageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var dt = await clsVehicleDamageData.GetVehicleDamagePageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if(dt.DamageData.Rows.Count == 0)
                return clsServiceResult <clsPagedResult<DataTable>>.Fail("بيانات الضرر غير موجودة");

            var paged = new clsPagedResult<DataTable>
            {
                Data = dt.DamageData,
                TotalPages = dt.TotalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public async Task<clsServiceResult<clsVehicleDamageDto>> AddNewAsync(clsVehicleDamageAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleDamageDto>.Invalid(validation);

            var entity = new clsVehicleDamageEntities
            {
                VehicleID = model.VehicleID,
                BookingID = model.BookingID,
                Description = model.Description,
                EstimatedCost = model.EstimatedCost,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsVehicleDamageData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleDamageDto>.Fail("فشل إضافة ضرر السيارة");

            entity.DamageID = newID.Value;
            return clsServiceResult<clsVehicleDamageDto>.OK(clsVehicleDamageMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int damageID , clsVehicleDamageUpdateModel model)
        {
            var entity = await clsVehicleDamageData.GetVehicleDamageByIDAsync(damageID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الضرر غير موجود");

            var validation = await _validator.ValidateUpdateAsync(damageID, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.Description = model.Description;
            entity.EstimatedCost = model.EstimatedCost;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsVehicleDamageData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث بيانات الضرر");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int damageID)
        {
            if (!await clsVehicleDamageData.IsDamageIDExistsAsync(damageID))
                return clsServiceResult<bool>.Fail("معرف الضرر غير صحيح");

            bool deleted = await clsVehicleDamageData.DeleteAsync(damageID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف بيانات الضرر");
        }
    }
}
