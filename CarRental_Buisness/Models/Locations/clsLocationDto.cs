using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Locations
{
    public class clsLocationDto 
    {

        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; } 
        public bool IsActive { get; set; }
    }
}
