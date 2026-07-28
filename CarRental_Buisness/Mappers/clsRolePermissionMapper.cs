using CarRental_Buisness.Models.Permissions.RolePermission;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsRolePermissionMapper
    {
        public static clsRolePermissionViewDto ToViewDto(clsRolePermissionEntityView entity)
        {
            return new clsRolePermissionViewDto
            {
                RolePermissionID = entity.RolePermissionID,
                PermissionCode = entity.PermissionCode,
                PermissionName = entity.PermissionName,
                IsAllowed = entity.IsAllowed,
                PermissionID = entity.PermissionID,
            };
        }
        public static clsRolePermissionDto ToDto(clsRolePermissionEntity entity)
        {
            return new clsRolePermissionDto
            {
                RolePermissionID = entity.RolePermissionID,
                RoleID = entity.RoleID,
                PermissionID = entity.PermissionID,
                IsAllowed = entity.IsAllowed,
            };
        }
    }
}
