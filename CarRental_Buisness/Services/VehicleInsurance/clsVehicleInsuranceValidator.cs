using CarRental_Buisness.Models.VehicleInsurance;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleInsurance
{
    public class clsVehicleInsuranceValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsVehicleInsuranceAddNewModel model)
        {
            var list = new clsValidationResult();

            if(!await clsVehiclesData.IsVehicleExistsAsync(model.VehicleID))
            {
                list.Add("VehicleID", "معرف السيارة غير صالح");
            }

            if(string.IsNullOrWhiteSpace(model.PolicyNumber))
            {
                list.Add("PolicyNumber", "رقم الوثيقة لايمكن أن يكون فارغ");
            }
            else if(await clsVehicleInsuranceData.ExistsByPolicyNumberAsync(model.PolicyNumber))
            {
                list.Add("PolicyNumber", "رقم الوثيقة مستخدم سابقا");
            }
            else if (model.PolicyNumber.Length > 200)
            {
                list.Add("PolicyNumber", "رقم الوثيقة تجاوز الحد المسموح به");
            }


            if (!Enum.IsDefined(typeof(enInsuranceProviders), model.ProviderID))
            {
                list.Add("ProviderID", "معرف شركة التأمين غير صالح");
            }

            if (!Enum.IsDefined(typeof(enInsuranceType), model.InsuranceTypeID))
            {
                list.Add("InsuranceTypeID", "معرف نوع التأمين غير صالح");
            }

            if (model.EndDate<=model.StartDate)
            {
                list.Add("EndDate", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
            }

            if(model.InsuranceCost <= 0)
            {
                list.Add("InsuranceCost", "تكلفة التأمين لايمكن أن تكون أقل من أو تساوي صفر");
            }

            if (!string.IsNullOrWhiteSpace(model.Notes))
            {
                if (model.Notes.Length > 500)
                {
                    list.Add("Notes", "الملاحظات تجاوزت الحد المسموح به");
                }
            }

            if(model.StartDate.Date < DateTime.Today)
            {
                list.Add("StartDate", "تاريخ البداية غير صالح");
            }

            if(await clsVehicleInsuranceData.VehicleHasActiveInsuranceAsync(model.VehicleID))
            {
                list.Add("VehicleID", "السيارة لديها تأمين ساري المفعول");
            }

            if(await clsVehicleInsuranceData.IsOverLapsAsync(null,model.VehicleID , model.StartDate, model.EndDate))
            {
                list.Add("VehicleID", "السيارة لديها تأمين ساري في هذه الفترة");
            }

            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int? insuranceID , int VehicleID ,clsVehicleInsuranceUpdateModel model)
        {
            var list = new clsValidationResult();

            if (!Enum.IsDefined(typeof(enInsuranceType), model.InsuranceTypeID))
            {
                list.Add("InsuranceTypeID", "معرف نوع التأمين غير صالح");
            }

            if (model.InsuranceCost < 0)
            {
                list.Add("InsuranceCost", "تكلفة التأمين لايمكن أن تكون أقل من صفر");
            }


            if (!string.IsNullOrWhiteSpace(model.Notes))
            {
                if (model.Notes.Length > 500)
                {
                    list.Add("Notes", "الملاحظات تجاوزت الحد المسموح به");
                }
            }

            if (model.StartDate < DateTime.Today)
            {
                list.Add("StartDate", "تاريخ البداية غير صالح");
            }

            if (model.EndDate <= model.StartDate)
            {
                list.Add("EndDate", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
            }

            if (await clsVehicleInsuranceData.IsOverLapsAsync(insuranceID, VehicleID, model.StartDate, model.EndDate))
            {
                list.Add("VehicleID", "السيارة لديها تأمين ساري في هذه الفترة");
            }

            return list;
        }
    }
}
