using CarRental_Buisness.Models.PaymentMethods;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.PaymentMethods
{
    public class clsPaymentMethodsValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? paymentMethodID , clsPaymentMethodsCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.MethodName))
            {
                list.Add("MethodName", "طريقة الدفع لايمكن أن تكون فارغة");
            }
            else if (model.MethodName.Length > 100)
            {
                list.Add("MethodName", "طريقة الدفع تجاوزت الحد المسموح به");
            }
            else
            {
                if (await clsPaymentMethodsData.IsPaymentMethodsExistsByNameAsync(paymentMethodID, model.MethodName))
                {
                    list.Add("MethodName", "طريقة الدفع موجود مسبقا");
                }
            }


            return list;
        }
    }
}
