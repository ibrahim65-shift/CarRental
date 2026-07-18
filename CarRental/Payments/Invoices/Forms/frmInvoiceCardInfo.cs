using CarRental_Buisness.Services.Invoices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Payments.Invoices.Forms
{
    public partial class frmInvoiceCardInfo : Form
    {
        private int? _InvoiceID;
        private int? _bookingId;
        public frmInvoiceCardInfo(int? invoiceId , int? bookingId)
        {
            InitializeComponent();
            _InvoiceID = invoiceId;
            _bookingId = bookingId;
        }

        private async void frmInvoiceCardInfo_Load(object sender, EventArgs e)
        {
            await _LoadInvoiceDataAsync();
        }

        private async Task _LoadInvoiceDataAsync()
        {
            if(_InvoiceID.HasValue)
                await ctrlInvoicesCard1.LoadAsync(() => new clsInvoicesService().GetInvoiceByIDAsync(_InvoiceID.Value));

            if(_bookingId.HasValue)
                await ctrlInvoicesCard1.LoadAsync(() => new clsInvoicesService().GetInvoiceByBookingIDAsync(_bookingId.Value));
        }
    }
}
