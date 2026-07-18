using CarRental_Buisness.Models.FuelTypes;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.FuelTypes
{
    public class clsFuelTypeValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsFuelTypeCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.FuelTypeName))
            {
                list.Add("FuelTypeName", "نوع الوقود لايمكن أن يكون فارغ");
            }
            else if (model.FuelTypeName.Length > 100)
            {
                list.Add("FuelTypeName", "نوع الوقود تجاوز الحد المسموح به");
            }
            else if (await clsFuelTypesData.IsFuelTypeNameExistsAsync(model.FuelTypeName))
            {
                list.Add("FuelTypeName", "نوع الوقود موجود بالفعل");
            }

                return list;
        }

        public async Task<clsValidationResult> ValidateUpdateAsync(clsFuelTypeCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.FuelTypeName))
            {
                list.Add("FuelTypeName", "نوع الوقود لايمكن أن يكون فارغ");
            }

            return list;
        }
    }
}
