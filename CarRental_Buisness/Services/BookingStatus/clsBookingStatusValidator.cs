using CarRental_Buisness.Models.BookingStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.BookingStatus
{
    public class clsBookingStatusValidator
    {
        public async Task<clsValidationResult> ValidateCreateUpdateAsync(int? BookingStatusID, clsBookingStatusCreateUpdateModel model)
        {
            var list = new  clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.StatusName))
            {
                list.Add("StatusName", "نوع الحالة لايمكن أن تكون فارغة");
            }
            else if (model.StatusName.Length > 50)
            {
                list.Add("StatusName", "نوع الحالة تجاوزت الحد المسموح به");
            }
            else
            {
                if(await clsBookingStatusData.IsBookingStatusExistsByNameAsync(BookingStatusID,model.StatusName))
                {
                    list.Add("StatusName", "نوع الحالة موجودة بالفعل");
                }
            }

            if(!string.IsNullOrWhiteSpace(model.Description))
            {
                if(model.Description.Length > 200)
                {
                    list.Add("Description", "الوصف تجاوز الحد المسموح به");
                }
            }

            return list;
        }
    }
}
