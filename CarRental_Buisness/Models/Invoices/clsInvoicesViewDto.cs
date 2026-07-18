using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Invoices
{
    public class clsInvoicesViewDto 
    {
        public int InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public string TypeName { get; set; }
        public int? BookingID { get; set; }
        public int? MaintenanceID { get; set; }
        public int? DamageID { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal BaseAmount { get; set; }

        public decimal AdditionalCharges { get; set; }

        public decimal LateFees { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string CurrencyCode { get; set; }

        public string Notes { get; set; }
       
        public DateTime CreatedDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime? EditedDate { get; set; }

        public int? EditedByUserID { get; set; }
        public int? CustomerID { get; set; }

        public int? VehicleID { get; set; }
    }
}
