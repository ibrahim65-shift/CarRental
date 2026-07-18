using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness
{
    public static class clsCurrentUser
    {
        public static clsUserDto User { get;private set; } 
        public static bool IsLoggedIn => User != null;

        public static int UserID => User?.UserID ?? 1;

        public static void Set(clsUserDto user)
        {
            User = user;
        }

        public static void Clear()
        {
            User = null;
        }
    }
}
