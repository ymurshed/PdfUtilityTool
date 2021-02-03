using System;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.XlsIO;

namespace PdfService.FileHandler
{
    public class ExcelHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            try
            {
                using (var excelEngine = new ExcelEngine())
                {
                    var application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    var workbook = application.Workbooks.Open(source, ExcelOpenType.Automatic);

                    var converter = new ExcelToPdfConverter(workbook);
                    var pdfDocument = converter.Convert();
                    pdfDocument.Save(destination);
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }
    }
}