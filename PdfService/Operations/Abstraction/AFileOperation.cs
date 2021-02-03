using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using PdfService.Constants;
using PdfService.Helper;
using PdfService.Models;

namespace PdfService.Operations.Abstraction
{
    public abstract class AFileOperation
    {
        public Timer Timer { get; set; }
       
        public abstract void DoTask();
        
        public void SetTimer()
        {
            var ms = AppSettingsModel.LogUpdateTimeInSec;
            Timer = new Timer(ms);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void ClearTimer()
        {
            Timer.AutoReset = false;
            Timer.Enabled = false;
        }

        public virtual void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine(InfoConstants.ElapsedEvent, e.SignalTime);
        }

        public void CalculateTimeRequired(DateTime startTime, DateTime stopTime)
        {
            var timeGap = (stopTime - startTime).TotalHours;
            var msg = string.Format(InfoConstants.TimeRequiredInHrs, timeGap);

            if (timeGap < 1)
            {
                if ((stopTime - startTime).TotalMinutes < 1)
                {
                    timeGap = (stopTime - startTime).TotalSeconds;
                    msg = string.Format(InfoConstants.TimeRequiredInSecs, timeGap);
                }
                else
                {
                    timeGap = (stopTime - startTime).TotalMinutes;
                    msg = string.Format(InfoConstants.TimeRequiredInMins, timeGap); 
                }
            }

            FileLogger.SetLog(msg);
        }

        public void SetSummary(string summaryFor, int totalItems, int processedItems, int corruptedFiles = 0)
        {
            var items = new[] {totalItems, processedItems};
            if (ReportGenerator.FileSummaryInfo.ContainsKey(summaryFor))
            {
                ReportGenerator.FileSummaryInfo[summaryFor] = items;
            }
            else
            {
                ReportGenerator.FileSummaryInfo.Add(summaryFor, items);
            }

            if (corruptedFiles > 0)
            {
                items = new[] { corruptedFiles, 0 };
                if (ReportGenerator.FileSummaryInfo.ContainsKey(InfoConstants.CorruptedFiles))
                {
                    ReportGenerator.FileSummaryInfo[InfoConstants.CorruptedFiles] = items;
                }
                else
                {
                    ReportGenerator.FileSummaryInfo.Add(InfoConstants.CorruptedFiles, items);
                }
            }
        }

        public List<string> GetFilesFromInputSource(string fileType = InfoConstants.Pdf)
        {
            List<string> filePaths;

            if (fileType == InfoConstants.Image)
            {
                filePaths = AppInstanceModel.Instance.AllInputFiles.Where(item => item.IsImageFile()).Select(item => item).ToList();
            }
            else if (fileType == InfoConstants.Doc)
            {
                filePaths = AppInstanceModel.Instance.AllInputFiles.Where(item => item.IsDocFile()).Select(item => item).ToList();
            }
            else if (fileType == InfoConstants.AllButPdf)
            {
                filePaths = AppInstanceModel.Instance.AllInputFiles.Where(item => item.IsSupportedFile()).Select(item => item).ToList();
            }
            else
            {
                filePaths = AppInstanceModel.Instance.AllInputFiles.Where(item => item.IsPdfFile()).Select(item => item).ToList();
                if (AppInstanceModel.Instance.AllInputPdfFiles != null)
                {
                    filePaths.AddRange(AppInstanceModel.Instance.AllInputPdfFiles);
                }
                filePaths = filePaths.Distinct().ToList();
            }
            
            return filePaths;
        }
    }
}
