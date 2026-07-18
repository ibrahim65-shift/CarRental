using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Payments.Invoices.DTOs
{
    public class clsPaymentInfo
    {
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
