using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities.Dashboard
{
    public class clsAlertsEntity
    {
        public enAlertType AlertType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public enSeverityType Severity { get; set; }
        public int ReferenceID { get; set; }
    }
}
