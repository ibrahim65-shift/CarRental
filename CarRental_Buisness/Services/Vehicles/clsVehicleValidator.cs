using CarRental_Buisness.Models.Vehicles;
using CarRental_Buisness.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClass;
using CarRental_DataAccess;

namespace CarRental_Buisness.Services.Vehicles
{
    public class clsVehicleValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsVehicleAddNewModel model)
        {
            var list = new clsValidationResult();

            if(!await clsMakeData.IsMakeIDExistsAsync(model.MakeID))
            {
                list.Add("MakeID", "معرف الماركة غير صحيح");
            }

            if(!await clsModelData.IsModelIDExistsAsync(model.ModelID))
            {
                list.Add("ModelID", "معرف الموديل غير صحيح");
            }


            if(model.Year<=0)
            {
                list.Add("Year", "السنة يجب أن تكون أكبر من صفر");
            }

            if (model.CurrentMileage < 0)
            {
                list.Add("CurrentMileage", "المسافة المقطوعة يجب أن تكون صفر أو أكبر");
            }

            if (model.RentalPricePerDay<=0)
            {
                list.Add("RentalPricePerDay", "سعر الإيجار اليومي يجب أن يكون أكبر من صفر");
            }

            if(model.FuelTypeID<=0 || !await clsFuelTypesData.IsFuelTypeIDExistsAsync((int)model.FuelTypeID))
            {
                list.Add("FuelTypeID", "معرف نوع الوقود غير صحيح");
            }

            if (model.CategoryID <= 0 || !await clsVehicleCategoryData.IsCategoryIDExistsAsync((int)model.CategoryID))
            {
                list.Add("CategoryID", "معرف نوع الفئة غير صحيح");
            }

            if (string.IsNullOrWhiteSpace(model.PlateNumber))
            {
                list.Add("PlateNumber", "رقم اللوحة لايمكن أن يكون فارغ");
            }
            else if (model.PlateNumber.Length > 20)
            {
                list.Add("PlateNumber", "رقم اللوحة تجاوز الحد المسموح به");
            }
            else
            {
                if(await clsVehiclesData.IsDuplicatedPlateNumberAsync(null,model.PlateNumber))
                {
                    list.Add("PlateNumber", "رقم اللوحة موجود بالفعل");
                }
            }

            if (string.IsNullOrWhiteSpace(model.VIN))
            {
                list.Add("VIN", "رقم الشاصي لايمكن أن يكون فارغ");
            }
            else
            {
                if (model.VIN.Length > 50)
                {
                    list.Add("VIN", "رقم الشاصي تجاوز الحد المسموح به");
                }

                if (await clsVehiclesData.IsDuplicatedVINAsync(null, model.VIN))
                {
                    list.Add("VIN", "رقم الشاصي موجود بالفعل");
                }
            }


            if (string.IsNullOrWhiteSpace(model.Color))
            {
                list.Add("Color", "لون السيارة لايمكن أن يكون فارغ");
            }
            else
            {
                if (model.Color.Length > 50)
                {
                    list.Add("Color", "لون السيارة تجاوز الحد المسموح به");
                }
            }

            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int VehicleID,clsVehicleUpdateModel model)
        {
            var list = new clsValidationResult();

            if (VehicleID <= 0 || !await clsVehiclesData.IsVehicleExistsAsync(VehicleID))
            {
                list.Add("VehicleID", "معرف السيارة غير صحيح");
            }

            var oldMileage =  await clsVehiclesData.GetCurrentMileageAsync(VehicleID);
            if (oldMileage.HasValue && model.CurrentMileage < oldMileage)
            {
                list.Add("CurrentMileage", "لايمكن للمسافة المقطوعة أن تكون أقل من السابقة");
            }

            if (model.RentalPricePerDay <= 0)
            {
                list.Add("RentalPricePerDay", "سعر الإيجار اليومي يجب أن يكون أكبر من صفر");
            }

            if (!Enum.IsDefined(typeof(enVehicleStatus), model.StatusID))
            {
                list.Add("StatusID", "حالة المركبة غير صحيحة");
            }

            return list;
        }
    }
}
