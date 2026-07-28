using CarRental_Buisness.Models.Permissions.Role;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsRoleMapper
    {
        public static clsRoleDto ToDto(clsRoleEntity entity)
        {
            return new clsRoleDto
            {
                RoleID = entity.RoleID,
                RoleName = entity.RoleName,
                Description = entity.Description,
                IsActive = entity.IsActive,
            };
        }
    }
}
