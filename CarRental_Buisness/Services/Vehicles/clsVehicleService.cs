using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Vehicles;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Vehicles
{
    public class clsVehicleService
    {
        private readonly clsVehicleValidator _validator = new clsVehicleValidator();

        public async Task<clsServiceResult<clsVehicleDto>> GetVehicleByIDAsync(int vehicleID)
        {
            var entity = await clsVehiclesData.GetVehicleByIDAsync(vehicleID);
            if (entity == null)
                return clsServiceResult<clsVehicleDto>.Fail("معرف السيارة غير موجود");

            return clsServiceResult<clsVehicleDto>.OK(await clsVehicleMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleDto>> GetVehicleByPlateNumberAsync(string plateNumber)
        {
            var entity = await clsVehiclesData.GetVehicleByPlateNumberAsync(plateNumber);
            if (entity == null)
                return clsServiceResult<clsVehicleDto>.Fail("لوحة السيارة غير موجود");

            return clsServiceResult<clsVehicleDto>.OK(await clsVehicleMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleDto>> GetVehicleByVINAsync(string VIN)
        {
            var entity = await clsVehiclesData.GetVehicleByVINAsync(VIN);
            if (entity == null)
                return clsServiceResult<clsVehicleDto>.Fail("هيكل السيارة غير موجود");

            return clsServiceResult<clsVehicleDto>.OK(await clsVehicleMapper.ToDto(entity));
        }
        public  async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var (dt,totalPages) = await clsVehiclesData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var result = new clsPagedResult<DataTable>
            {
                Data = dt,
                TotalPages = totalPages
            };
            

            return clsServiceResult<clsPagedResult<DataTable>>.OK(result);
        }
        public async Task<clsServiceResult<clsVehicleDto>> AddNewAsync(clsVehicleAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleDto>.Invalid(validation);

            var entity = new clsVehiclesEntities
            {
                MakeID = model.MakeID,
                ModelID = model.ModelID,
                Year = model.Year,
                CurrentMileage = model.CurrentMileage,
                RentalPricePerDay = model.RentalPricePerDay,
                FuelTypeID = model.FuelTypeID,
                CategoryID = model.CategoryID,
                PlateNumber = model.PlateNumber,
                VIN = model.VIN,
                Color = model.Color,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsVehiclesData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleDto>.Fail("فشل إضافة مركبة جديدة");

            entity.VehicleID = newID.Value;
           return clsServiceResult<clsVehicleDto>.OK(await clsVehicleMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleDto>> UpdateAsync(int VehicleID,clsVehicleUpdateModel model)
        {
            var entity = await clsVehiclesData.GetVehicleByIDAsync(VehicleID);
            if(entity == null)
                return clsServiceResult<clsVehicleDto>.Fail("معرف السيارة غير موجود");

            var validation = await _validator.ValidateUpdateAsync(VehicleID,model);
            if(!validation.IsValid)
                return clsServiceResult<clsVehicleDto>.Invalid(validation);

            entity.CurrentMileage= model.CurrentMileage;
            entity.RentalPricePerDay = model.RentalPricePerDay;
            entity.StatusID = model.StatusID;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsVehiclesData.UpdateAsync(entity);
            return update ?  clsServiceResult<clsVehicleDto>.OK(await clsVehicleMapper.ToDto(entity)) : clsServiceResult<clsVehicleDto>.Fail("فشل تحديث بيانات السيارة");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int VehicleID)
        {
            if(!await clsVehiclesData.IsVehicleExistsAsync(VehicleID))
                return clsServiceResult<bool>.Fail("معرف السيارة غير موجود");

            bool deleted = await clsVehiclesData.DeleteAsync(VehicleID);
            return deleted  ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف السيارة");
        }

        // ================= Make , Models ======================

        public  async Task<clsServiceResult<List<clsMakesDto>>> GetAllMakesAsync()
        {
            var result = await clsMakeData.GetAllMakesAsync();
            if (result == null || result.Count == 0)
                return clsServiceResult<List<clsMakesDto>>.Fail("لاتوجد بيانات");

            var list = result.Select(clsMakesMapper.ToDto).ToList();

            return clsServiceResult<List<clsMakesDto>>.OK(list);
        }
        public  async Task<clsServiceResult<List<clsModelsDto>>> GetModelsByMakeIDAsync(int makeId)
        {
            var result = await clsModelData.GetModelsByMakeIDAsync(makeId);
            if (result == null || result.Count == 0)
                return clsServiceResult<List<clsModelsDto>>.Fail("لاتوجد بيانات");

            var list = result.Select(clsModelMapper.ToDto).ToList();

            return clsServiceResult<List<clsModelsDto>>.OK(list);
        }

    }
}
