using CarRental.Helper;
using CarRental.Payments.PaymentTransactions.Controls;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.PaymentTransactions;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRental_Buisness.Helpers;
using CarRental.Payments.PaymentTransactions.Forms;
using CarRental.Payments.Invoices.Forms;

namespace CarRental.Payments.PaymentTransactions.Controls
{
    public partial class ctrlPaymentTransactions : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  PaymentID        = nameof(PaymentID);
            public const string  InvoiceID        = nameof(InvoiceID);
            public const string  MethodName       = nameof(MethodName);
            public const string  StatusName       = nameof(StatusName);
            public const string  PaymentDate      = nameof(PaymentDate);
            public const string  PaidAmount       = nameof(PaidAmount);
            public const string  Reference        = nameof(Reference);
            public const string  CreatedDate      = nameof(CreatedDate);
            public const string  CreatedByUserID  = nameof(CreatedByUserID);
            public const string  EditedDate       = nameof(EditedDate);
            public const string  EditedByUserID   = nameof(EditedByUserID);
           
        }

        private enum enFilter
        {
            PaymentID,
            InvoiceID,
            MethodName,
            TypeName,
            StatusName,
            CreatedByUserID,
            EditedByUserID
        }

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsPaymentTransactionsService _PaymentTransactionservice;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlPaymentTransactions(frmMain frmMain)
        {
            InitializeComponent();
            _PaymentTransactionservice = new clsPaymentTransactionsService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlPaymentTransactions_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.ctrlPaymentTransactions_Load", ex);
                clsMessages.ShowError();
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            _ExportToExcel();
        }
        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblSearch.Visible = string.IsNullOrEmpty(txtSearch.Text);
                int currentRequest = ++_searchRequestId;

                await _debouncer.DebounceAsync(async () =>
                {
                    if (currentRequest != _searchRequestId)
                        return;

                    _currentPage = 1;
                    await _ReloadDataAsync();
                });
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.txtSearch_TextChanged", ex);
                clsMessages.ShowError();
            }
        }
        private async void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    _currentPage = 1;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.txtSearch_KeyPress", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.btnRefresh_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _ReloadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
        }
        private async void cbPageNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (_isUpdatingPageCombo)
                    return;

                if (cbPageNumber.SelectedIndex >= 0)
                {
                    _currentPage = cbPageNumber.SelectedIndex + 1;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.cbPageNumber_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.btnPrevious_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentPage < _totalPages)
                {
                    _currentPage++;
                    await _ReloadDataAsync();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }

        private async void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        private void toolStripMenuItemShowInvoice_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.InvoiceID, out int invoiceId))
                return;

            using (frmInvoiceCardInfo frm = new frmInvoiceCardInfo(invoiceId, null))
                frm.ShowDialog();
        }
        private async void toolStripMenuItemRefund_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedPaymentTransactionId(out int paymentId))
                return;

            using (frmAddRefund frm = new frmAddRefund(_PaymentTransactionservice, paymentId))
            {
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
            }
        }

        // ==================  METHODS ===================

        private async Task _LoadDataAsync()
        {

            using (var loading = new frmLoading())
            {
                try
                {
                    loading.Show();
                    loading.Refresh();
                    if (!await _CheckDatabaseConnection())
                    {
                        _ShowServerErrorState();
                        return;
                    }

                    await _ReloadDataAsync();
                }
                catch (Exception ex)
                {
                    clsMessages.ShowError("حدث خطأ أثناء تحميل البيانات");
                    clsEventLogger.LogException("ctrlPaymentTransactions.LoadDataAsync", ex);
                    _ShowServerErrorState();
                }
                finally
                {
                    loading.Close();
                }
            }
        }
        private async Task _ReloadDataAsync()
        {
            string value = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                _currentSearchText = null;
                _currentColumn = null;
            }
            else
            {
                var filter = (enFilter)cbFilter.SelectedIndex;

                _currentSearchText = value;
                _currentColumn = _GetColumnNameForFilter(filter);
            }

            await _ApplyPagingAsync();
        }
        private async Task _ApplyPagingAsync()
        {
            try
            {
                var result = await _PaymentTransactionservice.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListPaymentTransactions.DataSource = null;
                    _columnsInitialized = false;
                    _UpdatePagingControls(0);
                    _ShowEmptyDataState();
                    return;
                }

                int newTotalPages = result.Data.TotalPages;

                if (_currentPage > newTotalPages && newTotalPages > 0)
                {
                    _currentPage = newTotalPages;
                }

                dgvListPaymentTransactions.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListPaymentTransactions.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlPaymentTransactions._ApplyPagingAsync", ex);
                _ShowServerErrorState();
            }
        }
        private void _UpdatePagingControls(int newTotalPages)
        {
            lblTotalPages.Text = newTotalPages.ToString();
            _totalPages = newTotalPages;

            if (newTotalPages <= 0)
            {
                _currentPage = 1; 
                cbPageNumber.Items.Clear();

                try
                {
                    _isUpdatingPageCombo = true;
                    cbPageNumber.Text = "";
                    cbPageNumber.SelectedIndex = -1;
                }
                finally
                {
                    _isUpdatingPageCombo = false;
                }

                // تعطيل أزرار التنقل لأن البيانات فارغة
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;

                return;
            }

            if (cbPageNumber.Items.Count != newTotalPages)
            {
                cbPageNumber.Items.Clear();
                for (int i = 1; i <= newTotalPages; i++)
                    cbPageNumber.Items.Add(i);
            }

            int newIndex = _currentPage - 1;
            if (newIndex >= 0 && newIndex < cbPageNumber.Items.Count)
            {
                try
                {
                    _isUpdatingPageCombo = true;
                    cbPageNumber.SelectedIndex = newIndex;
                }
                finally
                {
                    _isUpdatingPageCombo = false;
                }
            }

            btnPrevious.Enabled = (_currentPage > 1);
            btnNext.Enabled = (_currentPage < _totalPages);
        }
        private void _InitializeColumns()
        {
            if (dgvListPaymentTransactions.DataSource == null || dgvListPaymentTransactions.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.PaymentID      , "المعرف"        );
            _SetColumnHeader(Columns.InvoiceID      , "معرف الفاتورة"        );
            _SetColumnHeader(Columns.MethodName     , "طريقة الدفع"   );
            _SetColumnHeader(Columns.StatusName     , "الحالة"        );
            _SetColumnHeader(Columns.PaymentDate    , "تاريخ الدفع"   );
            _SetColumnHeader(Columns.PaidAmount     , "المبلغ المدفوع");
            _SetColumnHeader(Columns.Reference      , "رقم مرجعي"     );
            _SetColumnHeader(Columns.CreatedDate    , "تاريخ الإنشاء"  );
            _SetColumnHeader(Columns.CreatedByUserID, "المنشئ"        );
            _SetColumnHeader(Columns.EditedDate     , "تاريخ التعديل" );
            _SetColumnHeader(Columns.EditedByUserID , "المعدل"        );


            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListPaymentTransactions.Columns.Contains(columnName))
                dgvListPaymentTransactions.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListPaymentTransactions.Columns.Contains(columnName))
                dgvListPaymentTransactions.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListPaymentTransactions.Rows.Count == 0;
            lblTitleState.Text = isEmpty ? Properties.Resources.EmptyDataStateTitle : "";
            lblDescriptionState.Text = isEmpty ? Properties.Resources.EmptyDataStateDescription : "";
            pnlState.Visible = isEmpty;
        }
        private void _ShowServerErrorState()
        {
            lblTitleState.Text = Properties.Resources.ServerErrorTitle;
            lblDescriptionState.Text = Properties.Resources.ServerErrorDescription;
            pnlState.Visible = true;
        }
        private void _ExportToExcel()
        {
            var data = dgvListPaymentTransactions.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "عمليات الدفع");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"        , typeof(int));
            exportTable.Columns.Add("معرف الفاتورة"        , typeof(int));
            exportTable.Columns.Add("طريقة الدفع"   , typeof(string));
            exportTable.Columns.Add("النوع"         , typeof(string));
            exportTable.Columns.Add("الحالة"        , typeof(string));
            exportTable.Columns.Add("تاريخ الدفع"   , typeof(string));
            exportTable.Columns.Add("المبلغ المدفوع", typeof(decimal));
            exportTable.Columns.Add("رقم مرجعي", typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء"  , typeof(string));
            exportTable.Columns.Add("المنشئ"        , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل" , typeof(string));
            exportTable.Columns.Add("المعدل"        , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"        ] = row[Columns.PaymentID];
                newRow["معرف الفاتورة"        ] = row[Columns.InvoiceID];
                newRow["طريقة الدفع"   ] = row[Columns.MethodName];
                newRow["الحالة"        ] = row[Columns.StatusName];
                newRow["تاريخ الدفع"   ] = clsUtil.FormatDate(row[Columns.PaidAmount]);
                newRow["المبلغ المدفوع"] = row[Columns.PaidAmount];
                newRow["رقم مرجعي"     ] = row[Columns.Reference];
                newRow["تاريخ الإنشاء"  ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"        ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل" ] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"        ] = row[Columns.EditedByUserID];
              
                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedPaymentTransactionId(out int PaymentTransactionId)
        {
            PaymentTransactionId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.PaymentID, out PaymentTransactionId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListPaymentTransactions.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
        }
        private bool _TryGetCellIntValue(DataGridViewRow row, string columnName, out int value)
        {
            value = 0;

            try
            {
                if (row == null)
                    return false;

                if (!row.DataGridView.Columns.Contains(columnName))
                    return false;

                var cell = row.Cells[columnName];
                if (cell?.Value == null || cell.Value == DBNull.Value)
                    return false;

                value = Convert.ToInt32(cell.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> _CheckDatabaseConnection()
        {
            return await clsUtil.CheckDatabaseConnection();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.PaymentID: return Columns.PaymentID;
                case enFilter.InvoiceID: return Columns.InvoiceID;
                case enFilter.MethodName: return Columns.MethodName;
                case enFilter.StatusName: return Columns.StatusName;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

 
    }
}