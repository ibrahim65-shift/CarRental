using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Helpers
{
    public static class clsConnectionSecurity
    {

        private static readonly byte[] _entropy = Encoding.UTF8.GetBytes("CarRental_ConnectionSettings_v1");

        public static string Encrypt(string plainText)
        {

            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] encryptedBytes = ProtectedData.Protect(plainBytes,_entropy,DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedBytes);
        }
        public static string Decrypt(string cipherText)
        {

            if (string.IsNullOrWhiteSpace(cipherText))
                return string.Empty;

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes,_entropy, DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("البيانات المشفرة ليست بصيغة صحيحة.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("تعذر فك تشفير البيانات. ربما تم تشفيرها بواسطة مستخدم Windows آخر أو أن البيانات تالفة.", ex);
            }

        }
    }
}
