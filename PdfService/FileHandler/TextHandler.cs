using System;
using System.Drawing;
using System.IO;
using System.Text;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace PdfService.FileHandler
{
    public class TextHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            try
            {
                var document = new PdfDocument();
                var page = document.Pages.Add();
                var graphics = page.Graphics;
                var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                var reader = new StreamReader(source, Encoding.ASCII);
                var text = reader.ReadToEnd();
                reader.Close();

                var format = new PdfStringFormat
                {
                    Alignment = PdfTextAlignment.Justify,
                    LineAlignment = PdfVerticalAlignment.Top,
                    ParagraphIndent = 15f
                };

                graphics.DrawString(text, font, PdfBrushes.Black,
                    new RectangleF(new PointF(0, 0), page.GetClientSize()), format);
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