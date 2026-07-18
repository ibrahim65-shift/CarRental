using CarRental_Buisness.Models.PaymentMethods;
using CarRental_Buisness.Models.PaymentStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentStatus
{
    public class clsPaymentStatusValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? paymentMethodID, clsPaymentStatusCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.StatusName))
            {
                list.Add("StatusName", "حالة الدفع لايمكن أن تكون فارغة");
            }
            else if (model.StatusName.Length > 100)
            {
                list.Add("StatusName", "حالة الدفع تجاوزت الحد المسموح به");
            }
            else
            {
                if (await clsPaymentStatusData.IsPaymentStatusExistsByNameAsync(paymentMethodID, model.StatusName))
                {
                    list.Add("StatusName", "حالة الدفع موجود مسبقا");
                }
            }


            return list;
        }
    }
}
