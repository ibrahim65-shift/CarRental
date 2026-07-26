using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsRolePermissionEntity
    {
        public int RolePermissionID { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public bool IsAllowed { get; set; }
    }
}
