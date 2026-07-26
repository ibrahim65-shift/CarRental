using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Permissions.Permission;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.Permissions.Permission
{
    public class clsPermissionService
    {
        public async Task<clsServiceResult<clsPermissionDto>> GetByIdAsync(int id)
        {
            var entity = await clsPermissionData.GetByIDAsync(id);
            if (entity == null)
                return clsServiceResult<clsPermissionDto>.Fail("لاتوجد صلاحيات");

            return clsServiceResult<clsPermissionDto>.OK(clsPermissionMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsPermissionDto>>> GetAllAsync()
        {
            var result = await clsPermissionData.GetAllAsync();
            if (result == null || result.Count ==0)
                return clsServiceResult<List<clsPermissionDto>>.Fail("لاتوجد صلاحيات");

            var list = result.Select(clsPermissionMapper.ToDto).ToList();

            return clsServiceResult<List<clsPermissionDto>>.OK(list);
        }
        public async Task<clsServiceResult<bool>> IsExistsAsync(int permissionId)
        {
            bool exists = await clsPermissionData.IsExistsAsync(permissionId);
            return exists ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("لاتوجد صلاحية");
        }

    }
}
