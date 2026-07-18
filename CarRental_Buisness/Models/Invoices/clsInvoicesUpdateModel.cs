using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Invoices
{
    public class clsInvoicesUpdateModel
    {
        public decimal AdditionalCharges { get; set; }
        public decimal LateFees { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Notes { get; set; }
    }
}
