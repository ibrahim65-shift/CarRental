using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities.Dashboard
{
    public class clsLatestBookingsEntity
    {
        public int BookingID { get; set; }
        public string Customer { get; set; }
        public string Vehicle { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public string StatusName { get; set; }
    }
}
