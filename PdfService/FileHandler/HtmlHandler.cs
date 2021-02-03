using System;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.HtmlConverter;

namespace PdfService.FileHandler
{
    public class HtmlHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            try
            {
                var htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
                var settings = new WebKitConverterSettings {WebKitPath = @"/QtBinaries/"};
                htmlConverter.ConverterSettings = settings;
                var document = htmlConverter.Convert("https://www.google.com");

                document.Save(destination);
                document.Close(true);
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }
    }
}