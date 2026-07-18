using CarRental_Buisness.Models.InsuranceType;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.InsuranceType
{
    public class clsInsuranceTypeValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? insuranceTypeID
            ,clsInsuranceTypeCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.InsuranceType))
            {
                list.Add("InsuranceType", "نوع التأمين لايمكن أن يكون فارغ");
            }
            else if (model.InsuranceType.Length > 100)
            {
                list.Add("InsuranceType", "نوع التأمين تجاوز الحد المسموح به");
            }
            else
            {
                if (await clsInsuranceTypeData.IsInsuranceTypeExistsByTypeAsync(insuranceTypeID, model.InsuranceType))
                {
                    list.Add("InsuranceType", "نوع التأمين موجود مسبقا");
                }
            }


           return list;
        }
    }
}
