using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Permissions.RolePermission;
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

namespace CarRental_Buisness.Services.Permissions.RolePermission
{
    public class clsRolePermissionService
    {
        public async Task<clsServiceResult<clsRolePermissionDto>> GetRolePermissionsByIDAsync(int rolePermissionId)
        {
            var entity = await clsRolePermissionData.GetRolePermissionsByIdAsync(rolePermissionId);
            if (entity == null)
                return clsServiceResult<clsRolePermissionDto>.Fail("لاتوجد صلاحيات");

            return clsServiceResult<clsRolePermissionDto>.OK(clsRolePermissionMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<List<clsRolePermissionViewDto>>> GetPermissionsForRoleAsync(int roleId)
        {
            var result = await clsRolePermissionData.GetPermissionsForRoleAsync(roleId);
            if (result == null || result.Count == 0)
                return clsServiceResult<List<clsRolePermissionViewDto>>.Fail("لاتوجد صلاحيات");

            var list = result.Select(clsRolePermissionMapper.ToViewDto).ToList();

            return clsServiceResult<List<clsRolePermissionViewDto>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetRolePermissionsPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var (dt, totalPages) = await clsRolePermissionData.GetRolePermissionsPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var result = new clsPagedResult<DataTable>
            {
                Data = dt,
                TotalPages = totalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(result);
        }
        public async Task<clsServiceResult<bool>> SaveRolePermissionsAsync(int roleId, List<clsRolePermissionItem> permissions)
        {
            bool saved = await clsRolePermissionData.SaveRolePermissionsAsync(roleId, permissions);
            return saved ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حفظ الصلاحيات");
        }
        public async Task<clsServiceResult<bool>> HasPermissionAsync(int roleId, int permissionId)
        {
            bool has = await clsRolePermissionData.HasPermissionAsync(roleId, permissionId);
            return has ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("ليس لديه صلاحية");
        }
        public async Task<clsServiceResult<bool>> HasPermissionAsync(int roleId, string permissionCode)
        {
            bool has = await clsRolePermissionData.HasPermissionAsync(roleId, permissionCode);
            return has ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("ليس لديه صلاحية");
        }
    }
}
