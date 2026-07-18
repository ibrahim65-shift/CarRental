using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.PaymentMethods;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.PaymentMethods;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentMethods
{
    public class clsPaymentMethodsService
    {
        private readonly clsPaymentMethodsValidator _validator = new clsPaymentMethodsValidator();

        public async Task<clsServiceResult<clsPaymentMethodsDto>> GetPaymentMethodsByIDAsync(int PaymentMethodID)
        {
            var entity = await clsPaymentMethodsData.GetPaymentMethodsByIDAsync(PaymentMethodID);
            if (entity == null)
                return clsServiceResult<clsPaymentMethodsDto>.Fail("طريقة الدفع غير موجودة");

            return clsServiceResult<clsPaymentMethodsDto>.OK(clsPaymentMethodsMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPaymentMethodsDto>> GetPaymentMethodsByTypeAsync(string PaymentMethods)
        {
            var entity = await clsPaymentMethodsData.GetPaymentMethodsByNameAsync(PaymentMethods);
            if (entity == null)
                return clsServiceResult<clsPaymentMethodsDto>.Fail("طريقة الدفع غير موجودة");

            return clsServiceResult<clsPaymentMethodsDto>.OK(clsPaymentMethodsMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsPaymentMethodsDto>>>> GetPaymentMethodsPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsPaymentMethodsData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.PaymentMethodsData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsPaymentMethodsDto>>>.Fail("لاتوجد بيانات");

            var list = result.PaymentMethodsData.Select(clsPaymentMethodsMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsPaymentMethodsDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsPaymentMethodsDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsPaymentMethodsDto>> AddNewAsync(clsPaymentMethodsCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null, model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentMethodsDto>.Invalid(validation);

            var entity = new clsPaymentMethodsEntities
            {
                MethodName = model.MethodName
            };

            var newID = await clsPaymentMethodsData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsPaymentMethodsDto>.Fail("فشل إضافة طريقة الدفع");

            entity.PaymentMethodID = newID.Value;
            return clsServiceResult<clsPaymentMethodsDto>.OK(clsPaymentMethodsMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPaymentMethodsDto>> UpdateAsync(int PaymentMethodID, clsPaymentMethodsCreateUpdateModel model)
        {
            var entity = await clsPaymentMethodsData.GetPaymentMethodsByIDAsync(PaymentMethodID);
            if (entity == null)
                return clsServiceResult<clsPaymentMethodsDto>.Fail("طريقة الدفع غير موجود");

            var validation = await _validator.ValidateCreateUpdateAsync(PaymentMethodID, model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentMethodsDto>.Invalid(validation);

            entity.MethodName = model.MethodName;

            bool update = await clsPaymentMethodsData.UpdateAsync(entity);

            return update ? clsServiceResult<clsPaymentMethodsDto>.OK(clsPaymentMethodsMapper.ToDto(entity))
                : clsServiceResult<clsPaymentMethodsDto>.Fail("فشل تحديث طريقة الدفع");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int PaymentMethodID)
        {
            if (!await clsPaymentMethodsData.IsPaymentMethodsExistsByIDAsync(PaymentMethodID))
                return clsServiceResult<bool>.Fail("طريقة الدفع غير موجود");


            bool deleted = await clsPaymentMethodsData.DeleteAsync(PaymentMethodID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف طريقة الدفع");
        }
    }
}
