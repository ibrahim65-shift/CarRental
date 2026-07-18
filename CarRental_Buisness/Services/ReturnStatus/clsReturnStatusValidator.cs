using CarRental_Buisness.Models.PaymentMethods;
using CarRental_Buisness.Models.ReturnStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.ReturnStatus
{
    public class clsReturnStatusValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? returnStatusID, clsReturnStatusCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.StatusName))
            {
                list.Add("StatusName", "حالة الإرجاع لايمكن أن تكون فارغة");
            }
            else if (model.StatusName.Length > 100)
            {
                list.Add("StatusName", "حالة الإرجاع تجاوزت الحد المسموح به");
            }
            else
            {
                if (await clsReturnStatusData.IsReturnStatusExistsByNameAsync(returnStatusID, model.StatusName))
                {
                    list.Add("StatusName", "حالة الإرجاع موجود مسبقا");
                }
            }


            return list;
        }
    }
}
