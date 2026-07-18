using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Invoices
{
    public class clsUpdateLinkedInvoiceModel
    {
        public int? EntityID { get; set; }
        public enInvoiceTypes InvoiceType { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal AdditionalCharges { get; set; }
        public decimal LateFees { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string Notes { get; set; }
    }
}
