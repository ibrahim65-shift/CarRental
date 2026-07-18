using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Invoices
{
    public class clsInvoicesValidator
    {
        private const string NegativeAmountMessage = "المبلغ المدخل لايجب أن يكون سالب";
        private void ValidateCommon(clsValidationResult list, decimal AdditionalCharges, decimal LateFees,
            decimal DiscountAmount, string Notes)
        {
            if (AdditionalCharges < 0)
                list.Add("AdditionalCharges", NegativeAmountMessage);

            if (LateFees < 0)
                list.Add("LateFees", NegativeAmountMessage);

            if (DiscountAmount < 0)
                list.Add("DiscountAmount", NegativeAmountMessage);

            if (!string.IsNullOrWhiteSpace(Notes))
            {
                if (Notes.Length > 1000)
                    list.Add("Notes", "الملاحظات تجاوزت الحد المسموح به");
            }
        }
        public async Task<clsValidationResult> ValidateAddNewAsync(clsInvoicesAddNewModel model)
        {
            var list = new clsValidationResult();

            ValidateCommon(list,model.AdditionalCharges,model.LateFees,model.DiscountAmount,model.Notes);

            if(!Enum.IsDefined(typeof(enInvoiceTypes),model.InvoiceTypeID))
            {
                list.Add("InvoiceTypeID", "نوع الفاتورة غير صحيح");
            }

            if (model.BookingID.HasValue)
            {
                if (!await clsRentalBookingData.IsRentalBookingExistsAsync(model.BookingID.Value))
                {
                    list.Add("BookingID", "عملية الحجز غير موجودة");
                }
            }

            if (model.MaintenanceID.HasValue)
            {
                if (!await clsMaintenanceData.IsMaintenanceExistsAsync(model.MaintenanceID.Value))
                {
                    list.Add("MaintenanceID", "عملية الصيانة غير موجودة");
                }
            }

            if (model.DamageID.HasValue)
            {
                if (!await clsVehicleDamageData.IsDamageIDExistsAsync(model.DamageID.Value))
                {
                    list.Add("DamageID", "عملية الضرر غير موجودة");
                }
            }



            if (model.BaseAmount < 0)
                list.Add("BaseAmount", NegativeAmountMessage);

            if (model.TaxAmount < 0)
                list.Add("TaxAmount", NegativeAmountMessage);


            if(string.IsNullOrWhiteSpace(model.CurrencyCode))
            {
                list.Add("CurrencyCode", "العملة لايجب أن تكون فارغة");
            }
            else if(model.CurrencyCode.Length >10)
            {
                list.Add("CurrencyCode", "العملة تجاوزت الحد المسموح به");
            }
               
            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int InvoiceID,clsInvoicesUpdateModel model)
        {
            var list = new clsValidationResult();

            ValidateCommon(list, model.AdditionalCharges, model.LateFees, model.DiscountAmount, model.Notes);


            if (await clsPaymentTransactionsData.IsInvoicePaidAsync(InvoiceID))
                list.Add("InvoiceID", "تم الدفع لايمكن تعديل الفاتورة");

            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateLinkedInvoiceAsync(int InvoiceID,clsUpdateLinkedInvoiceModel model)
        {
            var list = new clsValidationResult();

            ValidateCommon(list, model.AdditionalCharges, model.LateFees, model.DiscountAmount, model.Notes);

            if (await clsPaymentTransactionsData.IsInvoicePaidAsync(InvoiceID))
                list.Add("InvoiceID", "تم الدفع لايمكن تعديل الفاتورة");

            return list;
        }

    }
}
