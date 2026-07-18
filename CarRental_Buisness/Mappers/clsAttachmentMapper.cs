using CarRental_Buisness.Models.Attachments;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsAttachmentMapper
    {
        public static clsAttachmentDto ToDto(clsAttachmentsEntities entities)
        {
            return new clsAttachmentDto
            {
                AttachmentID = entities.AttachmentID,
                RelatedTable = entities.RelatedTable,
                RelatedID = entities.RelatedID,
                FileName = entities.FileName,
                FilePath = entities.FilePath,
                MimeType = entities.MimeType,
                FileSizeKB = entities.FileSizeKB,
                IsPrimary = entities.IsPrimary
            };
        }
    }
}
