using System;
using System.Drawing;
using PdfService.Constants;
using PdfService.Helper;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using Syncfusion.XlsIO;

namespace PdfService.FileHandler
{
    public class CsvHandler : IFileHandler
    {
        public void ConvertToPdf(string source, string destination)
        {
            try
            {
                var excelEngine = new ExcelEngine();
                var application = excelEngine.Excel;
                var workbook = application.Workbooks.Open(source);
                var worksheet = workbook.Worksheets[0];
                var document = new PdfDocument();
                var page = document.Pages.Add();
                var pdfGrid = new PdfGrid();
                pdfGrid.Columns.Add(worksheet.UsedRange.LastColumn);
                pdfGrid.Headers.Add(1);
                var pdfGridHeader = pdfGrid.Headers[0];

                for (var i = 0; i < worksheet.UsedRange.LastColumn; i++)
                {
                    pdfGridHeader.Cells[i].Value = worksheet.UsedRange.Columns[i].DisplayText;
                }

                //Add rows
                for (var row = 2; row <= worksheet.UsedRange.LastRow; row++)
                {
                    var pdfGridRow = pdfGrid.Rows.Add();
                    for (var col = 1; col <= worksheet.UsedRange.LastColumn; col++)
                    {
                        var text = worksheet[row, col].Text;
                        pdfGridRow.Cells[col - 1].Value = text;
                    }
                }

                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable5DarkAccent6);
                pdfGrid.Draw(page, PointF.Empty);
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