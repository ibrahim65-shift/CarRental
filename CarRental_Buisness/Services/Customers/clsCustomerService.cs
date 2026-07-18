using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Customers;
using CarRental_Buisness.Models.FuelTypes;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Customers
{
    public class clsCustomerService
    {
        private readonly clsCustomerValidator _validator = new clsCustomerValidator();

        public async Task<clsServiceResult<clsCustomerDto>> GetCustomerByCustomerIDAsync(int customerID)
        {
            var entity = await clsCustomersData.GetCustomerByCustomerIDAsync(customerID);
            if (entity == null)
                return clsServiceResult<clsCustomerDto>.Fail("العميل غير موجود");

            return clsServiceResult<clsCustomerDto>.OK(clsCustomerMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsCustomerDto>> GetCustomerByPersonIDAsync(int personID)
        {
            var entity = await clsCustomersData.GetCustomerByPersonIDAsync(personID);
            if (entity == null)
                return clsServiceResult<clsCustomerDto>.Fail("العميل غير موجود");

            return clsServiceResult<clsCustomerDto>.OK(clsCustomerMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetCustomersPageAsync
            (int PageNumber, int PageSize, string ColumnName = null, string searchText = null)
        {
            var (dt , totalPages) =await clsCustomersData.GetCustomersPageAsync(PageNumber, PageSize, ColumnName, searchText);
            if (dt.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لايوجد عملاء");

            var result = new clsPagedResult<DataTable>
            {
                Data = dt,
                TotalPages = totalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(result);
        }
        public async Task<clsServiceResult<clsCustomerDto>> AddNewAsync(clsCustomerAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsCustomerDto>.Invalid(validation);

            var entity = new clsCustomersEntities
            {
                PersonID = model.PersonID,
                DriverLicenseNumber = model.DriverLicenseNumber,
                DriverLicenseExpiry = model.DriverLicenseExpiry,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                CreatedByUserID = clsCurrentUser.UserID,
                EditedDate= null,
                EditedByUserID= null,
            };

            var newID = await clsCustomersData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsCustomerDto>.Fail("فشل إضافة عميل");

            entity = await clsCustomersData.GetCustomerByCustomerIDAsync(newID.Value);
            return clsServiceResult <clsCustomerDto>.OK(clsCustomerMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsCustomerDto>> UpdateAsync(int customerID , clsCustomerUpdateModel model)
        {
            var validation = await _validator.ValidateUpdateAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsCustomerDto>.Invalid(validation);

           
            var entity = await clsCustomersData.GetCustomerByCustomerIDAsync(customerID);
            if (entity == null)
                return clsServiceResult<clsCustomerDto>.Fail("العميل غير موجود");

            entity.DriverLicenseExpiry = model.DriverLicenseExpiry;
            entity.EditedDate = DateTime.Now;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsCustomersData.UpdateAsync(entity);
            if (!update)
                return clsServiceResult<clsCustomerDto>.Fail("فشل تحديث بيانات العميل");

            return clsServiceResult<clsCustomerDto>.OK(clsCustomerMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int customerID)
        {
            bool deleted = await clsCustomersData.DeleteAsync(customerID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف العميل");
        }
        public async Task<clsServiceResult<bool>> ExistsByPersonIDAsync(int PersonID)
        {
            bool exists = await clsCustomersData.ExistsByPersonIDAsync(PersonID);
            return exists ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("الشخص غير موجود");
        }

        // ======================= Reports =======================

        public async Task<clsServiceResult<DataTable>> GetReportAllCustomersAsync(DateTime fromDate, DateTime toDate)
        {
            var result = await clsCustomersData.GetReportAllCustomersAsync(fromDate, toDate);
            if (result == null || result.Rows.Count == 0)
                return clsServiceResult<DataTable>.Fail("لاتوجد بيانات في المدة المحددة");

            return clsServiceResult<DataTable>.OK(result);
        }
    }

}
