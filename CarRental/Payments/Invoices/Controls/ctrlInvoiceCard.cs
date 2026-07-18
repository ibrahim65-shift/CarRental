using CarRental.Helper;
using CarRental.Properties;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_Buisness.Services.People;
using CarRental_Buisness.Services.Users;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Payments.Invoices.Controls
{
    public partial class ctrlInvoicesCard : UserControl
    {
        private const string Unkown = "غير معروف";

        private clsInvoicesDto _Invoice;
       
        public ctrlInvoicesCard()
        {
            InitializeComponent();
        }

        public clsInvoicesDto Invoices =>_Invoice;
        public async Task<bool> LoadAsync(Func<Task<clsServiceResult<clsInvoicesDto>>> loader)
        {
            try
            {
                var result = await loader();

                if(!result.Success || result.Data==null)
                {
                    ResetInvoicesInfo();
                    return false;  
                }

                _FillInvoicesInfo(result.Data);
                return true;
            }
            catch
            {
                ResetInvoicesInfo();
                return false;
            }
        }
        private void _FillInvoicesInfo(clsInvoicesDto Invoice)
        {
            _Invoice = Invoice;
            lblInvoiceID.Text    = Invoice.InvoiceID.ToString();
            lblInvoiceNumber.Text  = Invoice.InvoiceNumber;
            lblInvoiceType.Text   = SetInvoiceType(Invoice.InvoiceTypeID);
            lblInvoiceDate.Text  = Invoice.InvoiceDate.ToString("dd/MM/yyyy");
            lblBaseAmount.Text   = Invoice.BaseAmount.ToString();
            lblAdditionalCharges.Text    = Invoice.AdditionalCharges.ToString();
            lblLateFees.Text   = Invoice.LateFees.ToString();
            lblTaxAmount.Text = Invoice.TaxAmount.ToString();
            lblDiscount.Text      = Invoice.DiscountAmount.ToString() ;
            lblTotalAmount.Text = Invoice.TotalAmount.ToString();
            lblNotes.Text       = Invoice.Notes ?? "لاتوجد ملاحظات";
        }
        public void ResetInvoicesInfo()
        {
            lblInvoiceID.Text   = "????";
            lblInvoiceNumber.Text  = "????";
            lblInvoiceType.Text = "????";
            lblInvoiceDate.Text  = "????";
            lblBaseAmount.Text   = "????";
            lblAdditionalCharges.Text = "????";
            lblLateFees.Text  = "????";
            lblTaxAmount.Text     = "????";
            lblDiscount.Text      = "????";
            lblTotalAmount.Text      = "????";
        }

        private string SetInvoiceType(enInvoiceTypes type)
        {
            switch (type)
            {
                case enInvoiceTypes.Booking:
                    return "حجز";

                case enInvoiceTypes.Maintenance:
                    return "صيانة";

                case enInvoiceTypes.VehicleDamage:
                    return "ضرر المركبة";


                default: return Unkown;
            }
        }
    }
}
