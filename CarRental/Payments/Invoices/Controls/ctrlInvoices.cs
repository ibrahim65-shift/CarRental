using CarRental.Attachments.Forms;
using CarRental.Customers.CustomersList.Forms;
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental.Payments.Invoices.DTOs;
using CarRental.Payments.Invoices.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Services.Invoices;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Payments.Invoices.Controls
{
    public partial class ctrlInvoices : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  InvoiceID         = nameof(clsInvoicesViewDto.InvoiceID);
            public const string  InvoiceNumber     = nameof(clsInvoicesViewDto.InvoiceNumber);
            public const string  TypeName          = nameof(clsInvoicesViewDto.TypeName);
            public const string  BookingID         = nameof(clsInvoicesViewDto.BookingID);
            public const string MaintenanceID      = nameof(clsInvoicesViewDto.MaintenanceID);
            public const string DamageID      = nameof(clsInvoicesViewDto.DamageID);
            public const string  InvoiceDate       = nameof(clsInvoicesViewDto.InvoiceDate);
            public const string  BaseAmount      = nameof(clsInvoicesViewDto.BaseAmount);
            public const string  AdditionalCharges = nameof(clsInvoicesViewDto.AdditionalCharges);
            public const string  LateFees          = nameof(clsInvoicesViewDto.LateFees);
            public const string  TaxAmount         = nameof(clsInvoicesViewDto.TaxAmount);
            public const string  DiscountAmount    = nameof(clsInvoicesViewDto.DiscountAmount);
            public const string  TotalAmount       = nameof(clsInvoicesViewDto.TotalAmount);
            public const string  CurrencyCode      = nameof(clsInvoicesViewDto.CurrencyCode);
            public const string  Notes             = nameof(clsInvoicesViewDto.Notes);
            public const string  CreatedDate       = nameof(clsInvoicesViewDto.CreatedDate);
            public const string  CreatedByUserID   = nameof(clsInvoicesViewDto.CreatedByUserID);
            public const string  EditedDate        = nameof(clsInvoicesViewDto.EditedDate);
            public const string  EditedByUserID    = nameof(clsInvoicesViewDto.EditedByUserID);
            public const string  CustomerID        = nameof(clsInvoicesViewDto.CustomerID);
            public const string  VehicleID         = nameof(clsInvoicesViewDto.VehicleID);
           
        }
        private enum enFilter
        {
           InvoiceID,
           TypeName,
           BookingID,
           ReturnID,
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
        private readonly clsInvoicesService _InvoicesService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlInvoices(frmMain frmMain)
        {
            InitializeComponent();
            _InvoicesService = new clsInvoicesService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlInvoices_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlInvoices.ctrlInvoices_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedInvoiceID(out int InvoicesId))
                return;

            try
            {
                using (frmEditInvoice frm = new frmEditInvoice(_InvoicesService,InvoicesId))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        await RefreshDataAsync();
                        DataRefreshed?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlInvoices.btnEdit_Click", ex);
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
                clsEventLogger.LogException("ctrlInvoices.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlInvoices.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlInvoices.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlInvoices.btnSearch_Click", ex);
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
                clsEventLogger.LogException("ctrlInvoices.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlInvoices.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlInvoices.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemVehicleInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int VehicleID))
                return;

            using (frmVehicleCardInfo frm = new frmVehicleCardInfo(VehicleID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemCustomerInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.CustomerID, out int CustomerID))
                return;

            using (frmCustomerCardInfo frm = new frmCustomerCardInfo(CustomerID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemInvoiceInfo_Click(object sender, EventArgs e)
        {

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.InvoiceID, out int InvoiceID))
                return;

            using (frmInvoiceCardInfo frm = new frmInvoiceCardInfo(InvoiceID,null))
                frm.ShowDialog();
        }
        private async void toolStripMenuItemPayInvoice_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;
             
            var paymentInfo = _FillPaymentInfo(row);

            using(frmPayInvoice frm = new frmPayInvoice(paymentInfo))
            {
                if(frm.ShowDialog() == DialogResult.OK)
                {
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
            }
        }
        private void toolStripMenuItemAttach_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, Columns.InvoiceID, out int invoiceId))
                return;

            if (!_TryGetCellValue<string>(row, Columns.InvoiceNumber, out string invoiceNumber))
                return;

            using (frmRelatedAttachments frm = new frmRelatedAttachments("Invoices", invoiceId, invoiceNumber))
                frm.ShowDialog();
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
                    clsEventLogger.LogException("ctrlInvoices.LoadDataAsync", ex);
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
                var result = await _InvoicesService.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListInvoices.DataSource = null;
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

                dgvListInvoices.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListInvoices.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlInvoices._ApplyPagingAsync", ex);
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
            if (dgvListInvoices.DataSource == null || dgvListInvoices.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.InvoiceID         , "المعرف"        );
            _SetColumnHeader(Columns.InvoiceNumber     , "رقم الفاتورة"  );
            _SetColumnHeader(Columns.TypeName          , "النوع"  );
            _SetColumnHeader(Columns.BookingID         , "معرف الحجز"    );
            _SetColumnHeader(Columns.MaintenanceID, "معرف الصيانة"    );
            _SetColumnHeader(Columns.DamageID, "معرف الضرر"    );
            _SetColumnHeader(Columns.InvoiceDate       , "تاريخ الفاتورة");
            _SetColumnHeader(Columns.BaseAmount      , "المبلغ الأساسي"   );
            _SetColumnHeader(Columns.AdditionalCharges , "رسوم إضافية"   );
            _SetColumnHeader(Columns.LateFees          , "رسوم تأخير"    );
            _SetColumnHeader(Columns.TaxAmount         , "الضريبة"       );
            _SetColumnHeader(Columns.DiscountAmount    , "الخصم"         );
            _SetColumnHeader(Columns.TotalAmount       , "المجموع"       );
            _SetColumnHeader(Columns.CurrencyCode      , "العملة"        );
            _SetColumnHeader(Columns.Notes             , "الملاحظات"      );
            _SetColumnHeader(Columns.CreatedDate       , "تاريخ الإنشاء"  );
            _SetColumnHeader(Columns.CreatedByUserID   , "المنشئ"        );
            _SetColumnHeader(Columns.EditedDate        , "تاريخ التعديل" );
            _SetColumnHeader(Columns.EditedByUserID    , "المعدل"        );
            _SetColumnHeader(Columns.CustomerID        , "معرف العميل"   );
            _SetColumnHeader(Columns.VehicleID         , "معرف المركبة"  );


            _HideColumn(Columns.VehicleID);
            _HideColumn(Columns.CustomerID);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);
        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListInvoices.Columns.Contains(columnName))
                dgvListInvoices.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListInvoices.Columns.Contains(columnName))
                dgvListInvoices.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListInvoices.Rows.Count == 0;
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
            var data = dgvListInvoices.DataSource as List<clsInvoicesViewDto>;

            if (data == null || data.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "الفواتير");
        }
        private DataTable _CreateOptimizedExportData(List<clsInvoicesViewDto> source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"        , typeof(int));
            exportTable.Columns.Add("رقم الفاتورة"  , typeof(string));
            exportTable.Columns.Add("النوع"  , typeof(string));
            exportTable.Columns.Add("معرف الحجز"    , typeof(int));
            exportTable.Columns.Add("معرف الصيانة"    , typeof(int));
            exportTable.Columns.Add("معرف الضرر"    , typeof(int));
            exportTable.Columns.Add("معرف الإرجاع"   , typeof(int));
            exportTable.Columns.Add("تاريخ الفاتورة", typeof(string));
            exportTable.Columns.Add("المبلغ الأساسي"   , typeof(decimal));
            exportTable.Columns.Add("رسوم إضافية"   , typeof(decimal));
            exportTable.Columns.Add("رسوم تأخير"    , typeof(decimal));
            exportTable.Columns.Add("الضريبة"       , typeof(decimal));
            exportTable.Columns.Add("الخصم"         , typeof(decimal));
            exportTable.Columns.Add("المجموع"       , typeof(string));
            exportTable.Columns.Add("العملة"        , typeof(string));
            exportTable.Columns.Add("الملاحظات"      , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء"  , typeof(string));
            exportTable.Columns.Add("المنشئ"        , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل" , typeof(string));
            exportTable.Columns.Add("المعدل"        , typeof(int));
            exportTable.Columns.Add("معرف العميل"   , typeof(int));
            exportTable.Columns.Add("معرف المركبة", typeof(int));

            exportTable.BeginLoadData();

            foreach (var item in source)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"        ] = item.InvoiceID;
                newRow["رقم الفاتورة"  ] = item.InvoiceNumber;
                newRow["النوع"         ] = item.TypeName;
                newRow["معرف الحجز"] = item.BookingID ?? (object)DBNull.Value;
                newRow["معرف الصيانة"    ] = item.MaintenanceID ?? (object)DBNull.Value;
                newRow["معرف الضرر"    ] = item.DamageID ?? (object)DBNull.Value;
                newRow["تاريخ الفاتورة"] = clsUtil.FormatDate(item.InvoiceDate);
                newRow["المبلغ الأساسي"   ] = item.BaseAmount;
                newRow["رسوم إضافية"   ] = item.AdditionalCharges;
                newRow["رسوم تأخير"    ] = item.LateFees;
                newRow["الضريبة"       ] = item.TaxAmount;
                newRow["الخصم"         ] = item.DiscountAmount;
                newRow["المجموع"       ] = item.TotalAmount;
                newRow["العملة"        ] = item.CurrencyCode;
                newRow["الملاحظات"      ] = item.Notes;
                newRow["تاريخ الإنشاء"  ] = clsUtil.FormatDate(item.CreatedDate);
                newRow["المنشئ"        ] = item.CreatedByUserID;
                newRow["تاريخ التعديل" ] = clsUtil.FormatDate(item.EditedDate);
                newRow["المعدل"]         = item.EditedByUserID ?? (object)DBNull.Value;
                newRow["معرف العميل"   ] = item.CustomerID;
                newRow["معرف المركبة"  ] = item.VehicleID;


                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedInvoiceID(out int InvoiceID)
        {
            InvoiceID = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.InvoiceID, out InvoiceID);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListInvoices.CurrentRow;

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
        private bool _TryGetCellValue<T>(DataGridViewRow row, string columnName, out T value)
        {
            value = default(T);

            try
            {
                if (row == null)
                    return false;

                if (!row.DataGridView.Columns.Contains(columnName))
                    return false;

                var cell = row.Cells[columnName];
                if (cell?.Value == null || cell.Value == DBNull.Value)
                    return false;

                value = (T)Convert.ChangeType(cell.Value, typeof(T));
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
                case enFilter.InvoiceID: return Columns.InvoiceID;
                case enFilter.TypeName: return Columns.TypeName;
                case enFilter.BookingID: return Columns.BookingID ;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private clsPaymentInfo _FillPaymentInfo(DataGridViewRow row)
        {
            var paymentInfo = new clsPaymentInfo();

            if (_TryGetCellValue<int>(row, Columns.InvoiceID, out int invoiceId))
                paymentInfo.InvoiceID = invoiceId;

            if (_TryGetCellValue<string>(row, Columns.InvoiceNumber, out string invoiceNumber))
                paymentInfo.InvoiceNumber = invoiceNumber;

            if (_TryGetCellValue<decimal>(row, Columns.TotalAmount, out decimal totalAmount))
                paymentInfo.TotalAmount = totalAmount;


            return paymentInfo;
        }

    }
}