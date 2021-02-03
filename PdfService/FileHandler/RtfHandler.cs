using System;
using System.IO;
using System.Text;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace PdfService.FileHandler
{
    public class RtfHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            try
            {
                var doc = new PdfDocument();
                var page = doc.Pages.Add();
                var bounds = page.GetClientSize();
                var reader = new StreamReader(source, Encoding.ASCII);
                var text = reader.ReadToEnd();
                reader.Close();

                var imageMetafile = (PdfMetafile) PdfImage.FromRtf(text, bounds.Width, PdfImageType.Metafile);
                var format = new PdfMetafileLayoutFormat {SplitTextLines = true};
                imageMetafile.Draw(page, 0, 0, format);

                doc.Save(destination);
                doc.Close(true);
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }
    }
}