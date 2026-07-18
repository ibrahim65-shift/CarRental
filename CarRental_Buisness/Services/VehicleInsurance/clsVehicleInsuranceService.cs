using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.InsuranceProviders;
using CarRental_Buisness.Models.VehicleInsurance;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleInsurance
{
    public class clsVehicleInsuranceService
    {
        private readonly clsVehicleInsuranceValidator _validatro = new clsVehicleInsuranceValidator();

        public async Task<clsServiceResult<clsVehicleInsuranceDto>> GetVehicleInsuranceByIDAsync(int insuranceID)
        {
            var entity = await clsVehicleInsuranceData.GetVehicleInsuranceByIDAsync(insuranceID);
            if (entity == null)
                return clsServiceResult<clsVehicleInsuranceDto>.Fail("التأمين غير موجود");

            return clsServiceResult<clsVehicleInsuranceDto>.OK(clsVehicleInsuranceMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsVehicleInsuranceDto>>> GetVehicleInsuranceByVehicleIDAsync(int vehicleID)
        {
            var entities = await clsVehicleInsuranceData.GetVehicleInsuranceByVehicleIDAsync(vehicleID);
            if (entities == null)
                return clsServiceResult<List<clsVehicleInsuranceDto>>.Fail("لايوجد تأمينات لهذه السيارة");

            var list = entities.Select(e=>clsVehicleInsuranceMapper.ToDto(e)).ToList();
            return clsServiceResult<List<clsVehicleInsuranceDto>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var dt = await clsVehicleInsuranceData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.insuranceData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var paged = new clsPagedResult<DataTable>
            {
                Data = dt.insuranceData,
                TotalPages = dt.TotalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }

        public async Task<clsServiceResult<clsVehicleInsuranceDto>> AddNewAsync(clsVehicleInsuranceAddNewModel model)
        {
            var validation = await _validatro.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleInsuranceDto>.Invalid(validation);

            var entity = new clsVehicleInsuranceEntities
            {
                VehicleID = model.VehicleID,
                PolicyNumber = model.PolicyNumber,
                ProviderID = model.ProviderID,
                InsuranceCost = model.InsuranceCost,
                InsuranceTypeID = model.InsuranceTypeID,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Notes = model.Notes,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsVehicleInsuranceData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleInsuranceDto>.Fail("فشل إضافة تأمين جديد");

            entity.InsuranceID = newID.Value;
            return clsServiceResult<clsVehicleInsuranceDto>.OK(clsVehicleInsuranceMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleInsuranceDto>> UpdateAsync(int insuranceID , clsVehicleInsuranceUpdateModel model)
        {
            var entity = await clsVehicleInsuranceData.GetVehicleInsuranceByIDAsync(insuranceID);
            if (entity == null)
                return clsServiceResult<clsVehicleInsuranceDto>.Fail("التأمين غير موجود");

            var validation = await _validatro.ValidateUpdateAsync(insuranceID,entity.VehicleID, model);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleInsuranceDto>.Invalid(validation);

            entity.InsuranceCost = model.InsuranceCost;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.InsuranceTypeID = model.InsuranceTypeID;
            entity.Notes = model.Notes;

            bool update = await clsVehicleInsuranceData.UpdateAsync(entity);
            return update ? clsServiceResult<clsVehicleInsuranceDto>.OK(clsVehicleInsuranceMapper.ToDto(entity)) :
                clsServiceResult<clsVehicleInsuranceDto>.Fail("فشل تحديث التأمين");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int insuranceID)
        {
            if(!await clsVehicleInsuranceData.IsInsuranceExistsAsync(insuranceID))
            {
                return clsServiceResult<bool>.Fail("معرف التأمين غير صالح");
            }

            bool deleted = await clsVehicleInsuranceData.DeleteAsync(insuranceID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف التأمين");
        }
        public async Task<clsServiceResult<List<clsInsuranceProvidersDto>>> GetAllInsuranceProvidersAsync()
        {
            var result = await clsInsuranceProvidersData.GetAllInsuranceProvidersAsync();

            if (result.Count == 0 || result == null)
                return clsServiceResult<List<clsInsuranceProvidersDto>>.Fail("لاتوجد بيانات");

            var list = result.Select(e=>clsInsuranceProvidersMapper.ToDto(e)).ToList();

            return clsServiceResult<List<clsInsuranceProvidersDto>>.OK(list);
        }
    }
}
