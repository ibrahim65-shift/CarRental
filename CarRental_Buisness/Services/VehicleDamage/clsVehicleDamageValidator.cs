using CarRental_Buisness.Models.VehicleDamage;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleDamage
{
    public class clsVehicleDamageValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsVehicleDamageAddNewModel model)
        {
            var list = new clsValidationResult();

            if(!await clsVehiclesData.IsVehicleExistsAsync(model.VehicleID))
            {
                list.Add("VehicleID", "معرف السيارة غير صالح");
            }

            if(string.IsNullOrWhiteSpace(model.Description))
            {
                list.Add("Description", "الوصف لايمكن أن يكون فارغ");
            }
            else if(model.Description.Length > 500)
            {
                list.Add("Description", "الوصف تجاوز الحد المسموح به");
            }

            if(model.EstimatedCost !=null)
            {
                 if(model.EstimatedCost <= 0)
                 {
                     list.Add("EstimatedCost", "التكلفة المقدرة يجب أن تكون أكبر من صفر");
                 }
            }

            if(model.BookingID !=null)
            {
                 if(model.BookingID <= 0)
                 {
                     list.Add("BookingID", "معرف الحجز غير صالح");
                 }

                 if(await clsVehicleDamageData.IsDuplicateBookingIDAsync(null,model.BookingID))
                {
                    list.Add("BookingID", "الحجز هذا موجود بالفعل");
                }
            }



                return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int damageID,clsVehicleDamageUpdateModel model)
        {
            var list = new clsValidationResult();

            if (!await clsVehicleDamageData.IsDamageIDExistsAsync(damageID))
            {
                list.Add("DamageID", "معرف الضرر غير صالح");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                list.Add("Description", "الوصف لايمكن أن يكون فارغ");
            }
            else if (model.Description.Length > 500)
            {
                list.Add("Description", "الوصف تجاوز الحد المسموح به");
            }

            if (model.EstimatedCost != null)
            {
                if (model.EstimatedCost <= 0)
                {
                    list.Add("EstimatedCost", "التكلفة المقدرة يجب أن تكون أكبر من صفر");
                }
            }

            return list;
        }
    }
}
