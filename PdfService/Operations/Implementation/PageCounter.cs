using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using iTextSharp.text.pdf;
using PdfService.Constants;
using PdfService.Helper;
using PdfService.Models;
using PdfService.Operations.Abstraction;

namespace PdfService.Operations.Implementation
{
    public class PageCounter : AFileOperation, IPageCounter
    {
        private readonly object _lock = new object();

        public int TotalPdfProcessed;
        public int TotalPdfCount => AllPdfs.Count;
        public double Percent => TotalPdfCount == 0 ? 0 : (double)TotalPdfProcessed / TotalPdfCount * 100;

        public List<string> AllPdfs => GetFilesFromInputSource();
        public string ErrorPdf { get; set; }

        public ConcurrentQueue<PdfModel> ConcurrentPdfFileInfoModels;
        public ConcurrentQueue<string> ErrorFiles;

        public PageCounter()
        {
            ConcurrentPdfFileInfoModels = new ConcurrentQueue<PdfModel>();
            ErrorFiles = new ConcurrentQueue<string>();
        }

        public override void DoTask()
        {
            try
            {
                if (TotalPdfCount.IsFileCountExceeded(InfoConstants.Pdf)) return;
                FileLogger.SetLog(string.Format(InfoConstants.StartingPageCount, InfoConstants.Pdf, InfoConstants.Pdf, TotalPdfCount));
                var startTime = DateTime.Now;

                SetTimer();
                SetPdfFileInfoModels();
                ClearTimer();
                SavePdfFileInfoModels();

                var stopTime = DateTime.Now;
                FileLogger.SetLog(string.Format(InfoConstants.CompletePageCount, TotalPdfCount, TotalPdfProcessed, TotalPdfCount - TotalPdfProcessed));
                CalculateTimeRequired(startTime, stopTime);
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.PageCounterDoTask + ex.Message);
            }
            finally
            {
                ConcurrentPdfFileInfoModels = null;
            }
        }

        public override void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (TotalPdfCount == 0) return;
            var msg = string.Format(InfoConstants.PageCountForPdf, TotalPdfProcessed, TotalPdfCount, Percent.ToString("F"));
            FileLogger.WriteToFile(msg);
        }

        public int GetPdfPageCount(string file)
        {
            try
            {
                var item = new PdfReader(file);
                var pageCount = item.NumberOfPages;
                item.Dispose();
                return pageCount;
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

        #region Using Class Model
        public void SetPdfFileInfoModels()
        {
            FileLogger.SetLog(InfoConstants.CreatingPdfFileInfo);

            Parallel.For(0, TotalPdfCount, index =>
            {
                try
                {
                    var pdf = AllPdfs[index];

                    if (!string.IsNullOrWhiteSpace(pdf))
                    {
                        var fileName = Path.GetFileName(pdf);
                        var pageCount = GetPdfPageCount(pdf);
                        var pdfModel = new PdfModel(fileName, pageCount, pdf);

                        ConcurrentPdfFileInfoModels.Enqueue(pdfModel);
                        Interlocked.Increment(ref TotalPdfProcessed);

                        Console.WriteLine(InfoConstants.LogInParallelFor, index, TotalPdfProcessed); 
                    }
                }
                catch (Exception ex)
                {
                    ErrorFiles.Enqueue(ErrorPdf);
                }
            });

            // Handle log for corrupted files
            if (ErrorFiles.Count > 0)
            {
                var msg = string.Empty;
                const string header = ExceptionConstants.HeaderForCorruptedFiles;
                
                while (ErrorFiles.TryDequeue(out var errorFile))
                {
                    msg += errorFile + "\n";
                }

                // Remove last new line
                if (msg.Any()) msg.Remove(msg.Length - 1);
                
                FileLogger.SetLog(header + msg);
                ReportGenerator.GenerateErrorLog(msg);
            }

            FileLogger.WriteToFile(string.Format(InfoConstants.PageCountForPdf, TotalPdfProcessed, TotalPdfCount, Percent.ToString("F")));
            FileLogger.SetLog(InfoConstants.CreatingPdfFileInfoDone);

            SetSummary(InfoConstants.Pdf, TotalPdfCount, TotalPdfProcessed);
        }

        public void SavePdfFileInfoModels()
        {
            const string delimiter = ",";
            FileLogger.SetLog(InfoConstants.StartWritingDataIntoCsv);

            try
            {
                var sbHeader = new StringBuilder();
                var data = new[] { InfoConstants.CsvHeader1, InfoConstants.CsvHeader2, InfoConstants.CsvHeader3 };
                sbHeader.AppendLine(string.Join(delimiter, data));

                var sb = new StringBuilder();
                while (ConcurrentPdfFileInfoModels.TryDequeue(out var item))
                {
                    data = new[] { item.FileName, item.PageCount, item.FilePath };
                    sb.AppendLine(string.Join(delimiter, data));
                }

                var sbJoined = sbHeader.Append(sb).ToString();
                if (sb.Length == 0)
                {
                    FileLogger.SetLog(InfoConstants.NoDataFoundToWriteIntoCsv);
                    return;
                } 
                File.WriteAllText(AppSettingsModel.OutputPath, sbJoined);
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.SavePdfFileInfoModels + ex.Message);
            }

            FileLogger.SetLog(InfoConstants.StartWritingDataIntoCsvDone);
        }
        #endregion
    }
}
