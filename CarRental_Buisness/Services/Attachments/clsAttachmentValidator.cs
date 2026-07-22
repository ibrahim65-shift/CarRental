using CarRental_Buisness.Models.Attachments;
using CarRental_Buisness.Results;


namespace CarRental_Buisness.Services.Attachments
{
    public class clsAttachmentValidator
    {
        private void CommonValidate(clsValidationResult list , string FileName , string FilePath , string MimeType , int? FileSizeKB)
        {
            if(string.IsNullOrWhiteSpace(FileName))
            {
                list.Add("FileName", "اسم الملف لايمكن أن يكون فارغ");
            }
            else if (FileName.Length > 255)
            {
                list.Add("FileName", "اسم الملف تجاوز الحد المسموح به");
            }

            if(!string.IsNullOrWhiteSpace(FilePath))
            {
                if(FilePath.Length > 1000)
                {
                    list.Add("FilePath", "مسار الملف تجاوز الحد المسموح به");
                }
            }

            if(!string.IsNullOrWhiteSpace(MimeType))
            {
                if(MimeType.Length > 100)
                {
                    list.Add("MimeType", "نوع الملف تجاوز الحد المسموح به");
                }
            }

            if (FileSizeKB.HasValue)
            {
                if (FileSizeKB < 0)
                    list.Add("FileSizeKB", "الحجم غير صالح");

                if (FileSizeKB > 10240)
                    list.Add("FileSizeKB", "الملف أكبر من 10MB");
            }

        }
        public clsValidationResult ValidateAddNewAsync(clsAttachmentAddNewModel model
            ,string mimeType , int fileSizeKB , bool recordExists)
        {
            var list = new clsValidationResult();

            CommonValidate(list, model.FileName, model.SourceFilePath, mimeType, fileSizeKB);

            if (string.IsNullOrWhiteSpace(model.RelatedTable))
            {
                list.Add("RelatedTable", "اسم الجدول الخاص بالمرفق لايمكن أن يكون فارغ");
            }
            else if (model.RelatedTable.Length > 128)
            {
                list.Add("RelatedTable", "اسم الجدول الخاص بالمرفق تجاوز الحد المسموح به");
            }

            if(!recordExists)
            {
                list.Add("RelatedTable", "اسم الجدول الخاص بالمرفق غير مسموح به");
                list.Add("RelatedID", "معرف الجدول الخاص بالمرفق غير صالح");
            }

             return list;
        }
        public clsValidationResult ValidateUpdateAsync(clsAttachmentUpdateModel model, string mimeType, int? fileSizeKB)
        {
            var list = new clsValidationResult();

            CommonValidate(list, model.FileName, model.SourceFilePath, mimeType,  fileSizeKB);

            return list;
        }
        
    }
}
