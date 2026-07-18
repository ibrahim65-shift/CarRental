using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Maintenance
{
    public class clsMaintenanceValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsMaintenanceAddNewModel model)
        {
            var list = new clsValidationResult();

            if(model.VehicleID <=0 ||!await clsMaintenanceData.IsVehicleIDExistsAsync(model.VehicleID) )
            {
                list.Add("VehicleID", "معرف السيارة غير صحيح");
            }

            if(!string.IsNullOrWhiteSpace(model.Description))
            {
                if(model.Description.Length > 300)
                {
                    list.Add("Description", "الوصف تجاوز الحد المسموح به");
                }
            }

            if(string.IsNullOrWhiteSpace(model.Vendor))
            {
                list.Add("Vendor", "جهة الصيانة لايمكن أن تكون فارغة");
            }
            else if (model.Vendor.Length > 200)
            {
                list.Add("Vendor", "جهة الصيانة تجاوزت الحد المسموح به");
            }

            if (model.Cost<=0)
            {
                list.Add("Cost", "التكلفة يجب أن تكون أكبر من صفر");
            }

            return list;
        }

        public clsValidationResult ValidateUpdate(clsMaintenanceUpdateModel model)
        {
            var list = new clsValidationResult();



            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                if (model.Description.Length > 300)
                {
                    list.Add("Description", "الوصف تجاوز الحد المسموح به");
                }
            }

            if (model.Cost <= 0)
            {
                list.Add("Cost", "التكلفة يجب أن تكون أكبر من صفر");
            }

            return list;
        }
    }
}
