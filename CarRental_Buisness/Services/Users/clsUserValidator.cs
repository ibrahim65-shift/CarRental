using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarRental_Buisness.Validators
{
    public class clsUserValidator
    {
        public async Task<clsValidationResult> ValidateAddNewAsync(clsUserAddNewModel model)
        {
            var result = new clsValidationResult();

            if (model.PersonID <= 0)
            {
                result.Add("PersonID", "معرف الشخص غير صحيح");
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                result.Add("UserName", "اسم المستخدم لايمكن أن يكون فارغ");
            }
            else if (model.UserName.Length > 150)
            {
                result.Add("UserName", "اسم المستخدم طويل جدا");
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                result.Add("Password", "كلمة السر لايمكن أن تكون فارغة");
            }
            else if (model.Password.Length > 500)
            {
                result.Add("Password", "كلمة السر طويلة جدا");
            }

            if (await clsUsersData.IsPersonIDExistsAsync(model.PersonID))
            {
                result.Add("PersonID", "المستخدم موجود مسبقا");
            }

            if (await clsUsersData.IsUserNameExistsAsync(model.UserName))
            {
                result.Add("UserName", "اسم المستخدم موجود بالفعل");
            }

            if (!await clsUsersData.IsPersonIDExistsInSystemAsync(model.PersonID))
            {
                result.Add("PersonID", "معرف الشخص غير موجود في النظام");
            }

            if (!await clsUsersData.IsRoleIDExistsInSystemAsync(model.RoleID))
            {
                result.Add("RoleID", "معرف الدور غير موجود في النظام");
            }


            return result;
        }
        public clsValidationResult ValidateUpdateAsync(clsUserUpdateModel model)
        {

            var result = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                result.Add("UserName", "اسم المستخدم لايمكن أن يكون فارغ");
            }
            else if (model.UserName.Length > 150)
            {
                result.Add("UserName", "اسم المستخدم طويل جدا");
            }

           

            return result;
        }
    }
}
