using System;

namespace CarRental_Entities
{
    public class clsAttachmentsEntities
    {
        public int AttachmentID { get; set; }
        public string RelatedTable { get; set; }
        public int RelatedID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } // allows null
        public string MimeType { get; set; } // allows null
        public int? FileSizeKB { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }

    }
}
