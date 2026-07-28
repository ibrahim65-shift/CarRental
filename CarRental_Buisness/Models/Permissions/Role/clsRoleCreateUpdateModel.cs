using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Permissions.Role
{
    public class clsRoleCreateUpdateModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; } // allows null
        public bool IsActive { get; set; }
    }
}
