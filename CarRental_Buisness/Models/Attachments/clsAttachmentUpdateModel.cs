using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Models.Attachments
{
    public class clsAttachmentUpdateModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; } // allows null
        public string MimeType { get; set; } // allows null
        public int? FileSizeKB { get; set; }
        public bool IsPrimary { get; set; }
    }
}
