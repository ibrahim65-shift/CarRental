using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.PaymentTransactions
{
    public class clsPaymentTransactionsAddNewModel
    {
        public int InvoiceID { get; set; }
        public enPaymentMethod PaymentMethodID { get; set; }
        public decimal PaidAmount { get; set; }
        public string Reference { get; set; } // allows null
    }
}
