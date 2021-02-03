using System.IO;

namespace PdfService.FileHandler
{
    public class FileHandlerFactory
    {
        public static IFileHandler GetFileHandler(string path)
        {
            IFileHandler fileHandler = null;
            var extension = Path.GetExtension(path)?.ToLower();

            switch (extension)
            {
                case ".xls":
                case ".xlsx":
                    fileHandler = new ExcelHandler();
                    break;

                case ".doc":
                case ".docx":
                    fileHandler = new DocumentHandler();
                    break;

                case ".rtf":
                    fileHandler = new RtfHandler();
                    break;

                case ".txt":
                    fileHandler = new TextHandler();
                    break;

                case ".csv":
                    fileHandler = new CsvHandler();
                    break;

                case ".html":
                    fileHandler = new HtmlHandler();
                    break;

                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".giff":
                case ".tif":
                case ".tiff":
                case ".ico":
                case ".bmp":
                    fileHandler = new ImageHandler();
                    break;
            }

            return fileHandler;
        }
    }
}