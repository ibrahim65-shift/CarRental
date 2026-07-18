
using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental.SystemSettings.Users.Forms;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Services.Users;
using CarRental_Buisness.Services.Users;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.SystemSettings.Users.Controls
{
    public partial class ctrlUsers : UserControl,IRefreshable
    {
        private static class Columns
        {
            public const string UserID = "UserID";
            public const string FullName = "FullName";
            public const string Age = "Age";
            public const string Gender = "Gender";
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string Address = "Address";
            public const string Description = "Description";
            public const string UserName = "UserName";
            public const string IsActive = "IsActive";
            public const string IsLockedOut = "IsLockedOut";
            public const string FailedLoginAttempts = "FailedLoginAttempts";
            public const string LastFailedLoginDate = "LastFailedLoginDate";
            public const string CreatedDate = "CreatedDate";
            public const string CreatedByUserID = "CreatedByUserID";
            public const string EditedDate = "EditedDate";
            public const string EditedByUserID = "EditedByUserID";
            public const string PersonID = "PersonID";
        }

        private enum enFilter
        {
            UserID = 0, UserName = 1, FullName = 2,
            Phone = 3, Email = 4, Address = 5, Age = 6,
            Gender = 7, IsActive = 8, IsLocked = 9,
            FailedAttempts = 10, CreatedByUserID = 11,
            EditedByUserID = 12 
        }

        private enum enYesOrNo { Yes = 0, No = 1 };

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsUserService _UserService;

        private int _currentPage = 1;
        private int _totalPages = 0;

        // Event for external notification instead of static manager
        public event Action DataRefreshed;

        public ctrlUsers(frmMain frmMain)
        {
            InitializeComponent();
            _UserService = new clsUserService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlUsers_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlUsers.ctrlUsers_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditUser frm = new frmAddEditUser())
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
                clsEventLogger.LogException("ctrlUsers.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedUserId(out int UserId))
                return;

            try
            {
                using (frmAddEditUser frm = new frmAddEditUser(UserId))
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
                clsEventLogger.LogException("ctrlUsers.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedUserId(out int UserId))
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

                var result = await _UserService.DeleteAsync(UserId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف الشخص الذي يحمل الرقم التعريفي '{UserId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف الشخص");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlUsers.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlUsers.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlUsers.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlUsers.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlUsers.btnSearch_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsLockedOut)
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
                clsEventLogger.LogException("ctrlUsers.cbYesOrNo_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlUsers.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlUsers.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlUsers.btnNext_Click", ex);
                clsMessages.ShowError();
            }
        }
        private void toolStripMenuItemPersonInfo_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellIntValue(row, Columns.UserID, out int UserID))
                return;

            using (frmPersonCardInfo frm = new frmPersonCardInfo(UserID))
                frm.ShowDialog();
        }
        private void toolStripMenuItemSMS_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
        }
        private void toolStripMenuItemEmail_Click(object sender, EventArgs e)
        {
            clsMessages.ShowInfo("ستضاف الميزة قريبا");
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
                    clsEventLogger.LogException("ctrlUsers.LoadDataAsync", ex);
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
                var result = await _UserService.GetUsersPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListUsers.DataSource = null;
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

                dgvListUsers.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListUsers.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlUsers._ApplyPagingAsync", ex);
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
            if (dgvListUsers.DataSource == null || dgvListUsers.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.UserID, "المعرف");
            _SetColumnHeader(Columns.FullName, "الاسم الكامل");
            _SetColumnHeader(Columns.Age, "العمر");
            _SetColumnHeader(Columns.Gender, "الجنس");
            _SetColumnHeader(Columns.Email, "البريد");
            _SetColumnHeader(Columns.Phone, "الهاتف");
            _SetColumnHeader(Columns.Address, "العنوان");
            _SetColumnHeader(Columns.Description, "الصلاحية");
            _SetColumnHeader(Columns.UserName, "اسم المستخدم");
            _SetColumnHeader(Columns.IsActive, "نشط ؟");
            _SetColumnHeader(Columns.IsLockedOut, "مغلق ؟");
            _SetColumnHeader(Columns.FailedLoginAttempts, "عدد المحاولات الفاشلة");
            _SetColumnHeader(Columns.LastFailedLoginDate, "تاريخ اخر فشل");
            _SetColumnHeader(Columns.CreatedDate, "تاريخ الإنشاء");
            _SetColumnHeader(Columns.CreatedByUserID, "المنشئ");
            _SetColumnHeader(Columns.EditedDate, "تاريخ التعديل");
            _SetColumnHeader(Columns.EditedByUserID, "المعدل");
            _SetColumnHeader(Columns.PersonID, "المعرف الشخصي");

            _HideColumn(Columns.IsLockedOut);
            _HideColumn(Columns.FailedLoginAttempts);
            _HideColumn(Columns.LastFailedLoginDate);
            _HideColumn(Columns.CreatedDate);
            _HideColumn(Columns.CreatedByUserID);
            _HideColumn(Columns.EditedDate);
            _HideColumn(Columns.EditedByUserID);
            _HideColumn(Columns.PersonID);


        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListUsers.Columns.Contains(columnName))
                dgvListUsers.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListUsers.Columns.Contains(columnName))
                dgvListUsers.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListUsers.Rows.Count == 0;
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
            var data = dgvListUsers.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "الأشخاص");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف", typeof(int));
            exportTable.Columns.Add("الاسم الكامل", typeof(string));
            exportTable.Columns.Add("العمر", typeof(int));
            exportTable.Columns.Add("الجنس", typeof(string));
            exportTable.Columns.Add("البريد", typeof(string));
            exportTable.Columns.Add("الهاتف", typeof(string));
            exportTable.Columns.Add("العنوان", typeof(string));
            exportTable.Columns.Add("الصلاحية", typeof(string));
            exportTable.Columns.Add("اسم المستخدم", typeof(string));
            exportTable.Columns.Add("نشط ؟", typeof(string));
            exportTable.Columns.Add("مغلق ؟", typeof(bool));
            exportTable.Columns.Add("عدد المحاولات الفاشلة", typeof(int));
            exportTable.Columns.Add("تاريخ اخر فشل", typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء", typeof(string));
            exportTable.Columns.Add("المنشئ", typeof(int));
            exportTable.Columns.Add("تاريخ التعديل", typeof(string));
            exportTable.Columns.Add("المعدل", typeof(int));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"] = row[Columns.UserID];
                newRow["الاسم الكامل"] = row[Columns.FullName];
                newRow["العمر"] = row[Columns.Age];
                newRow["الجنس"] = row[Columns.Gender];
                newRow["البريد"] = row[Columns.Email];
                newRow["الهاتف"] = row[Columns.Phone];
                newRow["العنوان"] = row[Columns.Address];
                newRow["الصلاحية"] = row[Columns.Description];
                newRow["اسم المستخدم"] = row[Columns.UserName];
                newRow["نشط ؟"] = row[Columns.IsActive];
                newRow["مغلق ؟"] = row[Columns.IsLockedOut];
                newRow["عدد المحاولات الفاشلة"] = row[Columns.FailedLoginAttempts];
                newRow["تاريخ اخر فشل"] = row[Columns.LastFailedLoginDate];
                newRow["تاريخ الإنشاء"] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["المنشئ"] = row[Columns.CreatedByUserID];
                newRow["تاريخ التعديل"] = clsUtil.FormatDate(row[Columns.EditedDate]);
                newRow["المعدل"] = row[Columns.EditedByUserID];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;

        }
        private bool _TryGetSelectedUserId(out int UserId)
        {
            UserId = 0;

            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return false;

            return _TryGetCellIntValue(row, Columns.UserID, out UserId);
        }
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListUsers.CurrentRow;

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
            if (_currentColumn == Columns.IsLockedOut)
                return cbYesOrNo.SelectedIndex == (int)enYesOrNo.Yes ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.UserID: return Columns.UserID;
                case enFilter.UserName: return Columns.UserName;
                case enFilter.FullName: return Columns.FullName;
                case enFilter.Phone: return Columns.Phone;
                case enFilter.Email: return Columns.Email;
                case enFilter.Address: return Columns.Address;
                case enFilter.Age: return Columns.Age;
                case enFilter.Gender: return Columns.Gender;
                case enFilter.IsActive: return Columns.IsActive;
                case enFilter.IsLocked: return Columns.IsLockedOut;
                case enFilter.FailedAttempts: return Columns.FailedLoginAttempts;
                case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                case enFilter.EditedByUserID: return Columns.EditedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }

        }
    }
}