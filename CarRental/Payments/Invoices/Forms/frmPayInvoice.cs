using CarRental.Helper;
using CarRental.Payments.Invoices.DTOs;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.PaymentTransactions;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.PaymentTransactions;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Payments.Invoices.Forms
{
    public partial class frmPayInvoice : Form
    {
        private clsPaymentTransactionsService _paymentTransactionService;
        private clsPaymentInfo _paymentInfo;
        private Dictionary<string, Control> _validationControls;

        private bool _isSaving;
        public frmPayInvoice(clsPaymentInfo paymentInfo)
        {
            InitializeComponent();
            _paymentTransactionService = new clsPaymentTransactionsService();
            _paymentInfo=paymentInfo;

            _validationControls = new Dictionary<string, Control>
            {
                {"InvoiceID" , lblInvoiceNumber },
                {"PaymentMethodID" , cbPaymentMethod },
                {"PaidAmount" , lblTotalAmount },
                {"Reference" , txtReference },
            };
        }

        private void frmPayInvoice_Load(object sender, EventArgs e)
        {
            _InitializeForm();
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _SaveAddNewAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmPayInvoice.btnSave_Click", ex);
                clsMessages.ShowError();
            }
            finally
            {
                _isSaving = false;
                btnSave.Enabled = true;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        // ===================== METHODES ===============

        private void _InitializeForm()
        {
            lblInvoiceNumber.Text = _paymentInfo.InvoiceNumber;
            lblTotalAmount.Text = clsUiHelper.ToSAR(_paymentInfo.TotalAmount);
            clsUiHelper.FillComboBoxWithEnumDescriptions<enPaymentMethod>(cbPaymentMethod);
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _paymentTransactionService.AddNewAsync(_BuildPaymentTransactionsAddNewModel());
            if(!result.Success)
            {
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم دفع الفاتورة {_paymentInfo.InvoiceNumber} بنجاح. رقم عملية الدفع هو {result.Data.PaymentID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsPaymentTransactionsAddNewModel _BuildPaymentTransactionsAddNewModel()
        {
            return new clsPaymentTransactionsAddNewModel
            {
                InvoiceID = _paymentInfo.InvoiceID,
                PaymentMethodID = (enPaymentMethod)cbPaymentMethod.SelectedValue,
                PaidAmount = _paymentInfo.TotalAmount,
                Reference = clsUtil.NullIfEmpty(txtReference.Text)
            };
        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();
        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation == null || validation.Errors.Count == 0)
                return;

            foreach(var error in validation.Errors)
            {
                if(_validationControls.TryGetValue(error.FieldName,out Control control))
                {
                    errorProvider1.SetError(control, error.Message);
                }
            }
        }
    }
}
