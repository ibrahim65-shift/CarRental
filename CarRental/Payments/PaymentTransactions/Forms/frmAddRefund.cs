using CarRental.Helper;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Payments.PaymentTransactions.Forms
{
    public partial class frmAddRefund : Form
    {
        private clsPaymentTransactionsService _paymentTransactionService;
        private Dictionary<string, Control> _validationControl;

        private int _paymentId;
        private bool _isSaving;
        public frmAddRefund(clsPaymentTransactionsService paymentTransactionsService , int paymentId)
        {
            InitializeComponent();
            _paymentTransactionService = paymentTransactionsService;
            _paymentId = paymentId;

            _validationControl = new Dictionary<string, Control>
            {
                {"PaymentID" , lblPaymentId },
                {"RefundAmount" , numericUpDownRefundAmount },
                {"Reference" , txtReference },
            };
        }

        private void frmRefund_Load(object sender, EventArgs e)
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

                await _SaveAddRefundAsync();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("frmAddRefund.btnSave_Click", ex);
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

        // =============== METHODES ==============

        private void _InitializeForm()
        {
            lblPaymentId.Text = _paymentId.ToString();
            clsUiHelper.FillComboBoxWithEnumDescriptions<enPaymentMethod>(cbPaymentMethod);
        }
        private async Task _SaveAddRefundAsync()
        {
            var result = await _paymentTransactionService.AddRefundAsync(_BuildPaymentRefundModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم استرجاع مبلغ {clsUiHelper.ToSAR(numericUpDownRefundAmount.Value)} ريال بنجاح.");
            DialogResult = DialogResult.OK;
            Close();
        }
        private clsPaymentRefundModel _BuildPaymentRefundModel()
        {
            return new clsPaymentRefundModel
            {
                PaymentID = _paymentId,
                PaymentMethod=(enPaymentMethod)cbPaymentMethod.SelectedValue,
                RefundAmount = numericUpDownRefundAmount.Value,
                Reference = clsUtil.NullIfEmpty(txtReference.Text)
            };

        }
        private void _ApplyValidationErrors(clsValidationResult validation)
        {
            _ClearAllErrors();

            if (validation == null || validation.Errors.Count == 0)
                return;

            foreach (var error in validation.Errors)
            {
                if (_validationControl.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
            }
        }
        private void _ClearAllErrors()
        {
            errorProvider1.Clear();
        }
    }
}
