using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public class clsDbSessionContext
    {
        public int UserID {  get; set; }
        public string Source { get; set; } = "UI";
        public string MachineName { get; set; }
        public string IPAddress { get; set; }

    }
}
