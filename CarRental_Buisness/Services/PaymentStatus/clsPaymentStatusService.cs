using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.PaymentStatus;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.PaymentStatus;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentStatus
{
    public class clsPaymentStatusService
    {
        private readonly clsPaymentStatusValidator _validator = new clsPaymentStatusValidator();

        public async Task<clsServiceResult<clsPaymentStatusDto>> GetPaymentStatusByIDAsync(int PaymentStatusID)
        {
            var entity = await clsPaymentStatusData.GetPaymentStatusByIDAsync(PaymentStatusID);
            if (entity == null)
                return clsServiceResult<clsPaymentStatusDto>.Fail("حالة الدفع غير موجودة");

            return clsServiceResult<clsPaymentStatusDto>.OK(clsPaymentStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPaymentStatusDto>> GetPaymentStatusByTypeAsync(string PaymentStatus)
        {
            var entity = await clsPaymentStatusData.GetPaymentStatusByNameAsync(PaymentStatus);
            if (entity == null)
                return clsServiceResult<clsPaymentStatusDto>.Fail("حالة الدفع غير موجودة");

            return clsServiceResult<clsPaymentStatusDto>.OK(clsPaymentStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsPaymentStatusDto>>>> GetPaymentStatusPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsPaymentStatusData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.PaymentStatusData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsPaymentStatusDto>>>.Fail("لاتوجد بيانات");

            var list = result.PaymentStatusData.Select(clsPaymentStatusMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsPaymentStatusDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsPaymentStatusDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsPaymentStatusDto>> AddNewAsync(clsPaymentStatusCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null, model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentStatusDto>.Invalid(validation);

            var entity = new clsPaymentStatusEntities
            {
                StatusName = model.StatusName
            };

            var newID = await clsPaymentStatusData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsPaymentStatusDto>.Fail("فشل إضافة حالة الدفع");

            entity.PaymentStatusID = newID.Value;
            return clsServiceResult<clsPaymentStatusDto>.OK(clsPaymentStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPaymentStatusDto>> UpdateAsync(int PaymentStatusID, clsPaymentStatusCreateUpdateModel model)
        {
            var entity = await clsPaymentStatusData.GetPaymentStatusByIDAsync(PaymentStatusID);
            if (entity == null)
                return clsServiceResult<clsPaymentStatusDto>.Fail("حالة الدفع غير موجود");

            var validation = await _validator.ValidateCreateUpdateAsync(PaymentStatusID, model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentStatusDto>.Invalid(validation);

            entity.StatusName = model.StatusName;

            bool update = await clsPaymentStatusData.UpdateAsync(entity);

            return update ? clsServiceResult<clsPaymentStatusDto>.OK(clsPaymentStatusMapper.ToDto(entity))
                : clsServiceResult<clsPaymentStatusDto>.Fail("فشل تحديث حالة الدفع");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int PaymentStatusID)
        {
            if (!await clsPaymentStatusData.IsPaymentStatusExistsByIDAsync(PaymentStatusID))
                return clsServiceResult<bool>.Fail("حالة الدفع غير موجود");


            bool deleted = await clsPaymentStatusData.DeleteAsync(PaymentStatusID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف حالة الدفع");
        }
    }
}
