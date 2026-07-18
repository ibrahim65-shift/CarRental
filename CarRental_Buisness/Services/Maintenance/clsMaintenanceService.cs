using CarRental_Buisness.Mappers;
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

namespace CarRental_Buisness.Services.Maintenance
{
    public class clsMaintenanceService
    {
        private readonly clsMaintenanceValidator _validator = new clsMaintenanceValidator();

        public async Task<clsServiceResult<clsMaintenanceDto>> GetMaintenanceByMaintenanceIDAsync(int maintenanceID)
        {
            var entity = await clsMaintenanceData.GetMaintenanceByMaintenanceIDAsync(maintenanceID);
            if (entity == null)
                return clsServiceResult<clsMaintenanceDto>.Fail("معرف الصيانة غير موجود");

            return clsServiceResult<clsMaintenanceDto>.OK(clsMaintenanceMapper.ToDto(entity));
        }
        public  async Task<clsServiceResult<List<clsMaintenanceDto>>> GetMaintenanceByVehicleIDAsync(int vehicleID)
        {
            var entities = await clsMaintenanceData.GetMaintenanceByVehicleIDAsync(vehicleID);
            if (entities == null)
                return clsServiceResult<List<clsMaintenanceDto>>.Fail("حدث خطأ أثناء جلب البيانات");

            if(!entities.Any())
                return  clsServiceResult<List<clsMaintenanceDto>>.OK(new List<clsMaintenanceDto>());

            var list = entities.Select(e=> clsMaintenanceMapper.ToDto(e)).ToList();
            return clsServiceResult<List<clsMaintenanceDto>>.OK(list);
        }
        public  async Task<clsServiceResult<clsPagedResult<DataTable>>> GetMaintenancePageAsync
          (int PageNumber, int PageSize, string FilterColumn=null, string FilterValue= null)
        {
            var (dt,totalPages) = await clsMaintenanceData.GetMaintenancePageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لايوجد بيانات للصيانات");

            var result = new clsPagedResult<DataTable>
            {
                Data = dt,
                TotalPages = totalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(result);
        }
        public  async Task<clsServiceResult<clsMaintenanceDto>> AddNewAsync(clsMaintenanceAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsMaintenanceDto>.Invalid(validation);

            var entity = new clsMaintenanceEntities
            {
                VehicleID = model.VehicleID,
                Description = model.Description,
                Vendor = model.Vendor,
                Cost = model.Cost,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsMaintenanceData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsMaintenanceDto>.Fail("فشل إضافة الصيانة");

            entity.MaintenanceID = newID.Value;
            return clsServiceResult<clsMaintenanceDto>.OK(clsMaintenanceMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int maintenanceID, clsMaintenanceUpdateModel model)
        {
            var entity = await clsMaintenanceData.GetMaintenanceByMaintenanceIDAsync(maintenanceID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("معرف الصيانة غير موجود");

            var validation =  _validator.ValidateUpdate(model);
            if(!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.Description = model.Description;
            entity.Cost = model.Cost;
            entity.EditedByUserID = clsCurrentUser.UserID;   

            bool update = await clsMaintenanceData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الصيانة");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int maintenanceID)
        {
            if (!await clsMaintenanceData.IsMaintenanceExistsAsync(maintenanceID))
                return clsServiceResult<bool>.Fail("معرف الصيانة غير موجود");

            bool deleted = await clsMaintenanceData.DeleteAsync(maintenanceID);
            return deleted ? clsServiceResult<bool>.OK(true) :
                clsServiceResult<bool>.Fail("فشل حذف الصيانة");
        }
    }
}
