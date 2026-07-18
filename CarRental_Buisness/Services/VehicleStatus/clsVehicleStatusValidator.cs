
using CarRental_Buisness.Models.VehicleStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleStatus
{
    public class clsVehicleStatusValidator
    {
        public async Task<clsValidationResult> ValidteVehicleStatusAsync(int? statusID, clsVehicleStatusCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.StatusName))
            {
                list.Add("StatusName", "الحالة لايمكن أن تكون فارغة");
            }
            else if (model.StatusName.Length > 100)
            {
                list.Add("StatusName", "الحالة تجاوزت الحد المسموح به");
            }
            else
            {
                if (await clsVehicleStatusData.IsStatusNameExistsAsync(statusID, model.StatusName))
                {
                    list.Add("StatusName", "الحالة موجودة بالفعل");
                }
            }

            return list;
        }

    }
}
