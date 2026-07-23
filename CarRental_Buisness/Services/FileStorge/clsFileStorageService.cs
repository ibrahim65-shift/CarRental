using SharedClass;
using System;
using System.IO;
using System.Web;

namespace CarRental_Buisness.Services.FileStorge
{
    public static class clsFileStorageService
    {
        private const string AttachmentsFolder = "Attachments";

        private static string GetAttachmentsFolder()
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AttachmentsFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
        private static string GenerateFileName(string extension)
        {
            return $"{Guid.NewGuid()}{extension}";
        }
        public static string SaveFile(string sourceFilePath)
        {
            if(string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException(nameof(sourceFilePath));

            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("الملف غير موجود", sourceFilePath);

            string extension = Path.GetExtension(sourceFilePath);
            string newFileName = GenerateFileName(extension);
            string destinationPath = Path.Combine(GetAttachmentsFolder(), newFileName);

            File.Copy(sourceFilePath, destinationPath);

            return destinationPath;
        }
        public static bool DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            try
            {
                if (!File.Exists(filePath))
                    return false;

                File.Delete(filePath);
                return true;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("clsFileStorageService.DeleteFile", ex);
                return false;
            }
        }
        public static int GetFileSizeKB(string filePath)
        {
            return (int)Math.Ceiling(new FileInfo(filePath).Length / 1024.0);
        }
        public static string GetMimeType(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return "application/octet-stream";

            return MimeMapping.GetMimeMapping(filePath);
        }
    }
}
