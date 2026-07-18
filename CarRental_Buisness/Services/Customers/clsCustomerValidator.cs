using CarRental_Buisness.Models.Customers;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarRental_Buisness.Services.Customers
{
    public class clsCustomerValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsCustomerAddNewModel model)
        {
            var list = new clsValidationResult();

            if (model.PersonID <= 0)
            {
                list.Add("PersonID", "معرف الشخص غير صالح");
            }

            if (string.IsNullOrWhiteSpace(model.DriverLicenseNumber))
            {
                list.Add("DriverLicenseNumber", "رقم الرخصة لايمكن أن يكون فارغ");
            }
            else if (model.DriverLicenseNumber.Length > 50)
            {
                list.Add("DriverLicenseNumber", "رقم الرخصة تجاوز الحد المسموح به");
            }

            if (model.DriverLicenseExpiry == default)
            {
                list.Add("DriverLicenseExpiry", "تاريخ انتهاء الرخصة غير صالح");
            }

            if (await clsCustomersData.IsLicenseExpiryAsync(model.DriverLicenseExpiry.Date))
            {
                list.Add("DriverLicenseExpiry", "الرخصة منتهية أو باقي شهر أو أقل على انتهائها");
            }

            if (!await clsPersonData.PersonExists(model.PersonID))
            {
                list.Add("PersonID", "لايوجد سجل مرتبط بهذا المعرف");
            }

            if (await clsCustomersData.IsLicenseNumberDuplicatedAsync(model.DriverLicenseNumber))
            {
                list.Add("DriverLicenseNumber", "الرخصة موجودة مسبقا");
            }

            if (await clsCustomersData.IsPersonIdDuplicatedAsync(model.PersonID))
            {
                list.Add("PersonID", "العميل موجود مسبقا");
            }

            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(clsCustomerUpdateModel model)
        {
            var list = new clsValidationResult();

            if (model.DriverLicenseExpiry == default)
            {
                list.Add("DriverLicenseExpiry", "تاريخ انتهاء الرخصة غير صالح");
            }

            if (await clsCustomersData.IsLicenseExpiryAsync(model.DriverLicenseExpiry.Date))
            {
                list.Add("DriverLicenseExpiry", "الرخصة منتهية أو باقي شهر أو أقل على انتهائها");
            }

            return list;
        }
    }
}
