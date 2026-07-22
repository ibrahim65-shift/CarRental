

namespace CarRental_Buisness.Models.Attachments
{
    public class clsAttachmentAddNewModel
    {
        public string RelatedTable { get; set; }
        public int RelatedID { get; set; }
        public string FileName { get; set; }
        public string SourceFilePath { get; set; } 
        public bool IsPrimary { get; set; }
    }
}
