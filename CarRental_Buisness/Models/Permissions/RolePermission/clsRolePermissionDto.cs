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
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public bool IsAllowed { get; set; }
    }
}
