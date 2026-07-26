using CarRental_Buisness.Models.Permissions.Role;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Permissions.Role
{
    public class clsRoleValidator
    {
      
        public async Task<clsValidationResult> ValidationAsync(int? roleId , clsRoleCreateUpdateModel model)
        {
            var list = new clsValidationResult();

            if (string.IsNullOrWhiteSpace(model.RoleName))
                list.Add("RoleName", "اسم الدور لايمكن أن يكون فارغ");
            else if (model.RoleName.Length > 150)
                list.Add("RoleName", "اسم الدور تجاوز الحد المسموح به");

            if (!string.IsNullOrWhiteSpace(model.Description) && model.Description.Length > 200)
                list.Add("Description", "الوصف تجاوز الحد المسموح به");

            if(roleId.HasValue)
            {
                if (await clsRoleData.IsRoleNameExistsExceptAsync(roleId.Value, model.RoleName))
                    list.Add("RoleName", "اسم الدور مستخدم بالفعل");
            }
            else
            {
                if (await clsRoleData.IsRoleNameExistsAsync(model.RoleName))
                    list.Add("RoleName", "اسم الدور مستخدم بالفعل");
            }

            return list;
        }
    }
}
