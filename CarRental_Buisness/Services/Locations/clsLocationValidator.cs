using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Locations
{
    public class clsLocationValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsLocationAddNewModel model)
        {
            var list = new clsValidationResult();

            if(string.IsNullOrWhiteSpace(model.Name))
            {
                list.Add("Name", "اسم الفرع لايمكن أن يكون فارغ");
            }
            else if(model.Name.Length >200)
            {
                list.Add("Name", "اسم الفرع تجاوز الحد المسموح به");
            }

            if (!string.IsNullOrWhiteSpace(model.Address))
            {
                if (model.Address.Length > 400)
                {
                    list.Add("Address", "العنوان تجاوز الحد المسموح به");
                }

                if (await clsLocationsData.IsAddressExistsAsync(null,model.Address))
                {
                    list.Add("Address", "العنوان موجود بالفعل");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                if (model.Phone.Length > 50)
                {
                    list.Add("Phone", "رقم الهاتف تجاوز الحد المسموح به");
                }

                if (await clsLocationsData.IsPhoneExistsAsync(null,model.Phone))
                {
                    list.Add("Phone", "رقم الهاتف مستخدم بالفعل");
                }
            }


            return list;
        }
        public async Task<clsValidationResult> ValidateUpdateAsync(int locationID,clsLocationUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                list.Add("Name", "اسم الفرع لايمكن أن يكون فارغ");
            }
            else if (model.Name.Length > 200)
            {
                list.Add("Name", "اسم الفرع تجاوز الحد المسموح به");
            }

            if (!string.IsNullOrWhiteSpace(model.Address))
            {
                if (model.Address.Length > 400)
                {
                    list.Add("Address", "العنوان تجاوز الحد المسموح به");
                }

                if (await clsLocationsData.IsAddressExistsAsync(locationID,model.Address))
                {
                    list.Add("Address", "العنوان موجود بالفعل");
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Phone))
            {
                if (model.Phone.Length > 50)
                {
                    list.Add("Phone", "رقم الهاتف تجاوز الحد المسموح به");
                }

                if (await clsLocationsData.IsPhoneExistsAsync(locationID,model.Phone))
                {
                    list.Add("Phone", "رقم الهاتف مستخدم بالفعل");
                }
            }


            return list;
        }
    }
}
