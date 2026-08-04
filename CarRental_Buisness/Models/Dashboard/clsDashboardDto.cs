using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Dashboard
{
    public class clsDashboardDto
    {
        public clsStatisticsDto Statistics { get; set; }
        public List<clsLatestBookingsDto> LatestBookings { get; set; }
        public List<clsAlertsDto> Alerts { get; set; }
    }
}
