using CarRental_Buisness.Models.Permissions.Permission;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public static class clsPermissionMapper
    {
        public static clsPermissionDto ToDto(clsPermissionEntity entity)
        {
            return new clsPermissionDto
            {
                PermissionID = entity.PermissionID,
                PermissionCode = entity.PermissionCode,
                PermissionName = entity.PermissionName
            };
        }
    }
}
