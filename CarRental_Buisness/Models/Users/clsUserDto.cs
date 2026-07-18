using CarRental_Buisness.Models.People;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Users
{
    public class clsUserDto
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
        public int PersonID { get; set; }


        public clsPersonDto PersonInfo { get; set; } // Composition
    }
}
