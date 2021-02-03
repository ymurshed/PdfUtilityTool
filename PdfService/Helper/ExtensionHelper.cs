using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfService.Constants;

namespace PdfService.Helper
{
    public static class ExtensionHelper
    {
        public static List<string> PdfExt = new List<string> { ".pdf" };
        public static List<string> DocExt = new List<string> { ".doc", ".docx", ".xls", ".xlsx" };
        public static List<string> ImageExt = new List<string> { ".jpeg", ".jpg", ".tiff", ".tif" };

        public static List<string> SupportedExt = new List<string>
        {
            ".txt", ".html",
            ".xls", ".xlsx", ".csv",
            ".doc", ".docx", ".rtf",
            ".jpeg", ".jpg", ".png", ".gif", ".giff", ".tif", ".tiff", ".ico", ".bmp"
        };

        public static bool IsImageFile(this string file)
        {
            var ext = Path.GetExtension(file)?.ToLower();
            return !string.IsNullOrWhiteSpace(ext) && ImageExt.Contains(ext);
        }

        public static bool IsDocFile(this string file)
        {
            var ext = Path.GetExtension(file)?.ToLower();
            return !string.IsNullOrWhiteSpace(ext) && DocExt.Contains(ext);
        }

        public static bool IsPdfFile(this string file)
        {
            var ext = Path.GetExtension(file)?.ToLower();
            return !string.IsNullOrWhiteSpace(ext) && PdfExt.Contains(ext);
        }

        public static bool IsSupportedFile(this string file)
        {
            var ext = Path.GetExtension(file)?.ToLower();
            return !string.IsNullOrWhiteSpace(ext) && SupportedExt.Contains(ext);
        }

        public static bool IsNonSupportedFile(this string file)
        {
            return !IsSupportedFile(file) && !IsPdfFile(file);
        }

        public static bool IsFileEmpty(this string file)
        {
            return File.Exists(file) && new FileInfo(file).Length == 0;
        }

        public static bool IsFolderEmpty(this string folder, string ext = InfoConstants.StarTxtExtension)
        {
            var count = 0;
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, ext, SearchOption.AllDirectories).ToList();
                count = files.Count;
            }
            return count == 0;
        }

        public static bool IsFileCountExceeded(this int totalFileCount, string fileType)
        {
            var fileCountExceeded = false;
            if (totalFileCount > AppSettingsConstants.MaxFileProcessCount)
            {
                fileCountExceeded = true;
                var maxFileCountExceedMsg = fileType.Equals(InfoConstants.AllButPdf)
                    ? string.Format(InfoConstants.MaxAllowedFileCountExceed, totalFileCount, AppSettingsConstants.MaxFileProcessCount)
                    : string.Format(InfoConstants.MaxFileCountExceed, fileType, totalFileCount, AppSettingsConstants.MaxFileProcessCount);

                FileLogger.SetLog(maxFileCountExceedMsg);
            }
            return fileCountExceeded;
        }
    }
}