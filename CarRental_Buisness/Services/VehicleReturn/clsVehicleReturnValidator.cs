using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleReturn
{
    public class clsVehicleReturnValidator
    {
        public async Task<clsValidationResult> ValidateStartInspectionAsync(int bookingId , DateTime ActualReturnDate)
        {
            var list = new clsValidationResult();

            if(await clsVehicleReturnData.IsVehicleReturnExistsByBookingIDAsync(bookingId))
            {
                list.Add("BookingID", "هذا الحجز موجود بالفعل");
            }

            if(ActualReturnDate < DateTime.Today)
            {
                list.Add("ActualReturnDate", "تاريخ الإرجاع الفعلي لايمكن أن يكون أصغر من تاريخ اليوم");
            }

            return list;
        }

        public clsValidationResult ValidateUpdateInspectionAsync(int? startMileage,clsUpdateInspectionModel model)
        {
            var list = new clsValidationResult();

            if (startMileage == null)
            {
                list.Add("MileageStart", "بداية الأميال غير متوفرة");
                return list;
            }

            if (!string.IsNullOrWhiteSpace(model.FinalCheckNotes))
            {
                if(model.FinalCheckNotes.Length > 500)
                {
                    list.Add("FinalCheckNotes", "الملاحظات النهائية تجاوزت الحد المسموح به");
                }
            }

            if(model.MileageEnd !=null)
            {
                if(model.MileageEnd < startMileage)
                {
                    list.Add("MileageEnd", "الأميال النهائية لايمكن أن تكون أصغر من بداية الأميال");
                }
            }

            if(model.AdditionalCharges != null)
            {
                if(model.AdditionalCharges<0)
                {
                    list.Add("AdditionalCharges", "الرسوم الإضافية يجب أن تكون أكبر من صفر");
                }
            }

            return list;
        }

        public clsValidationResult ValidateFinalizeAsync(int? startMileage , int? mileageEnd)
        {
            var list = new clsValidationResult();

            if (startMileage == null)
            {
                list.Add("MileageStart", "بداية الأميال غير متوفرة");
                return list;
            }

            if(mileageEnd==null)
            {
                list.Add("MileageEnd", "يرجى إدخال الأميال النهائية");
            }
            else
            {
                if (mileageEnd < startMileage)
                {
                    list.Add("MileageEnd", "الأميال النهائية لايمكن أن تكون أصغر من بداية الأميال");
                }
            }

            return list;
        }


    }
}
