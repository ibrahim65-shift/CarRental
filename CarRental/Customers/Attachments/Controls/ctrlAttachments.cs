using CarRental.Customers.Attachments.Controls;
using CarRental.Customers.Attachments.Forms;
using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Services.Attachments;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Customers.Attachments.Controls
{
    public partial class ctrlAttachments : UserControl
    {
        private static class Columns
        {
            public const string AttachmentID= nameof(clsAttachmentDto.AttachmentID);
            public const string RelatedTable = nameof(clsAttachmentDto.RelatedTable);
            public const string RelatedID= nameof(clsAttachmentDto.RelatedID);
            public const string FileName= nameof(clsAttachmentDto.FileName);
            public const string FilePath= nameof(clsAttachmentDto.FilePath);
            public const string MimeType= nameof(clsAttachmentDto.MimeType);
            public const string FileSizeKB= nameof(clsAttachmentDto.FileSizeKB);
            public const string IsPrimary= nameof(clsAttachmentDto.IsPrimary);
        }

        private enum enFilter
        {
            AttachmentID, RelatedTable, RelatedID,
            IsPrimary
        }
        private enum enYesOrNo { Yes = 0, No = 1 };

        private bool _isUpdatingPageCombo = false;

        private string _currentSearchText = null;
        private string _currentColumn = null;

        private readonly clsDebouncer _debouncer = new clsDebouncer();
        private int _searchRequestId = 0;
        private bool _columnsInitialized = false;

        private readonly frmMain _frmMain;
        private readonly clsAttachmentService _Attachmentservice;

        private int _currentPage = 1;
        private int _totalPages = 0;

  
        public event Action DataRefreshed;

        public ctrlAttachments(frmMain frmMain)
        {
            InitializeComponent();
            _Attachmentservice = new clsAttachmentService();
            _frmMain = frmMain ?? throw new ArgumentNullException(nameof(frmMain));
        }

        public async Task RefreshDataAsync()
        {
            _currentPage = 1;
            await _LoadDataAsync();
        }
        private async void ctrlAttachments_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilter.SelectedIndex = 0;
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlAttachments.ctrlAttachments_Load", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditAttachment frm = new frmAddEditAttachment())
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
                clsEventLogger.LogException("ctrlAttachments.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedAttachmentId(out int AttachmentId))
                return;

            try
            {
                using (frmAddEditAttachment frm = new frmAddEditAttachment(AttachmentId))
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
                clsEventLogger.LogException("ctrlAttachments.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedAttachmentId(out int AttachmentId))
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

                var result = await _Attachmentservice.DeleteAsync(AttachmentId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف المرفق الذي يحمل الرقم التعريفي '{AttachmentId}' بنجاح");
                    await RefreshDataAsync();
                    DataRefreshed?.Invoke();
                }
                else
                {
                    clsMessages.ShowError(result.ErrorMessage ?? "حدث خطأ أثناء حذف المرفق");
                }
            }
            catch (Exception ex)
            {
                clsMessages.ShowError();
                clsEventLogger.LogException("ctrlAttachments.btnDelete_Click", ex);
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
                clsEventLogger.LogException("ctrlAttachments.txtSearch_TextChanged", ex);
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
                clsEventLogger.LogException("ctrlAttachments.txtSearch_KeyPress", ex);
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
                clsEventLogger.LogException("ctrlAttachments.btnRefresh_Click", ex);
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
                clsEventLogger.LogException("ctrlAttachments.btnSearch_Click", ex);
                clsMessages.ShowError();
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
                clsEventLogger.LogException("ctrlAttchments.cbYesOrNo_SelectedIndexChanged", ex);
                clsMessages.ShowError();
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentColumn = _GetColumnNameForFilter((enFilter)cbFilter.SelectedIndex);

            if (_currentColumn == Columns.IsPrimary)
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
                clsEventLogger.LogException("ctrlAttachments.cbPageNumber_SelectedIndexChanged", ex);
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
                clsEventLogger.LogException("ctrlAttachments.btnPrevious_Click", ex);
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
                clsEventLogger.LogException("ctrlAttachments.btnNext_Click", ex);
                clsMessages.ShowError();
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
                    clsEventLogger.LogException("ctrlAttachments.LoadDataAsync", ex);
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
                var result = await _Attachmentservice.GetAttachmentsPageAsync(
                    _currentPage,
                    Properties.Settings.Default.NumberOfItems,
                    _currentColumn,
                    _currentSearchText);


                if (!result.Success || result.Data == null)
                {
                    dgvListAttachments.DataSource = null;
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

                dgvListAttachments.DataSource = result.Data.Data;

                _UpdatePagingControls(newTotalPages);

                if (!_columnsInitialized && dgvListAttachments.Rows.Count > 0)
                {
                    _InitializeColumns();
                    _columnsInitialized = true;
                }


                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlAttachments._ApplyPagingAsync", ex);
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
            if (dgvListAttachments.DataSource == null || dgvListAttachments.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.AttachmentID           , "المعرف"      );
            _SetColumnHeader(Columns.RelatedTable	  		, "جدول مرتبط"  );
            _SetColumnHeader(Columns.RelatedID				, "معرف مرتبط"  );
            _SetColumnHeader(Columns.FileName			    , "اسم الملف"   );
            _SetColumnHeader(Columns.FilePath				, "مسار الملف"  );
            _SetColumnHeader(Columns.MimeType				, "نوع الملف"   );
            _SetColumnHeader(Columns.FileSizeKB			    , "حجم الملف"   );
            _SetColumnHeader(Columns.IsPrimary              , "أساسي ؟"     );
            //_SetColumnHeader(Columns.IsDeleted			, "محذوف"       );
            //_SetColumnHeader(Columns.CreatedDate			, "تاريخ الإنشاء");
            //_SetColumnHeader(Columns.CreatedByUserID		, "المنشئ"      );


            //_HideColumn(Columns.IsDeleted);
            //_HideColumn(Columns.CreatedDate);
            //_HideColumn(Columns.CreatedByUserID);

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvListAttachments.Columns.Contains(columnName))
                dgvListAttachments.Columns[columnName].HeaderText = headerText;
        }
        private void _HideColumn(string columnName)
        {
            if (dgvListAttachments.Columns.Contains(columnName))
                dgvListAttachments.Columns[columnName].Visible = false;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvListAttachments.Rows.Count == 0;
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
            var data = dgvListAttachments.DataSource as List<clsAttachmentDto>;
            
            if (data == null || data.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "المرفقات");
        }
        private DataTable _CreateOptimizedExportData(List<clsAttachmentDto> source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"            ,typeof(int));
            exportTable.Columns.Add("جدول مرتبط"        , typeof(string));
            exportTable.Columns.Add("معرف مرتبط"        , typeof(int));
            exportTable.Columns.Add("اسم الملف"         , typeof(string));
            exportTable.Columns.Add("مسار الملف"        , typeof(string));
            exportTable.Columns.Add("نوع الملف"         , typeof(string));
            exportTable.Columns.Add("حجم الملف"         , typeof(int));
            exportTable.Columns.Add("أساسي ؟"           , typeof(bool));
          
            exportTable.BeginLoadData();

            foreach (var item in source)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"            ] = item.AttachmentID;
                newRow["جدول مرتبط"        ] = item.RelatedTable;
                newRow["معرف مرتبط"        ] = item.RelatedID;
                newRow["اسم الملف"         ] = item.FileName;
                newRow["مسار الملف"        ] = item.FilePath;
                newRow["نوع الملف"         ] = item.MimeType;
                newRow["حجم الملف"         ] = item.FileSizeKB;
                newRow["أساسي ؟"           ] = item.IsPrimary;


                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
        private bool _TryGetSelectedAttachmentId(out int AttachmentId)
        {
            AttachmentId = 0;
            if (dgvListAttachments.CurrentRow == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            if (dgvListAttachments.CurrentRow.DataBoundItem is clsAttachmentDto attachment)
            {
                AttachmentId = attachment.AttachmentID;
                return true;
            }
            return false;
        }
        private async Task<bool> _CheckDatabaseConnection()
        {
            return await clsUtil.CheckDatabaseConnection();
        }
        private string _GetFilterValue()
        {
            if (_currentColumn == Columns.IsPrimary)
                return cbYesOrNo.SelectedIndex == (int)enYesOrNo.Yes ? "1" : "0";


            return txtSearch.Text.Trim();
        }
        private string _GetColumnNameForFilter(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.AttachmentID: return Columns.AttachmentID;
                case enFilter.RelatedTable: return Columns.RelatedTable;
                case enFilter.RelatedID: return Columns.RelatedID;
                case enFilter.IsPrimary: return Columns.IsPrimary;
                //case enFilter.CreatedByUserID: return Columns.CreatedByUserID;
                default: throw new ArgumentOutOfRangeException();
            }
        }

      
    }
}