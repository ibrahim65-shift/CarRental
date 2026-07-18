using CarRental_Buisness.Models.VehicleCategory;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleCategory
{
    public class clsVehicleCategoryValidator
    {
        public async Task<clsValidationResult> ValidteVehicleCategoryAsync(int? CategoryID , clsVehicleCategoryCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.CategoryName))
            {
                list.Add("CategoryName", "اسم الفئة لايمكن أن يكون فارغ");
            }
            else if(model.CategoryName.Length > 100)
            {
                list.Add("CategoryName", "اسم الفئة تجاوز الحد المسموح به");
            }
            else
            {
                if(await clsVehicleCategoryData.IsCategoryNameExistsAsync(CategoryID, model.CategoryName))
                {
                    list.Add("CategoryName", "اسم الفئة موجودة بالفعل");
                }
            }

            return list;
        }
    }
}
