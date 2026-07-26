using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public class clsRolePermissionItem
    {
        public int PermissionID { get; set; }
        public bool IsAllowed { get; set; }

        public clsRolePermissionItem(int permID, bool isAllowed)
        {
            PermissionID = permID;
            IsAllowed = isAllowed;
        }
    }
}
