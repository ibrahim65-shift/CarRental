using CarRental_Buisness.Models.PaymentTransactions;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentTransactions
{
    public class clsPaymentTransactionsValidator
    {
        public async Task<clsValidationResult> ValidateAddNew(clsPaymentTransactionsAddNewModel model)
        {
            var list = new clsValidationResult();
     
            if (model.InvoiceID <= 0) // exists 
                list.Add("InvoiceID", "معرف الفاتورة غير صالح");

            if (!await clsInvoicesData.ExistsAsync(model.InvoiceID))
                list.Add("InvoiceID", "الفاتورة غير موجودة");

            if (await clsPaymentTransactionsData.IsInvoicePaidAsync(model.InvoiceID))
                list.Add("InvoiceID", "الفاتورة مدفوع بالفعل");

            if (!Enum.IsDefined(typeof(enPaymentMethod), model.PaymentMethodID))
                list.Add("PaymentMethodID", "طريقة الدفع غير صحيحة");

            if (!string.IsNullOrWhiteSpace(model.Reference))
            {
                if (model.Reference.Length > 200)
                    list.Add("Reference", "المرجع تجاوز الحد المسموح به");
            }

            if (model.PaidAmount <= 0)
                list.Add("PaidAmount", "المبلغ المدفوع يجب أن يكون أكبر من صفر");

            return list;
        }
        public async Task<clsValidationResult> ValidateRefundAsync(clsPaymentRefundModel model)
        {
            var list = new clsValidationResult();

            if (model.PaymentID <= 0)
            {
                list.Add("PaymentID", "معرف عملية الدفع غير صالح");
                return list;
            }


            if (!Enum.IsDefined(typeof(enPaymentMethod), model.PaymentMethod))
                list.Add("PaymentMethodID", "طريقة الدفع غير صحيحة");

            if (model.RefundAmount <= 0)
                list.Add("RefundAmount", "مبلغ الاسترجاع يجب أن يكون أكبر من صفر");

            if (!string.IsNullOrWhiteSpace(model.Reference))
            {
                model.Reference = model.Reference.Trim();

                if (model.Reference.Length > 200)
                    list.Add("Reference", "المرجع تجاوز الحد المسموح به");
            }

            var payment =await clsPaymentTransactionsData.GetPaymentTransactionsByIDAsync(model.PaymentID);

            if (payment == null)
            {
                list.Add("PaymentID", "عملية الدفع غير موجودة");
                return list;
            }

            if (payment.PaymentStatusID != enPaymentStatus.Paid)
            {
                list.Add("PaymentID", "يمكن استرجاع العمليات المدفوعة فقط");
                return list;
            }

            decimal refundedAmount = await clsPaymentTransactionsData.GetTotalRefundedAmountAsync(model.PaymentID) ?? 0m;

            decimal remaining = payment.PaidAmount - refundedAmount;

            if (model.RefundAmount > remaining)
            {
                list.Add("RefundAmount", $"المبلغ المتبقي القابل للاسترجاع هو {remaining}");
            }

            return list;
        }
    }
}
