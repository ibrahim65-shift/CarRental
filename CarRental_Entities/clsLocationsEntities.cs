using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsLocationsEntities
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } // Allows null
        public string Phone { get; set; } // allows null
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
