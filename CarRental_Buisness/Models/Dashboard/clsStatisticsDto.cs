using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Dashboard
{
    public class clsStatisticsDto
    {
        public int CustomersCount { get; set; }
        public int VehiclesCount { get; set; }
        public int ActiveBookings { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int UsersCount { get; set; }
        public int AvailableVehicles { get; set; }
        public int RentedVehicles { get; set; }
        public int MaintenanceVehicles { get; set; }
    }
}
