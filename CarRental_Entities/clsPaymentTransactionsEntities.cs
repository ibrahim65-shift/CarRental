using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Entities
{
    public class clsPaymentTransactionsEntities
    {
        public int PaymentID { get; set; }
        public int InvoiceID { get; set; }
        public enPaymentMethod PaymentMethodID { get; set; }
        public enPaymentStatus PaymentStatusID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }
        public string Reference { get; set; } // allows null
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime? EditedDate { get; set; }
        public int? EditedByUserID { get; set; }
    }
}
