using System;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.OfficeChartToImageConverter;
using WordToPDF;

namespace PdfService.FileHandler
{
    public class DocumentHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            ConvertToPdfUsingSyncfusion(source, destination);
        }

        private static void ConvertToPdfUsingSyncfusion(string source, string destination)
        {
            try
            {
                var wordDocument = new WordDocument(source, FormatType.Docx)
                {
                    ChartToImageConverter = new ChartToImageConverter()
                };

                var converter = new DocToPDFConverter();
                var pdfDocument = converter.ConvertToPDF(wordDocument);

                pdfDocument.Save(destination);
                pdfDocument.Close(true);
                wordDocument.Close();
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }

        private static void ConvertToPdfUsingWordToPdf(string source, string destination)
        {
            try
            {
                var word2Pdf = new Word2Pdf
                {
                    InputLocation = source,
                    OutputLocation = destination
                };
                word2Pdf.Word2PdfCOnversion();
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }
    }
}