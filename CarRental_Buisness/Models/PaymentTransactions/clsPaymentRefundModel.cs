using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.PaymentTransactions
{
    public class clsPaymentRefundModel
    {
        public int PaymentID { get; set; }
        public enPaymentMethod PaymentMethod { get; set; }
        public decimal RefundAmount { get; set; } 
        public string Reference {  get; set; }
    }
}
