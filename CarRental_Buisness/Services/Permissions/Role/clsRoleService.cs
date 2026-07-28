using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Permissions.Role;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Permissions.Role
{
    public class clsRoleService
    {
        private readonly clsRoleValidator _validator = new clsRoleValidator();
        public async Task<clsServiceResult<clsRoleDto>> GetByIDAsync(int roleId)
        {
            var entity = await clsRoleData.GetByIDAsync(roleId);
            if (entity == null)
                return clsServiceResult<clsRoleDto>.Fail("لاتوجد بيانات");

            return clsServiceResult<clsRoleDto>.OK(clsRoleMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsRoleDto>> AddNewAsync(clsRoleCreateUpdateModel model)
        {
            var validation = await _validator.ValidationAsync(null, model);
            if (!validation.IsValid)
                return clsServiceResult<clsRoleDto>.Invalid(validation);

            var entity = new clsRoleEntity
            {
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive,
            };

            var newId = await clsRoleData.AddNewAsync(entity);
            if (newId == null)
                return clsServiceResult<clsRoleDto>.Fail("فشل إضافة دور جديد");

            entity.RoleID = newId.Value;
            return clsServiceResult<clsRoleDto>.OK(clsRoleMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int roleId, clsRoleCreateUpdateModel model)
        {
            var entity = await clsRoleData.GetByIDAsync(roleId);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الدور غير موجود");

            var validation = await _validator.ValidationAsync(roleId, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.RoleName = model.RoleName;
            entity.Description = model.Description;
            entity.IsActive = model.IsActive;

            bool updated = await clsRoleData.UpdateAsync(roleId, entity);
            return updated ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الدور");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int roleId)
        {
            bool deleted = await clsRoleData.DeleteAsync(roleId);

            if (!await clsRoleData.IsExistsAsync(roleId))
                return clsServiceResult<bool>.Fail("الدور غير موجود");

            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الدور");
        }
        public async Task<clsServiceResult<List<clsRoleDto>>> GetAllAsync()
        {
            var result = await clsRoleData.GetAllAsync();
            if (result == null || result.Count == 0)
                return clsServiceResult<List<clsRoleDto>>.Fail("لاتوجد بيانات");

            var list = result.Select(clsRoleMapper.ToDto).ToList();

            return clsServiceResult<List<clsRoleDto>>.OK(list);
        }
        public async Task<clsServiceResult<List<clsRoleDto>>> GetActiveRolesAsync()
        {
            var result = await clsRoleData.GetActiveRolesAsync();
            if (result == null || result.Count == 0)
                return clsServiceResult<List<clsRoleDto>>.Fail("لاتوجد بيانات");

            var list = result.Select(clsRoleMapper.ToDto).ToList();

            return clsServiceResult<List<clsRoleDto>>.OK(list);
        }
        public async Task<clsServiceResult<bool>> IsExistsAsync(int roleId)
        {
            bool exists = await clsRoleData.IsExistsAsync(roleId);
            return exists ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("الدور غير موجود");
        }
    }
}
