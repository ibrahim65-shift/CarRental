using CarRental_Buisness.Models.RatePlans;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.RatePlans
{
    public class clsRatePlansValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? ratePlanId,clsRatePlansCreateUpdateModel model)
        {
            var list = new clsValidationResult();

           
            if (!Enum.IsDefined(typeof(enRatePlaneScope), model.RatePlanScope))
            {
                list.Add("RatePlanScope", "نطاق الخطة غير صالح");
            }

            if (model.RatePlanScope == enRatePlaneScope.Category)
            {
                if (model.CategoryID == null)
                    list.Add("CategoryID","يجب تحديد الفئة عندما يكون نطاق الخطة حسب الفئة");

                if (model.VehicleID != null)
                    list.Add("VehicleID", "لا يمكن تحديد مركبة عندما يكون النطاق حسب الفئة");
            }
            else if (model.RatePlanScope == enRatePlaneScope.Vehicle)
            {
                if (model.VehicleID == null)
                    list.Add("VehicleID","يجب تحديد المركبة عندما يكون نطاق الخطة حسب المركبة");

                if (model.CategoryID != null)
                    list.Add("CategoryID", "لا يمكن تحديد فئة عندما يكون النطاق حسب المركبة");
            }

            if (model.CategoryID != null)
            {
                if (!await clsVehicleCategoryData.IsCategoryIDExistsAsync((int)model.CategoryID))
                {
                    list.Add("CategoryID", "معرف الفئة غير صالح");
                }
            }

            if (model.VehicleID != null)
            {

                if (!await clsVehiclesData.IsVehicleExistsAsync(model.VehicleID.Value))
                {
                    list.Add("VehicleID", "معرف المركبة غير صالح");
                }
            }

          
            if (model.StartDate < DateTime.Today)
            {
                list.Add("StartDate", "يرجى إدخال تاريخ بداية صحيح");
            }

            if (model.EndDate < model.StartDate)
            {
                list.Add("EndDate", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
            }


            int? categoryId = model.RatePlanScope == enRatePlaneScope.Category ? (int?)model.CategoryID: null;

            if (await clsRatePlansData.OverLapsAsync(ratePlanId,categoryId, model.VehicleID, model.StartDate, model.EndDate))
            {
                list.Add("OverLaps","هناك نطاق خطة يتقاطع مع نطاق الخطة الحالية");
            }

            if (model.PricePerDay <= 0)
            {
                list.Add("PricePerDay", "سعر الإيجار اليومي يجب أن يكون أكبر من صفر");
            }

            if (!string.IsNullOrWhiteSpace(model.Notes))
            {
                if (model.Notes.Length > 500)
                {
                    list.Add("Notes", "الملاحظات تجاوزت الحد المسموح به");
                }
            }

            return list;
        }
    }
}
