using CarRental.Helper;
using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Attachments;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Attachments.Forms
{
    public partial class frmAddEditAttachment : Form
    {
        private enum enMode { AddNew , Update}

        private readonly clsAttachmentService _attachmentService;
        private  Dictionary<string, Control> _validationControls = new Dictionary<string, Control>();

        private enMode _mode;

        private int? _attachmentId;
        private readonly string _relatedTable;
        private readonly int _relatedId;
        private string _originalFilePath;
        private bool _isSaving;
        private bool _fileChanged;
        public frmAddEditAttachment(clsAttachmentService attachmentService,int? attachmentId , string relatedTable , int relatedId)
        {
            InitializeComponent();
            _attachmentService = attachmentService;
            _attachmentId = attachmentId;
            _relatedTable = relatedTable;
            _relatedId = relatedId;

            if(_attachmentId.HasValue)
            {
                _mode = enMode.Update;
                this.Text = "تعديل مرفق";
            }
            else
            {
                _mode = enMode.AddNew;
                this.Text = "إضافة مرفق";
            }

            _validationControls = new Dictionary<string, Control>
            {
                {"FilePath",txtFilePath },
                {"FileName",txtFileName }
            };

            openFileDialog1.Title = "اختر ملف";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
        }

        private async void frmAddEditAttachment_Load(object sender, EventArgs e)
        {
            try
            {
                if (_mode == enMode.Update)
                    await _LoadAttachmentDataAsync();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditAttachment.frmAddEditAttachment_Load", ex);
                clsMessages.ShowError();
            }
        }
        private void linkLabelBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(!string.Equals(_originalFilePath,openFileDialog1.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    _fileChanged = true;
                }

                txtFilePath.Text = openFileDialog1.FileName;
                txtFileName.Text = string.IsNullOrWhiteSpace(txtFileName.Text) ?
                                   Path.GetFileNameWithoutExtension(txtFilePath.Text) 
                                   : txtFileName.Text.Trim();
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

                if (_mode == enMode.AddNew)
                   await _SaveAddNewAsync();
                else
                    await _SaveUpdateAsync();

            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("frmAddEditAttachment.btnSave_Click", ex);
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

        // ================ METHODS ===================

        private async Task _LoadAttachmentDataAsync()
        {
            if (!_attachmentId.HasValue)
                throw new InvalidOperationException("معرف المرفق غير معروف");

            var result = await _attachmentService.GetAttachmentByIDAsync(_attachmentId.Value);
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                Close();
                return;
            }

            var attachData = result.Data;

            _originalFilePath = attachData.FilePath;
            txtFileName.Text = attachData.FileName;
            txtFilePath.Text = attachData.FilePath;
            chkIsPrimary.Checked = attachData.IsPrimary;

            _fileChanged = false;
        }
        private async Task _SaveAddNewAsync()
        {
            var result = await _attachmentService.AddNewAsync(_BuildAttachmentAddNewModel());
            if(!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم إضافة المرفق بنجاح. والرقم التعريفي هو {result.Data.AttachmentID}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private async Task _SaveUpdateAsync()
        {
            var result = await _attachmentService.UpdateAsync(_attachmentId.Value,_BuildAttachmentUpdateModel());
            if (!result.Success)
            {
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                    clsMessages.ShowError(result.ErrorMessage);

                _ApplyValidationErrors(result.Validation);
                return;
            }

            clsMessages.ShowSuccess($"تم تعديل بيانات المرفق بنجاح. للرقم التعريفي {_attachmentId}");

            DialogResult = DialogResult.OK;
            Close();
        }
        private clsAttachmentAddNewModel _BuildAttachmentAddNewModel()
        {
            return new clsAttachmentAddNewModel
            {
                RelatedTable = _relatedTable,
                RelatedID = _relatedId,
                FileName = string.IsNullOrWhiteSpace(txtFileName.Text) ? Path.GetFileNameWithoutExtension(openFileDialog1.FileName) : txtFileName.Text.Trim(),
                SourceFilePath = txtFilePath.Text.Trim(),
                IsPrimary = chkIsPrimary.Checked
            };
        }
        private clsAttachmentUpdateModel _BuildAttachmentUpdateModel()
        {
            return new clsAttachmentUpdateModel
            {
                FileName = txtFileName.Text.Trim(),
                SourceFilePath = _fileChanged ? txtFilePath.Text.Trim() :null,
                IsPrimary = chkIsPrimary.Checked
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
                if (_validationControls.TryGetValue(error.FieldName, out Control control))
                    errorProvider1.SetError(control, error.Message);
                else
                    clsMessages.ShowError(error.Message);
            }
        }

    }
}
