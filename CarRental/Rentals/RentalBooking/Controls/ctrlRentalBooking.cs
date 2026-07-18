using CarRental.Customers.CustomersList.Forms;
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental.Payments.Invoices.Forms;
using CarRental.Rentals.RentalBooking.Controls;
using CarRental.Rentals.RentalBooking.Forms;
using CarRental.Vehicles.VehicleDamage.Forms;
using CarRental.Vehicles.VehiclesList.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.RentalBooking;
using CarRental_Buisness.Services.VehicleReturn;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Rentals.RentalBooking.Controls
{
    public partial class ctrlRentalBooking : UserControl, IRefreshable
    {
        private static class Columns
        {
            public const string  BookingID              = nameof(BookingID);
            public const string  CustomerID             = nameof(CustomerID);
            public const string  VehicleID              = nameof(VehicleID);
            public const string  RentalStartDate        = nameof(RentalStartDate);
            public const string  RentalEndDate          = nameof(RentalEndDate);
            public const string  InitialRentalDays      = nameof(InitialRentalDays);
            public const string  PickupLocationName     = nameof(PickupLocationName);
            public const string  DropOffLocationName    = nameof(DropOffLocationName);
            public const string  RentalPricePerDay      = nameof(RentalPricePerDay);
            public const string  InitialTotalDueAmount  = nameof(InitialTotalDueAmount);
            public const string  BookingStatusID         = nameof(BookingStatusID);
            public const string  StatusName             = nameof(StatusName);
            public const string  IsOverdue              = nameof(IsOverdue);
            public const string  IsTodayStart           = nameof(IsTodayStart);
            public const string  IsTodayEnd             = nameof(IsTodayEnd);
            public const string  InitialCheckNotes      = nameof(InitialCheckNotes);
            public const string  CreatedDate            = nameof(CreatedDate);
            public const string  CreatedByUserID        = nameof(CreatedByUserID);
            public const string  EditedDate             = nameof(EditedDate);
            public const string  EditedByUserID         = nameof(EditedByUserID);
           
        }
        private enum enFilter
        {
            BookingID,
            CustomerID,
            VehicleID,
            PickupLocationName,
            DropOffLocationName,
            RentalPricePerDay,
            StatusName,
            IsOverdue,
            IsTodayStart,
            IsTodayEnd,
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
        private readonly clsRentalBookingService _RentalBookingService;
        private readonly clsVehicleReturnService _vehicleReturnSerivce;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlRentalBooking(frmMain frmMain)
        {
            InitializeComponent();
            _RentalBookingService = new clsRentalBookingService();
            _vehicleReturnSerivce = new clsVehicleReturnService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlRentalBooking_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRentalBooking.ctrlRentalBooking_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditRentalBooking frm = new frmAddEditRentalBooking(_RentalBookingService))
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
                clsEventLogger.LogException("ctrlRentalBooking.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int RentalBookingId))
                return;

            try
            {
                using (frmAddEditRentalBooking frm = new frmAddEditRentalBooking(_RentalBookingService,RentalBookingId))
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
                clsEventLogger.LogException("ctrlRentalBooking.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int RentalBookingId))
                return;

            try
            {
                if (!await _CheckDatabaseConnection())
                {
                    _ShowServerErrorState();
                    return;
                }

                if (!clsMessages.ShowDeleteDialog())
                    return;

                var result = await _RentalBookingService.DeleteAsync(RentalBookingId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف حجز الإيجار التي تحمل الرقم التعريفي '{RentalBookingId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف حجز الإيجار");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlRentalBooking.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsOverdue ||
                _currentColumn == Columns.IsTodayStart || 
                _currentColumn == Columns.IsTodayEnd)
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
                clsEventLogger.LogException("ctrlRentalBooking.cbYesOrNo_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlRentalBooking.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemStartInpection.Enabled = false;
            toolStripMenuItemVehicleDamage.Enabled = false;
            toolStripMenuItemUpdateStatus.Enabled = false;

            if (!_TryGetSelectedRentalBookingId(out int bookingID))
                return;

 
            enBookingStatus status = (enBookingStatus)Convert.ToInt32(dgvListRentalBooking.CurrentRow.Cells["BookingStatusID"].Value);

            if(status == enBookingStatus.Draft || status == enBookingStatus.Confirmed)
            {
                toolStripMenuItemUpdateStatus.Enabled = true;
            }
        

            if(status == enBookingStatus.Completed)
            {
                var result = await _RentalBookingService.IsDuplicateVehilceDamageAsync(bookingID);
                toolStripMenuItemVehicleDamage.Enabled = result.Success;
            }

            if (status != enBookingStatus.Active)
                return;
             
            var result2 = await _RentalBookingService.CanStartVehicleReturnAsync(bookingID);
            toolStripMenuItemStartInpection.Enabled = result2.Success;
        }
        private async void toolStripMenuItemStartInpection_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int BookingId))
                return;

            if (MessageBox.Show("هل تريد بدء إرجاع المركبة ؟", "بدء إرجاع المركبة", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            var result = await _vehicleReturnSerivce.StartInspectionAsync(BookingId);
            if(!result.Success)
            {
                clsMessages.ShowError(result.ErrorMessage);
                return;
            }

            clsMessages.ShowSuccess("تم بدء عملية إرجاع المركبة بنجاح. سيتم الآن فتح شاشة إرجاع المركبات لإكمال العملية.");

            await RefreshDataAsync();
            DataRefreshed?.Invoke();

            _frmMain.OpenVehicleReturnPage();
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
        private void toolStripMenuItemVehicleDamage_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int bookingId))
                return;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.VehicleID, out int vehicleId))
                return;

            using (frmAddEditVehicleDamage frm = new frmAddEditVehicleDamage(vehicleId, bookingId,null))
                frm.ShowDialog();
        }
        private async void toolStripMenuItemUpdateStatus_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int bookingId))
                return;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.BookingStatusID, out int bookingStatusId))
                return;

            using (frmUpdateRentalBookingStatus frm = new frmUpdateRentalBookingStatus(bookingId, (enBookingStatus)bookingStatusId))
            {
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
            }
        }
        private void toolStripMenuItemViewInvoice_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRentalBookingId(out int bookingId))
                return;

            using (frmInvoiceCardInfo frm = new frmInvoiceCardInfo(null, bookingId))
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
                    clsEventLogger.LogException("ctrlRentalBooking.LoadDataAsync", ex);
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
                var result = await _RentalBookingService.GetPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListRentalBooking.DataSource = null;
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

                dgvListRentalBooking.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListRentalBooking.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRentalBooking._ApplyPagingAsync", ex);
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
            if (dgvListRentalBooking.DataSource == null || dgvListRentalBooking.Rows.Count == 0)
                return;


            _SetColumnHeader(Columns.BookingID             , "المعرف"       );
            _SetColumnHeader(Columns.CustomerID            , "معرف العميل"  );
            _SetColumnHeader(Columns.VehicleID             , "معرف المركبة" );
            _SetColumnHeader(Columns.BookingStatusID       , "معرف الحالة"  );
            _SetColumnHeader(Columns.RentalStartDate       , "بداية الإيجار" );
            _SetColumnHeader(Columns.RentalEndDate         , "نهاية الإيجار" );
            _SetColumnHeader(Columns.InitialRentalDays     , "مدة الإيجار"   );
            _SetColumnHeader(Columns.PickupLocationName    , "موقع الاستلام"  );
            _SetColumnHeader(Columns.DropOffLocationName   , "موقع التسليم" );
            _SetColumnHeader(Columns.RentalPricePerDay     , "السعر اليومي" );
            _SetColumnHeader(Columns.InitialTotalDueAmount , "الإجمالي"      );
            _SetColumnHeader(Columns.StatusName            , "الحالة"       );
            _SetColumnHeader(Columns.IsOverdue             , "متأخر ؟"      );
            _SetColumnHeader(Columns.IsTodayStart          , "يبدأ اليوم ؟" );
            _SetColumnHeader(Columns.IsTodayEnd            , "ينتهي اليوم ؟");
            _SetColumnHeader(Columns.InitialCheckNotes     , "ملاحظات"       );
            _SetColumnHeader(Columns.CreatedDate           , "تاريخ الإنشاء" );
            _SetColumnHeader(Columns.CreatedByUserID       , "المنشئ"       );
            _SetColumnHeader(Columns.EditedDate            , "تاريخ التعديل");
            _SetColumnHeader(Columns.EditedByUserID        , "المعدل"       );

            _HideColumn(Columns.BookingStatusID);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListRentalBooking.Columns.Contains(columnName))
                dgvListRentalBooking.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListRentalBooking.Columns.Contains(columnName))
                dgvListRentalBooking.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListRentalBooking.Rows.Count == 0;
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
            var data = dgvListRentalBooking.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "حجوزات الإيجارات");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"       , typeof(int));
            exportTable.Columns.Add("معرف العميل"  , typeof(int));
            exportTable.Columns.Add("معرف المركبة" , typeof(int));
            exportTable.Columns.Add("معرف الحالة" , typeof(int));
            exportTable.Columns.Add("بداية الإيجار" , typeof(string));
            exportTable.Columns.Add("نهاية الإيجار" , typeof(string));
            exportTable.Columns.Add("مدة الإيجار"   , typeof(int));
            exportTable.Columns.Add("موقع الاستلام"  , typeof(string));
            exportTable.Columns.Add("موقع التسليم" , typeof(string));
            exportTable.Columns.Add("السعر اليومي" , typeof(decimal));
            exportTable.Columns.Add("الإجمالي"      , typeof(decimal));
            exportTable.Columns.Add("الحالة"       , typeof(string));
            exportTable.Columns.Add("متأخر ؟"      , typeof(bool));
            exportTable.Columns.Add("يبدأ اليوم ؟" , typeof(bool));
            exportTable.Columns.Add("ينتهي اليوم ؟", typeof(bool));
            exportTable.Columns.Add("ملاحظات"       , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء" , typeof(string));
            exportTable.Columns.Add("المنشئ"       , typeof(int));
            exportTable.Columns.Add("تاريخ التعديل", typeof(string));
            exportTable.Columns.Add("المعدل"       , typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"       ] = row[Columns.BookingID];
                newRow["معرف العميل"  ] = row[Columns.CustomerID];
                newRow["معرف المركبة" ] = row[Columns.VehicleID];
                newRow["معرف الحالة"  ] = row[Columns.BookingStatusID];
                newRow["بداية الإيجار" ] = clsUtil.FormatDate(row[Columns.RentalStartDate]);
                newRow["نهاية الإيجار" ] = clsUtil.FormatDate(row[Columns.RentalEndDate]);
                newRow["مدة الإيجار"   ] = row[Columns.InitialRentalDays];
                newRow["موقع الاستلام"  ] = row[Columns.PickupLocationName];
                newRow["موقع التسليم" ] = row[Columns.DropOffLocationName];
                newRow["السعر اليومي" ] = row[Columns.RentalPricePerDay];
                newRow["الإجمالي"      ] = row[Columns.InitialTotalDueAmount];
                newRow["الحالة"       ] = row[Columns.StatusName];
                newRow["متأخر ؟"      ] = row[Columns.IsOverdue];
                newRow["يبدأ اليوم ؟" ] = row[Columns.IsTodayStart];
                newRow["ينتهي اليوم ؟"] = row[Columns.IsTodayEnd];
                newRow["ملاحظات"       ] = row[Columns.InitialCheckNotes];
                newRow["تاريخ الإنشاء" ] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"       ] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"       ] = row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedRentalBookingId(out int BookingId)
        {
            BookingId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.BookingID, out BookingId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListRentalBooking.CurrentRow;

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
        private string _GetFilterValue()
        {
            if (_currentColumn == Columns.IsOverdue ||
                _currentColumn == Columns.IsTodayStart ||
                _currentColumn == Columns.IsTodayEnd )
                return cbYesOrNo.SelectedIndex == (int)enYesOrNo.Yes ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.BookingID: return Columns.BookingID;
                case enFilter.CustomerID: return Columns.CustomerID ;
                case enFilter.VehicleID: return Columns.VehicleID;
                case enFilter.PickupLocationName: return Columns.PickupLocationName;
                case enFilter.DropOffLocationName: return Columns.DropOffLocationName;
                case enFilter.RentalPricePerDay: return Columns.RentalPricePerDay;
                case enFilter.StatusName: return Columns.StatusName;
                case enFilter.IsOverdue: return Columns.IsOverdue;
                case enFilter.IsTodayStart: return Columns.IsTodayStart;
                case enFilter.IsTodayEnd: return Columns.IsTodayEnd;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        
    }
}