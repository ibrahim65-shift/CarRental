using CarRental.Attachments.Forms;
using CarRental.Customers.CustomersList.Forms;
using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Services.Attachments;
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

namespace CarRental.Attachments.Controls
{
    public partial class ctrlRelatedAttachments : UserControl,IRefreshable
    {
        private readonly Dictionary<string, string> _columnHeader = new Dictionary<string, string>
        {
            {"AttachmentID" ,"المعرف" },
            {"RelatedTable" ,"الجدول المرتبط" },
            {"RelatedID" ,"المعرف المرتبط" },
            {"FileName" ,"الاسم" },
            {"FilePath" ,"المسار" },
            {"MimeType" ,"النوع" },
            {"FileSizeKB" ,"الحجم" },
            {"IsPrimary" ,"أساسي ؟" },
            {"IsDeleted" ,"محذوف ؟" },
            {"CreatedDate" ,"تاريخ الإنشاء" },
            {"CreatedByUserID" ,"المنشئ" },
        };

        private readonly HashSet<string> _hideColumns = new HashSet<string>
        {
            "IsDeleted" ,
            "CreatedDate" ,
            "CreatedByUserID"
        };


        private readonly clsAttachmentService _attachmentService;
        public event Action DataRefreshed;

        private string _relatedTable;
        private int _relatedId;

        public ctrlRelatedAttachments(string relatedTable , int relatedId , string attachOwner)
        {
            InitializeComponent();
            _attachmentService = new clsAttachmentService();
            _relatedTable = relatedTable;
            _relatedId = relatedId;

            lblAttachmentsOwner.Text = attachOwner;
        }

        public async Task RefreshDataAsync()
        {
            await _LoadDataAsync();
        }
        private async void ctrlRelatedAttachments_Load(object sender, EventArgs e)
        {
            try
            {
                await _LoadDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlRelatedAttachments.ctrlRelatedAttachments_Load", ex);
                _ShowServerErrorState();
            }
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddEditAttachment frm = new frmAddEditAttachment(_attachmentService, null, _relatedTable, _relatedId))
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
                clsEventLogger.LogException("ctrlRelatedAttachments.btnAdd_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, "AttachmentID", out int attachmentId))
                return;

            try
            {
                using (frmAddEditAttachment frm = new frmAddEditAttachment(_attachmentService, attachmentId, _relatedTable, _relatedId))
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
                clsEventLogger.LogException("ctrlRelatedAttachments.btnEdit_Click", ex);
                clsMessages.ShowError();
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_TryGetSelectedRow(out DataGridViewRow row))
                return;

            if (!_TryGetCellValue<int>(row, "AttachmentID", out int attachmentId))
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

                var result = await _attachmentService.DeleteAsync(attachmentId);

                if (result.Success)
                {
                    clsMessages.ShowSuccess($"تم حذف المرفق الذي يحمل الرقم التعريفي '{attachmentId}' بنجاح");
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
                clsEventLogger.LogException("ctrlRelatedAttachments.btnDelete_Click", ex);
            }
        }


        // ================ METHODS =================

        private async Task _LoadDataAsync()
        {
            var result = await _attachmentService.GetAttachmentsByTableAndIDAsync(_relatedTable, _relatedId);
            if(!result.Success)
            {
                lblAttachmentsCount.Text = "0";
                dgvListAttachments.DataSource = null;
                _ShowEmptyDataState();
                return;
            }


            dgvListAttachments.DataSource = result.Data.Data;
            _InitializeColumns();
            _ShowEmptyDataState();
            lblAttachmentsCount.Text = result.Data.Data.Count.ToString();
        }
        private void _InitializeColumns()
        {
            if (dgvListAttachments.DataSource == null || dgvListAttachments.Rows.Count == 0)
                return;

            foreach(var col in _columnHeader)
            {
                if(_hideColumns.Contains(col.Key))
                {
                    _HideColumn(col.Key);
                    continue;
                }

                _SetColumnHeader(col.Key, col.Value);
            }
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
        private bool _TryGetSelectedRow(out DataGridViewRow row)
        {
            row = dgvListAttachments.CurrentRow;

            if (row == null)
            {
                clsMessages.ShowError("الرجاء اختيار صف أولا");
                return false;
            }

            return true;
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
    }
}
