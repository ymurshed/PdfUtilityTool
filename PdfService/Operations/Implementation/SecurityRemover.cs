using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using PdfService.Constants;
using PdfService.Helper;
using PdfService.Operations.Abstraction;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

namespace PdfService.Operations.Implementation
{
    public class SecurityRemover: AFileOperation, ISecurityRemover
    {
        private readonly object _lock = new object();

        public int TotalPdfProcessed;
        public int TotalPdfCount => AllPdfs.Count;
        public double Percent => TotalPdfCount == 0 ? 0 : (double)TotalPdfProcessed / TotalPdfCount * 100;

        public List<string> AllPdfs => GetFilesFromInputSource();
        public string ErrorPdf { get; set; }

        public ConcurrentQueue<string> ErrorFiles;

        public SecurityRemover()
        {
            ErrorFiles = new ConcurrentQueue<string>();
        }

        public override void DoTask()
        {
            try
            {
                FileLogger.SetLog(string.Format(InfoConstants.StartingRemoveSecurity, InfoConstants.Pdf, InfoConstants.Pdf, TotalPdfCount));
                var startTime = DateTime.Now;

                SetTimer();
                ProcessSecuritySettingsInPdf();
                ClearTimer();
                
                var stopTime = DateTime.Now;
                FileLogger.SetLog(string.Format(InfoConstants.CompleteRemoveSecurity, TotalPdfCount, TotalPdfProcessed, TotalPdfCount - TotalPdfProcessed));
                CalculateTimeRequired(startTime, stopTime);
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.SecurityRemoverDoTask + ex.Message);
            }
        }

        public override void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (TotalPdfCount == 0) return;
            var msg = string.Format(InfoConstants.RemoveSecurityForPdf, TotalPdfProcessed, TotalPdfCount, Percent.ToString("F"));
            FileLogger.WriteToFile(msg);
        }

        public void ProcessSecuritySettingsInPdf()
        {
            FileLogger.SetLog(InfoConstants.ProcessingSecuritySettings);

            Parallel.For(0, TotalPdfCount, index =>
            {
                try
                {
                    var pdf = AllPdfs[index];

                    if (!string.IsNullOrWhiteSpace(pdf))
                    {
                        RemoveSecurityFromPdf(pdf);
                        Interlocked.Increment(ref TotalPdfProcessed);

                        Console.WriteLine(InfoConstants.LogInParallelFor, index, TotalPdfProcessed);
                    }
                }
                catch (Exception ex)
                {
                    ErrorFiles.Enqueue(ErrorPdf);
                }
            });

            // Handle log for files those have error during processing
            if (ErrorFiles.Count > 0)
            {
                var msg = string.Empty;
                const string header = ExceptionConstants.ErrorProcessingSecuritySettings;

                while (ErrorFiles.TryDequeue(out var errorFile))
                {
                    msg += errorFile + "\n";
                }

                // Remove last new line
                if (msg.Any()) msg.Remove(msg.Length - 1);

                FileLogger.SetLog(header + msg);
                ReportGenerator.GenerateErrorLog(msg);
            }

            FileLogger.WriteToFile(string.Format(InfoConstants.RemoveSecurityForPdf, TotalPdfProcessed, TotalPdfCount, Percent.ToString("F")));
            FileLogger.SetLog(InfoConstants.ProcessingSecuritySettingsDone);

            SetSummary(InfoConstants.Pdf, TotalPdfCount, TotalPdfProcessed);
        }

        public void RemoveSecurityFromPdf(string file)
        {
            try
            {
                var loadedDocument = new PdfLoadedDocument(file);
                loadedDocument.Security.Permissions = PdfPermissionsFlags.Default;
                loadedDocument.Security.UserPassword = string.Empty;
                loadedDocument.Security.OwnerPassword = string.Empty;
                loadedDocument.Save(file);
                loadedDocument.Close(true);
            }
            catch (Exception ex)
            {
                lock (_lock)
                {
                    ErrorPdf = file;
                    throw;
                }
            }
        }
    }
}
