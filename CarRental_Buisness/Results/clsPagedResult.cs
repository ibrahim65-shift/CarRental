using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Results
{
    public class clsPagedResult<T>
    {
        public T Data { get; set; }
        public int TotalPages { get; set; }
    }
}
