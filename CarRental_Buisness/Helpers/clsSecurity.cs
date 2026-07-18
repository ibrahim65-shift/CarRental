using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness
{
    public class clsSecurity
    {
        private const int SaltSize = 32;
        private const int HahsSize = 32;
        private const char Separator = ':';
        public static string GeneratSalt(int size=SaltSize)
        {
            byte[] saltBytes = new byte[size];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password , string salt)
        {
            if(password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentNullException("salt is required",nameof (salt));

            using(SHA256 sha256 = SHA256.Create())
            {
                byte[]data = Encoding.UTF8.GetBytes(password+salt);
                byte[] hash = sha256.ComputeHash(data); 
                return Convert.ToBase64String(hash);
            }
        }
        public static string Pack(string salt, string hash)
        {
            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentNullException("Salt is required", nameof(salt));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException("Hash is required", nameof(hash));

            return $"{salt}{Separator}{hash}";

        }
        public static bool TryUnPack(string packed , out string salt , out string hash)
        {
            salt = null ;
            hash = null ;

            if(string.IsNullOrWhiteSpace(packed))
                return false ;

            int idx = packed.IndexOf(Separator) ;
            if (idx <= 0 || idx == packed.Length - 1)
                return false;

          
            salt= packed.Substring(0, idx) ;
            hash = packed.Substring(idx + 1);
           
            return IsValidBase64(salt) && IsValidBase64(hash) ;
        }
        public static bool Verfiy(string password, string packed)
        {
            if(!TryUnPack(packed,out string salt , out string storedHash))
                return false;

            var computed = HashPassword(password, salt);
            return string.Equals(computed,storedHash, StringComparison.Ordinal);
        }
        private static bool IsValidBase64(string value)
        {
            try
            {
                Convert.FromBase64String(value);
                return true ;
            }
            catch { return false ; }
        }
        public static string EncryptedPassword(string password)
        {
            string salt = GeneratSalt();
            string hash = HashPassword(password, salt);
            return Pack(salt, hash);
        }
    }
}
