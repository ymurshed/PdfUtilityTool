using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using PdfService.Constants;
using PdfService.FileHandler;
using PdfService.Helper;
using PdfService.Models;
using PdfService.Operations.Abstraction;

namespace PdfService.Operations.Implementation
{
    public class PdfConverter : AFileOperation, IPdfConverter
    {
        public int TotalNonPdfProcessed;
        public int TotalNonPdfCount { get; set; }
        public double Percent => TotalNonPdfCount == 0 ? 0 : (double)TotalNonPdfProcessed / TotalNonPdfCount * 100;

        public ConcurrentQueue<string> ErrorMessage;

        public PdfConverter()
        {
            ErrorMessage = new ConcurrentQueue<string>();
        }

        public override void DoTask()
        {
            try
            {
                ConvertToPdf();
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.PdfConverterDoTask + ex.Message);
            }
            finally
            {
                ErrorMessage = null;
            }
        }

        public override void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (TotalNonPdfCount == 0) return;
            var msg = string.Format(InfoConstants.FileConvertedToPdf, TotalNonPdfProcessed, TotalNonPdfCount, Percent.ToString("F")); 
            FileLogger.WriteToFile(msg);
        }

        public void ConvertToPdf()
        {
            var startTime = DateTime.Now;
            var files = GetFilesFromInputSource(InfoConstants.AllButPdf);
            TotalNonPdfCount = files.Count;

            if (TotalNonPdfCount.IsFileCountExceeded(InfoConstants.AllButPdf)) return;
            FileLogger.SetLog(string.Format(InfoConstants.StartingFileConversion, files.Count));

            SetTimer();

            Parallel.For(0, TotalNonPdfCount, index =>
            {
                try
                {
                    var source = files[index];
                    var ext = Path.GetExtension(source);

                    if (!string.IsNullOrWhiteSpace(ext))
                    {
                        var destination = source.Replace(ext, InfoConstants.PdfExtension);

                        var fileHandler = FileHandlerFactory.GetFileHandler(source);
                        fileHandler.ConvertToPdf(source, destination);
                        AppInstanceModel.Instance.AllInputPdfFiles.Add(destination);

                        Interlocked.Increment(ref TotalNonPdfProcessed);
                        Console.WriteLine(InfoConstants.LogInParallelFor, index, TotalNonPdfProcessed);
                    }
                }
                catch (Exception ex)
                {
                    // Todo: will handle later
                    ErrorMessage.Enqueue(ex.Message);
                }
            });

            FileLogger.SetLog(string.Format(InfoConstants.FileConvertedToPdf, TotalNonPdfProcessed, TotalNonPdfCount, Percent.ToString("F")));
            FileLogger.SetLog(string.Format(InfoConstants.CompleteFileConversion));

            ClearTimer();

            var stopTime = DateTime.Now;
            CalculateTimeRequired(startTime, stopTime);

            SetSummary(InfoConstants.NonPdf, TotalNonPdfCount, TotalNonPdfProcessed);
        }
    }
}
