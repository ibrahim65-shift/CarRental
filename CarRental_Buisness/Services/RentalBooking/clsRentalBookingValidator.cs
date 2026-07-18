using CarRental_Buisness.Models.RentalBooking;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.RentalBooking
{
    public class clsRentalBookingValidator
    {
        private async Task ValidateCommon(clsValidationResult list , int VehicleID , DateTime RentalStartDate , DateTime RentalEndDate
            , int PickupLocationID  , int DropOffLocationID , string InitialCheckNotes)
        {
            if(!await clsVehiclesData.IsVehicleExistsAsync(VehicleID))
            {
                list.Add("VehicleID", "معرف المركبة غير صالح");
            }

            if (RentalStartDate.Date < DateTime.Today)
            {
                list.Add("RentalStartDate", "يرجى إدخال تاريخ بداية صحيح");
            }

            if (RentalEndDate.Date == RentalStartDate.Date)
            {
                list.Add("RentalEndDate", "تاريخ نهاية الإيجار لايمكن أن يكون في نفس تاريخ بداية الإيجار");
            }

            if (RentalEndDate.Date < RentalStartDate.Date)
            {
                list.Add("RentalEndDate", "لايمكن لتاريخ نهاية الإيجار أن يكون أصغر من تاريخ بداية الإيجار");
            }

            if(!await clsLocationsData.IsLocationIDExistsAsync(PickupLocationID))
            {
                list.Add("PickupLocationID", "معرف موقع الاستلام غير صالح");
            }

            if(!await clsLocationsData.IsLocationIDExistsAsync(DropOffLocationID))
            {
                list.Add("DropOffLocationID", "معرف موقع التسليم غير صالح");
            }

            if(!string.IsNullOrWhiteSpace(InitialCheckNotes))
            {
                if(InitialCheckNotes.Length > 500)
                {
                    list.Add("InitialCheckNotes", "ملاحظات الفحص تجاوزت الحد المسموح به");
                }
            }

        }

        public async Task<clsValidationResult> ValidateAddNewAsync(clsRentalBookingAddNewModel model , DateTime driverLicenseExpiry)
        {
            var list = new clsValidationResult();

            await ValidateCommon(list, model.VehicleID, model.RentalStartDate, model.RentalEndDate, model.PickupLocationID,
                model.DropOffLocationID, model.InitialCheckNotes);

            if(!await clsCustomersData.IsCustomerExistsAsync(model.CustomerID))
            {
                list.Add("CustomerID", "معرف العميل غير صالح");
            }

            if (await clsCustomersData.IsLicenseExpiryAsync(driverLicenseExpiry.Date))
            {
                list.Add("DriverLicenseExpiry", "الرخصة منتهية أو باقي شهر أو أقل على انتهائها");
            }

            if (await clsVehiclesData.IsVehicleExistsAsync(model.VehicleID))
            {
                if (await clsRentalBookingData.OverLapsAsync(null, model.VehicleID, model.RentalStartDate, model.RentalEndDate))
                {
                    list.Add("VehicleID", "هذه السيارة مستأجرة");
                }
            }

           

            return list;
        }

        public async Task<clsValidationResult> ValidateUpdateAsync(int BookingID ,clsRentalBookingUpdateModel model)
        {
            var list = new clsValidationResult();

            await ValidateCommon(list, model.VehicleID, model.RentalStartDate, model.RentalEndDate, model.PickupLocationID,
                model.DropOffLocationID, model.InitialCheckNotes);

            if (await clsVehiclesData.IsVehicleExistsAsync(model.VehicleID))
            {
                if (await clsRentalBookingData.OverLapsAsync(BookingID, model.VehicleID, model.RentalStartDate, model.RentalEndDate))
                {
                    list.Add("VehicleID", "هذه السيارة مستأجرة");
                }
            }

            return list;
        }


    }
}
