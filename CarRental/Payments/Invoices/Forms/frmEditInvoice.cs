using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
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

namespace CarRental.Payments.Invoices.Forms
{
    public partial class frmEditInvoice : Form
    {
        private clsInvoicesService _invoiceService;
        private Dictionary<string, Control> _validationControls;

        private int _invoiceId;
        private bool _isSaving;
        public frmEditInvoice(clsInvoicesService invoiceService  , int invoiceId)
        {
            InitializeComponent();
            _invoiceService = invoiceService;
            _invoiceId = invoiceId;

            _validationControls = new Dictionary<string, Control>
            {
                {"AdditionalCharges", numericUpDownAdditionalCharges },
                {"LateFees", numericUpDownLateFees },
                {"DiscountAmount", numericUpDownDiscountAmount },
                {"Notes", txtNotes },
                {"InvoiceID", lblInvoiceNumber },
            };

        }

        private async void frmEditInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                await _LoadInvoiceDataAsync();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmEditInvoice.frmEditInvoice_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            try
            {
                _isSaving = true;
                btnSave.Enabled = false;

                await _SaveUpdateInvoiceAsync();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmEditInvoice.btnSave_Click", ex);
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

        // =================== METHODES ================

        private async Task _LoadInvoiceDataAsync()
        {
            var result = await _invoiceService.GetInvoiceByIDAsync(_invoiceId);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                Close();
            }

            var invoiceData = result.Data;

            lblInvoiceNumber.Text = invoiceData.InvoiceNumber;
            numericUpDownAdditionalCharges.Value = invoiceData.AdditionalCharges;
            numericUpDownLateFees.Value = invoiceData.LateFees;
            numericUpDownDiscountAmount.Value = invoiceData.DiscountAmount;
            txtNotes.Text = invoiceData.Notes;
        }
        private async Task _SaveUpdateInvoiceAsync()
        {
            var result = await _invoiceService.UpdateAsync(_invoiceId , _BuildInvoiceUpdateModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }


            clsMessages.ShowSuccess($"تم تعديل بيانات الفاتورة بنجاح. للرقم التعريفي {_invoiceId}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsInvoicesUpdateModel _BuildInvoiceUpdateModel()
        {
            return new clsInvoicesUpdateModel
            {
                AdditionalCharges = numericUpDownAdditionalCharges.Value,
                LateFees = numericUpDownLateFees.Value,
                DiscountAmount = numericUpDownDiscountAmount.Value,
                Notes = clsUtil.NullIfEmpty(txtNotes.Text)
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

            foreach (var error in validation.Errors)
            {
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
            }
        }
    }
}
