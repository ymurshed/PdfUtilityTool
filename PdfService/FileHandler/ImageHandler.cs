using System;
using System.Drawing;
using System.IO;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace PdfService.FileHandler
{
    public class ImageHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            var ext = Path.GetExtension(source)?.ToLower();
            var useSyncfusion = ext == ".ico"; 

            if (useSyncfusion) ConvertToPdfUsingSyncfusion(source, destination); 
            else ConvertToPdfUsingiTextSharp(source, destination);
        }

        private static void ConvertToPdfUsingiTextSharp(string source, string destination)
        {
            try
            {
                iTextSharp.text.Rectangle pageSize;

                using (var srcImage = new Bitmap(source))
                {
                    pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
                }

                using (var ms = new MemoryStream())
                {
                    var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                    document.Open();

                    var image = iTextSharp.text.Image.GetInstance(source);
                    document.Add(image);
                    document.Close();

                    File.WriteAllBytes(destination, ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(string.Format(ExceptionConstants.ConvertToPdf, source, ex.Message));
            }
        }

        // Sometimes images gets cut off using Syncfusion
        private static void ConvertToPdfUsingSyncfusion(string source, string destination)
        {
            try
            {
                var doc = new PdfDocument();
                var page = doc.Pages.Add();
                var graphics = page.Graphics;
                var image = new PdfBitmap(source);

                graphics.DrawImage(image, 0, 0);
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