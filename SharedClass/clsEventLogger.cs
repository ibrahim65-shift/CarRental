using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public static class clsEventLogger
    {
        private const string Source = "CarRental";
        private const string LogName = "Application";

        static clsEventLogger()
        {
            try
            {
                if(!EventLog.SourceExists(Source))
                {
                    EventLog.CreateEventSource(Source,LogName);
                }
            }
            catch { }
        }

        public static void LogException(string location,Exception ex)
        {
            try
            {
                string message =
                    $"Exception Location: {location}\n\n" +
                    $"Time: {DateTime.Now}\n\n" +
                    $"Message: {ex.Message}\n\n" +
                    $"Stack Trace:\n{ex.StackTrace}";

                EventLog.WriteEntry(Source, message, EventLogEntryType.Error);
            }
            catch { }
        }
    }
}
