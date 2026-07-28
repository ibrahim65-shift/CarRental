using System;
using System.Collections.Generic;

namespace CarRental_Buisness.Helpers
{
    public static class clsAuthorizationCache
    {
        private static HashSet<string> _permissionCache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static void LoadPermissions(IEnumerable<string> permissions)
        {
            _permissionCache = new HashSet<string>(permissions ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        }
        public static bool HasPermission(string permissionCode)
        {
            if (string.IsNullOrWhiteSpace(permissionCode))
                return false;


            return  _permissionCache.Contains(permissionCode);
        }
        public static void Clear()
        {
            _permissionCache.Clear();
        }
    }
}
