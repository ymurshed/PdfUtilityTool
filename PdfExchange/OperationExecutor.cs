using System.Collections.Generic;
using PdfExchange.Helper;
using PdfService.AppConfigManager;
using PdfService.Constants;
using PdfService.Operations.Implementation;

namespace PdfExchange
{
    public class OperationExecutor
    {
        public static Dictionary<string, object> AppSettingsItems { get; set; }
        public static MainWindow AppWindow { get; set; }

        private static void LoadAppSettingsItems()
        {
            var inputSource = string.Empty;
            var selectedOperations = new List<string>();
            var fileTypeToConvertPdf = string.Empty;

            AppWindow.Dispatcher?.Invoke(() =>
            {
                fileTypeToConvertPdf = InfoConstants.AllButPdf;
                inputSource = AppWindow.ComboBoxInputSource.SelectedValue.ToString();
                selectedOperations = FormControlHelper.ConvertToList(AppWindow.ListBoxRight.Items.SourceCollection,
                    AppSettingsConstants.OperationTypesExecutionSequence);
            });

            AppSettingsItems = new Dictionary<string, object>
            {
                {AppSettingsConstants.Source, inputSource},
                {AppSettingsConstants.SelectedOperations, selectedOperations},
                {AppSettingsConstants.FileTypeForConvertToPdf, fileTypeToConvertPdf},
                
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

        public static bool ExecutePdfService()
        {
            LoadAppSettingsItems();

            var basicOperation = new BasicOperation();
            basicOperation.OnInit(AppSettingsItems);
            basicOperation.DoTask();
            basicOperation.OnRelease();
            return true;
        }
    }
}
