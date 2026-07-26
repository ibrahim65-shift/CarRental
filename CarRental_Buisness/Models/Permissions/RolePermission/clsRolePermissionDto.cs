using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Permissions.RolePermission
{
    public class clsRolePermissionDto
    {
        public int RolePermissionID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public bool IsAllowed { get; set; }
    }
}
