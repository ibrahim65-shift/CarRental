using CarRental.Attachments.Forms;
using CarRental.Customers.CustomersList.Forms;
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental.Payments.Invoices.Forms;
using CarRental.Rentals.VehicleReturn.Controls;
using CarRental.Rentals.VehicleReturn.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.VehicleReturn;
using CarRental_Buisness.Services.WorkFlow;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Rentals.VehicleReturn.Controls
{
    public partial class ctrlVehicleReturn : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  ReturnID               = nameof(ReturnID);
            public const string  BookingID              = nameof(BookingID);
            public const string  ReturnStatusID         = nameof(ReturnStatusID);
            public const string  ReturnStatus           = nameof(ReturnStatus);
            public const string  CustomerID             = nameof(CustomerID);
            public const string  VehicleID              = nameof(VehicleID);
            public const string  ActualReturnDate       = nameof(ActualReturnDate);
            public const string  InitialRentalDays      = nameof(InitialRentalDays);
            public const string  LateDays               = nameof(LateDays);
            public const string  IsLateReturn           = nameof(IsLateReturn);
            public const string  MileageStart           = nameof(MileageStart);
            public const string  MileageEnd             = nameof(MileageEnd);
            public const string  ConsumedMileage        = nameof(ConsumedMileage);
            public const string  AdditionalCharges      = nameof(AdditionalCharges);
            public const string  HasAdditionalCharges   = nameof(HasAdditionalCharges);
            public const string  FinalCheckNotes        = nameof(FinalCheckNotes);
            public const string  CreatedDate            = nameof(CreatedDate);
            public const string  CreatedByUserID        = nameof(CreatedByUserID);
            public const string  EditedDate             = nameof(EditedDate);
            public const string  EditedByUserID         = nameof(EditedByUserID);
           
        }
        private enum enFilter
        {
           ReturnID,
		   ReturnStatus,
		   IsLateReturn,
		   HasAdditionalCharges,
		   CreatedByUserID,
		   EditedByUserID
        }

        private enum enYesOrNo { Yes = 0, No = 1 };

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsVehicleReturnService _VehicleReturnService;
        private readonly clsVehicleReturnWorkflowService _returnWorkflowService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlVehicleReturn(frmMain frmMain)
        {
            InitializeComponent();
            _VehicleReturnService = new clsVehicleReturnService();
            _returnWorkflowService = new clsVehicleReturnWorkflowService(_VehicleReturnService);
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlVehicleReturn_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleReturn.ctrlVehicleReturn_Load", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsLateReturn ||
                _currentColumn == Columns.HasAdditionalCharges)
            {
                cbYesOrNo.SelectedIndex = 0;
                txtSearch.Visible = false;
                cbYesOrNo.Visible = true;
                lblSearch.Visible = false;
            }
            else
            {
                txtSearch.Visible = true;
                cbYesOrNo.Visible = false;
                lblSearch.Visible = true;
            }
        }
        private async void cbYesOrNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                await _ReloadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleReturn.cbYesOrNo_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
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
                clsEventLogger.LogException("ctrlVehicleReturn.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlVehicleReturn.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetSelectedVehicleReturnId(out int returnID))
                return;

            _ApplyReturnStatus();

        }
        private async void toolStripMenuItemUpdateInspection_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleReturnId(out int returnID))
                return;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.MileageStart, out int mileageStart))
                return;

            var inspectionInfo = _GetInspectionInfo(row);

            using (frmUpdateInspection frm = new frmUpdateInspection(_VehicleReturnService,returnID,mileageStart, inspectionInfo))
            {
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
            }
        }
        private async void toolStripMenuItemFinalizeReturn_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleReturnId(out int returnID))
                return;

            if (MessageBox.Show("هل تريد إنهاء عملية إرجاع المركبة ؟", "إرجاع المركبة", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;

            var result = await _VehicleReturnService.FinalizeAsync(returnID);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess();

            await RefreshDataAsync();
            DataRefreshed?.Invoke();
        }
        private async void toolStripMenuItemMarkAsInvoiced_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedVehicleReturnId(out int returnID))
                return;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, Columns.BookingID, out int bookingID))
                return;

            if (!_TryGetCellValue<int>(row, Columns.LateDays, out int lateDays))
                return;

            if (!_TryGetCellValue<decimal>(row, Columns.AdditionalCharges, out decimal additionalCharges))
                additionalCharges = 0;

            if (MessageBox.Show("هل تريد إصدار الفاتورة وإنهاء عملية إرجاع المركبة؟","تأكيد", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
                return;

            var result = await _returnWorkflowService.MarkAsInvoicedAsync( bookingID,returnID, lateDays, additionalCharges, clsUiHelper.applicationSettings);
            if (!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess("تم إصدار الفاتورة بنجاح.");

            await RefreshDataAsync();
            DataRefreshed?.Invoke();

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

            if (!_TryGetCellIntValue(row, Columns.CustomerID, out int customerID))
                return;

            using (frmCustomerCardInfo frm = new frmCustomerCardInfo(customerID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemViewInvoice_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, Columns.BookingID, out int bookingID))
                return;

            using (frmInvoiceCardInfo frm = new frmInvoiceCardInfo(null, bookingID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemAttach_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, Columns.ReturnID, out int returnId))
                return;

            using (frmRelatedAttachments frm = new frmRelatedAttachments("VehicleReturn", returnId, "إرجاع المركبة"))
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
                    clsEventLogger.LogException("ctrlVehicleReturn.LoadDataAsync", ex);
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
            string value = _GetFilterValue();

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
                var result = await _VehicleReturnService.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListVehicleReturn.DataSource = null;
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

                dgvListVehicleReturn.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListVehicleReturn.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleReturn._ApplyPagingAsync", ex);
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
            if (dgvListVehicleReturn.DataSource == null || dgvListVehicleReturn.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.ReturnID              , "المعرف"               );
            _SetColumnHeader(Columns.BookingID             , "معرف الحجز"           );
            _SetColumnHeader(Columns.ReturnStatusID        , "معرف الحالة"          );
            _SetColumnHeader(Columns.ReturnStatus          , "الحالة"               );
            _SetColumnHeader(Columns.CustomerID            , "معرف العميل"          );
            _SetColumnHeader(Columns.VehicleID             , "معرف المركبة"         );
            _SetColumnHeader(Columns.ActualReturnDate      , "تاريخ الإرجاع الفعلي"  );
            _SetColumnHeader(Columns.InitialRentalDays     , "أيام الإيجار الأصلية"   );
            _SetColumnHeader(Columns.LateDays              , "أيام التأخير"         );
            _SetColumnHeader(Columns.IsLateReturn          , "متأخر ؟"              );
            _SetColumnHeader(Columns.MileageStart          , "العداد عند الاستلام"    );
            _SetColumnHeader(Columns.MileageEnd            , "العداد عند الإرجاع"    );
            _SetColumnHeader(Columns.ConsumedMileage       , "الكيلومترات المستهلكة");
            _SetColumnHeader(Columns.AdditionalCharges     , "رسوم إضافية"          );
            _SetColumnHeader(Columns.HasAdditionalCharges  , "توجد رسوم إضافية ؟"   );
            _SetColumnHeader(Columns.FinalCheckNotes       , "ملاحظات الفحص النهائي" );
            _SetColumnHeader(Columns.CreatedDate           , "تاريخ الإنشاء"         );
            _SetColumnHeader(Columns.CreatedByUserID       , "المنشئ"               );
            _SetColumnHeader(Columns.EditedDate            , "تاريخ التعديل"        );
            _SetColumnHeader(Columns.EditedByUserID        , "المعدل"               );


            _HideColumn(Columns.BookingID);
            _HideColumn(Columns.ReturnStatusID);
            _HideColumn(Columns.CustomerID);
            _HideColumn(Columns.VehicleID);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListVehicleReturn.Columns.Contains(columnName))
                dgvListVehicleReturn.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListVehicleReturn.Columns.Contains(columnName))
                dgvListVehicleReturn.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListVehicleReturn.Rows.Count == 0;
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
            var data = dgvListVehicleReturn.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "السيارات المرجعة");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"               , typeof(int));
            exportTable.Columns.Add("معرف الحجز"           , typeof(int));
            exportTable.Columns.Add("معرف الحالة"          , typeof(int));
            exportTable.Columns.Add("الحالة"               , typeof(string));
            exportTable.Columns.Add("معرف العميل"          , typeof(int));
            exportTable.Columns.Add("معرف المركبة"         , typeof(int));
            exportTable.Columns.Add("تاريخ الإرجاع الفعلي"  , typeof(string));
            exportTable.Columns.Add("أيام الإيجار الأصلية"   , typeof(int));
            exportTable.Columns.Add("أيام التأخير"         , typeof(int));
            exportTable.Columns.Add("متأخر ؟"              , typeof(decimal));
            exportTable.Columns.Add("العداد عند الاستلام"    , typeof(int));
            exportTable.Columns.Add("العداد عند الإرجاع"    , typeof(int));
            exportTable.Columns.Add("الكيلومترات المستهلكة", typeof(int));
            exportTable.Columns.Add("رسوم إضافية"          , typeof(decimal));
            exportTable.Columns.Add("توجد رسوم إضافية ؟"   , typeof(bool));
            exportTable.Columns.Add("ملاحظات الفحص النهائي" , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء"         , typeof(string));
            exportTable.Columns.Add("المنشئ"               , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل"        , typeof(string));
            exportTable.Columns.Add("المعدل"               , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"               ]= row[Columns.ReturnID];
                newRow["معرف الحجز"           ]= row[Columns.BookingID];
                newRow["معرف الحالة"          ]= row[Columns.ReturnStatusID];
                newRow["الحالة"               ]= row[Columns.ReturnStatus];
                newRow["معرف العميل"          ]= row[Columns.CustomerID];
                newRow["معرف المركبة"         ]= row[Columns.VehicleID];
                newRow["تاريخ الإرجاع الفعلي"  ]= clsUtil.FormatDate(row[Columns.ActualReturnDate]);
                newRow["أيام الإيجار الأصلية"   ]= row[Columns.InitialRentalDays];
                newRow["أيام التأخير"         ]= row[Columns.LateDays];
                newRow["متأخر ؟"              ]= row[Columns.IsLateReturn];
                newRow["العداد عند الاستلام"    ]= row[Columns.MileageStart];
                newRow["العداد عند الإرجاع"    ]= row[Columns.MileageEnd];
                newRow["الكيلومترات المستهلكة"]= row[Columns.ConsumedMileage];
                newRow["رسوم إضافية"          ]= row[Columns.AdditionalCharges];
                newRow["توجد رسوم إضافية ؟"   ]= row[Columns.HasAdditionalCharges];
                newRow["ملاحظات الفحص النهائي" ]= row[Columns.FinalCheckNotes];
                newRow["تاريخ الإنشاء"         ]= clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"               ]= row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"        ]= clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"               ]= row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedVehicleReturnId(out int ReturnID)
        {
            ReturnID = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.ReturnID, out ReturnID);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListVehicleReturn.CurrentRow;

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
        private string _GetFilterValue()
        {
            if (_currentColumn == Columns.IsLateReturn ||
                _currentColumn == Columns.HasAdditionalCharges )
                return cbYesOrNo.SelectedIndex == (int)enYesOrNo.Yes ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.ReturnID: return Columns.ReturnID;
                case enFilter.ReturnStatus: return Columns.ReturnStatus ;
                case enFilter.IsLateReturn: return Columns.IsLateReturn;
                case enFilter.HasAdditionalCharges: return Columns.HasAdditionalCharges;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private void _ApplyReturnStatus()
        {
            _ResetContextMenu();

            enReturnStatus status = (enReturnStatus)Convert.ToInt32(dgvListVehicleReturn.CurrentRow.Cells["ReturnStatusID"].Value);

            if (!Enum.IsDefined(typeof(enReturnStatus), status))
                return;

            switch (status)
            {
                case enReturnStatus.UnderInspection:

                    toolStripMenuItemUpdateInspection.Enabled = true;

                    toolStripMenuItemFinalizeReturn.Enabled = true;

                    break;

                case enReturnStatus.Finalized:

                    toolStripMenuItemMarkAsInvoiced.Enabled = true;

                    break;

                case enReturnStatus.Invoiced:
                    toolStripMenuItemViewInvoice.Enabled = true;
                    break;

                default:
                    break;
            }
        }
        private void _ResetContextMenu()
        {
            toolStripMenuItemUpdateInspection.Enabled = false;
            toolStripMenuItemFinalizeReturn.Enabled = false;
            toolStripMenuItemMarkAsInvoiced.Enabled = false;
            toolStripMenuItemViewInvoice.Enabled = false;
        }
        private clsUpdateInspectionModel _GetInspectionInfo(DataGridViewRow row)
        {
            var inspectionInfo = new clsUpdateInspectionModel();

            if (_TryGetCellValue<int>(row, Columns.MileageEnd, out int mileageEnd))
                inspectionInfo.MileageEnd = mileageEnd;

            if (_TryGetCellValue<decimal>(row, Columns.AdditionalCharges, out decimal additionalCharges))
                inspectionInfo.AdditionalCharges = additionalCharges;

            if (_TryGetCellValue<string>(row, Columns.FinalCheckNotes, out string finalCheckNotes))
                inspectionInfo.FinalCheckNotes = finalCheckNotes;

            return inspectionInfo;
        }

    }
}