using System;
using System.Collections.Generic;
using PdfService.AppConfigManager;
using PdfService.Constants;
using PdfService.Operations.Implementation;

namespace PdfUtilityTool
{
    public class OperationExecutor
    {
        public static Dictionary<string, object> AppSettingsItems { get; set; }

        private static void LoadAppSettingsItems()
        {
            // Use both operations now
            var selectedOperations = new List<string>
            {
                AppSettingsConstants.OperationTypeConvertPdf,
                AppSettingsConstants.OperationTypePageCount
            };

            AppSettingsItems = new Dictionary<string, object>
            {
                {AppSettingsConstants.Source, AppSettingsConstants.InputSource.Directory},
                {AppSettingsConstants.SelectedOperations, selectedOperations},
                {AppSettingsConstants.FileTypeForConvertToPdf, InfoConstants.DocImageBoth},
                
                {AppSettingsConstants.InputPath, AppConfigReader.GetInputPath()},
                {AppSettingsConstants.RootOutputPath, AppConfigReader.GetRootOutputPath()},
                {AppSettingsConstants.RootLogPath, AppConfigReader.GetRootLogPath()},
                {AppSettingsConstants.OutputPath, ""},
                {AppSettingsConstants.LogPath, ""},

                {AppSettingsConstants.EnableCombinedLogAndReport, AppConfigReader.EnableCombinedLogAndReport()},
                {AppSettingsConstants.CombinedOutputPath, AppConfigReader.GetCombinedOutputPath()},
                {AppSettingsConstants.CombinedSummaryLogPath, AppConfigReader.GetCombinedSummaryLogPath()},
                {AppSettingsConstants.CombinedErrorLogPath, AppConfigReader.GetCombinedErrorLogPath()},

                {AppSettingsConstants.LogUpdateTimeInSec, AppConfigReader.GetLogUpdateTime()}
            };
        }

        public static void ExecutePdfService()
        {
            LoadAppSettingsItems();

            var basicOperation = new BasicOperation();
            basicOperation.OnInit(AppSettingsItems);
            basicOperation.DoTask();
            basicOperation.OnRelease();

            Console.WriteLine(".......... All operation completed. Please colse the app and run again if needed. ..........");
            Console.ReadLine();
        }
    }
}
