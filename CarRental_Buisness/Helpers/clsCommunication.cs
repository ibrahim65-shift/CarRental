using SharedClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Helpers
{
    public static class clsCommunication
    {
        public static bool SendEmail(string email , string subject = "" , string body="")
        {
			try
			{
                subject = Uri.EscapeDataString(subject ?? string.Empty);
                body = Uri.EscapeDataString(body ?? string.Empty);

                Process.Start(new ProcessStartInfo
                {
                    FileName = $"mailto:{email}?subject={subject}&body={body}",
                    UseShellExecute = true
                });

                return true;
			}
			catch (Exception ex)
			{
                clsEventLogger.LogException("clsCommunication.SendEmail", ex);
                return false;
			}
        }
    }
}
