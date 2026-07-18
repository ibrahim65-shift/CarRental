using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.PaymentTransactions;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentTransactions
{
    public class clsPaymentTransactionsService
    {
        private readonly clsPaymentTransactionsValidator _validator = new clsPaymentTransactionsValidator();

        public async Task<clsServiceResult<clsPaymentTransactionsDto>> GetPaymentTransactionsByIDAsync(int PaymentID)
        {
            var entity = await clsPaymentTransactionsData.GetPaymentTransactionsByIDAsync(PaymentID);
            if (entity == null)
                return clsServiceResult<clsPaymentTransactionsDto>.Fail("معرف الدفع غير صالح");

            return clsServiceResult<clsPaymentTransactionsDto>.OK(clsPaymentTransactionsMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsPaymentTransactionsDto>>> GetPaymentTransactionsByInvoiceIDAsync(int InvoiceID)
        {
            var entity = await clsPaymentTransactionsData.GetPaymentTransactionsByInvoiceIDAsync(InvoiceID);
            if (entity == null)
                return clsServiceResult<List<clsPaymentTransactionsDto>>.Fail("معرف الفاتورة غير صالح");

            var list = entity.Select(clsPaymentTransactionsMapper.ToDto).ToList();
            return clsServiceResult<List<clsPaymentTransactionsDto>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
                 (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var data = await clsPaymentTransactionsData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (data.paymentData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var paged = new clsPagedResult<DataTable>
            {
                Data = data.paymentData,
                TotalPages = data.TotalPage
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public async Task<clsServiceResult<clsPaymentTransactionsDto>> AddNewAsync(clsPaymentTransactionsAddNewModel model)
        {
            var validation =  await _validator.ValidateAddNew(model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentTransactionsDto>.Invalid(validation);

            var entity = new clsPaymentTransactionsEntities
            {

                InvoiceID = model.InvoiceID,
                PaymentMethodID = model.PaymentMethodID,
                PaidAmount = model.PaidAmount,
                Reference = model.Reference,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsPaymentTransactionsData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsPaymentTransactionsDto>.Fail("فشل إضافة معاملة الدفع");

            entity.PaymentID = newID.Value;

            return clsServiceResult<clsPaymentTransactionsDto>.OK(clsPaymentTransactionsMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPaymentTransactionsDto>> AddRefundAsync(clsPaymentRefundModel model)
        {
            var validation = await _validator.ValidateRefundAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsPaymentTransactionsDto>.Invalid(validation);

            var refundID = await clsPaymentTransactionsData.AddRefundAsync(  model.PaymentID,model.PaymentMethod,model.RefundAmount, model.Reference,
                                      clsCurrentUser.UserID);

            if (refundID == null)
                return clsServiceResult<clsPaymentTransactionsDto>.Fail("فشل إضافة عملية الاسترجاع");

            return await GetPaymentTransactionsByIDAsync(refundID.Value);
        }
        public async Task<clsServiceResult<bool>> IsInvoicePaidAsync(int invoiceId)
        {
            bool isPaid = await clsPaymentTransactionsData.IsInvoicePaidAsync(invoiceId);
            return isPaid ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("الفاتورة غير مدفوعة");
        }

    }
}
