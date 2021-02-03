using PdfService.Constants;
using PdfService.Helper;
using PdfService.Operations.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using PdfService.Models;
using static PdfService.Models.AppSettingsModel;

namespace PdfService.Operations.Implementation
{
    public class BasicOperation : AFileOperation, IBasicOperation
    {
        public static bool IsFolderEmpty => InputPath.IsFolderEmpty();

        public override void DoTask()
        {
            try
            {
                foreach (var selectedOperation in SelectedOperations)
                {
                    var className = AppSettingsConstants.OperationTypes[selectedOperation];
                    var type = Type.GetType(className);
                    if (type != null)
                    {
                        var obj = (AFileOperation)Activator.CreateInstance(type);
                        obj.DoTask();
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.BasicOperationDoTask + ex.Message);
                throw ex;
            }
        }

        public void OnInit(Dictionary<string, object> appSettingsItems)
        {
            LoadAppSettings(appSettingsItems);
            LoadAllInputFiles();
        }

        public void OnRelease()
        {
            GenerateLogAndReport();
            DataCleanUp();
        }

        private void DataCleanUp()
        {
            DeleteProcessedInputFile();

            if (CombinedErrorLogPath.IsFileEmpty()) File.Delete(CombinedErrorLogPath);
            if (CombinedSummaryLogPath.IsFileEmpty()) File.Delete(CombinedSummaryLogPath);
            if (CombinedOutputPath.IsFileEmpty()) File.Delete(CombinedOutputPath);

            AppInstanceModel.Instance = null;
            ReportGenerator.FileSummaryInfo.Clear();
        }

        private void GenerateLogAndReport()
        {
            if (Source == AppSettingsConstants.InputSource.List)
            {
                ReportGenerator.GenerateSummaryLog();
                ReportGenerator.GenerateCombinedOutput();
            }
        }

        private void LoadAppSettings(Dictionary<string, object> appSettingsItems)
        {
            foreach (var item in appSettingsItems)
            {
                switch (item.Key)
                {
                    case AppSettingsConstants.Source:
                        Source = (AppSettingsConstants.InputSource)Enum.Parse
                                 (typeof(AppSettingsConstants.InputSource), item.Value.ToString());
                        break;

                    case AppSettingsConstants.SelectedOperations:
                        SelectedOperations = (List<string>)item.Value;
                        break;

                    case AppSettingsConstants.FileTypeForConvertToPdf:
                        FileTypeForConvertToPdf = item.Value.ToString();
                        break;

                    case AppSettingsConstants.InputPath:
                        InputPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.RootOutputPath:
                        RootOutputPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.RootLogPath:
                        RootLogPath = item.Value.ToString();
                        break;

                    // Todo: need to handle
                    case AppSettingsConstants.OutputPath:
                        OutputPath = item.Value.ToString();
                        break;

                    // Todo: need to handle
                    case AppSettingsConstants.LogPath:
                        LogPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.EnableCombinedLogAndReport:
                        bool.TryParse(item.Value.ToString(), out var isEnabled);
                        EnableCombinedLogAndReport = isEnabled;
                        break;

                    case AppSettingsConstants.CombinedOutputPath:
                        CombinedOutputPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.CombinedSummaryLogPath:
                        CombinedSummaryLogPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.CombinedErrorLogPath:
                        CombinedErrorLogPath = item.Value.ToString();
                        break;

                    case AppSettingsConstants.LogUpdateTimeInSec:
                        int.TryParse(item.Value.ToString(), out var timeInSec);
                        LogUpdateTimeInSec = timeInSec;
                        break;
                }
            }
        }

        private void LoadAllInputFiles()
        {
            try
            {
                // Initiate the AppInstanceModel instance 
                FileLogger.SetLog(string.Format(InfoConstants.LoadAllInputFiles, AppInstanceModel.Instance.AllInputFiles.Count));

                var nonSupportedFileCount = AppInstanceModel.Instance.NonSupportedFileCount;
                if (nonSupportedFileCount > 0)
                {
                    FileLogger.SetLog(string.Format(InfoConstants.NonSupportedFileFound, nonSupportedFileCount));
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.LoadAllInputFiles + ex.Message);
            }
        }

        private void DeleteProcessedInputFile()
        {
            try
            {
                if (File.Exists(AppInstanceModel.Instance.CurrentProcessingFile))
                {
                    File.Delete(AppInstanceModel.Instance.CurrentProcessingFile);
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.DeleteProcessedInputFile + ex.Message);
            }
        }
    }
}
