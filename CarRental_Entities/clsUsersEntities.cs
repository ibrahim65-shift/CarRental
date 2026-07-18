using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsUsersEntities
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLockedOut { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastFailedLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }

        public clsPersonEntities Person {  get; set; } // Composition
    }
}
