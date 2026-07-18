using Microsoft.Win32;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarRental_Buisness.Helpers
{
    public static class clsUtil
    {
        private static readonly string RegistryKeyPath = @"SoftWare\CarRental";
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("My-Name-Is-Ibrahim-Mohammed-CarRental");
        public static bool RemeberUserNameAndPassword(string userName, string password)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                {
                    if (key == null)
                        return false;

                    key.SetValue("UserName", userName ?? string.Empty);

                    if (!string.IsNullOrEmpty(password))
                    {
                        byte[] encryptedPassword = ProtectedData.Protect
                            (
                               Encoding.UTF8.GetBytes(password), Entropy, DataProtectionScope.CurrentUser
                            );

                        key.SetValue("Password", encryptedPassword, RegistryValueKind.Binary);
                    }
                    else
                    {
                        if (key.GetValue("Password") != null)
                            key.DeleteValue("Password", false);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUtil.RemeberUserNameAndPassword (General)", ex);
                return false;
            }
        }
        public static bool GetStoredCredential(ref string userName, ref string password)
        {
            userName = null; password = null;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    if (key == null)
                        return false;


                    object userNameValue = key.GetValue("UserName");
                    if (userNameValue != null)
                        userName = userNameValue.ToString();


                    object passwordValue = key.GetValue("Password");

                    if (passwordValue is byte[] encryptedPassword)
                    {
                        byte[] decryptedPassword = ProtectedData.Unprotect
                            (
                               encryptedPassword, Entropy, DataProtectionScope.CurrentUser
                            );

                        password = Encoding.UTF8.GetString(decryptedPassword);
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUtil.GetStoredCredential (General)", ex);
                return false;
            }
        }
        public static void DeleteCredentialsFromRegistry()
        {
            try
            {
                using (var root = Registry.CurrentUser)
                {
                    using (var sub = root.OpenSubKey(RegistryKeyPath))
                    {
                        if (sub != null)
                        {
                            sub.DeleteSubKey(RegistryKeyPath, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUtil.DeleteCredentialsFromRegistry (General)", ex);
            }
        }
        public static async Task<bool> CheckDatabaseConnection()
        {
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarRentalDB"].ConnectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (var command = new SqlCommand("Select 1", connection))
                    {
                        var result = await command.ExecuteScalarAsync().ConfigureAwait(false);
                        return result?.ToString() == "1";
                    }
                }

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("clsUtil.CheckDatabaseConnection (General)", ex);
                return false;
            }
        }
        public static string FormatDate(object dateValue)
        {
            if (dateValue == DBNull.Value || dateValue == null)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out var date))
                return date.ToString("dd/MM/yyyy");

            return string.Empty;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {

                var addr = new MailAddress(email);
                if (addr.Address != email)
                    return false;

                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                                           RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (!emailRegex.IsMatch(email))
                    return false;

                string domain = email.Substring(email.IndexOf('@') + 1);
                try
                {
                    var hostEntry = Dns.GetHostEntry(domain);
                    if (hostEntry.AddressList.Length == 0)
                        return false;
                }
                catch (SocketException)
                {
                    return false; 
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string NullIfEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

    }
}
