using System.Collections.Generic;

namespace PdfService.Constants
{
    public class AppSettingsConstants
    {
        public const int MaxFileProcessCount = 9500;

        public enum InputSource { List = 1, Directory, SingleFile }

        public static Dictionary<string, string> OperationTypes => new Dictionary<string, string>
        {
            {OperationTypeConvertPdf, "PdfService.Operations.Implementation.PdfConverter"},
            {OperationTypePageCount, "PdfService.Operations.Implementation.PageCounter"},
            {OperationTypeRemoveSecurity, "PdfService.Operations.Implementation.SecurityRemover"}
        };

        public static Dictionary<int, string> OperationTypesExecutionSequence => new Dictionary<int, string>
        {
            {1, OperationTypeConvertPdf},
            {2, OperationTypeRemoveSecurity},
            {3, OperationTypePageCount}
        };

        public const string FileTypeForConvertToPdf = "File Type for Convert to Pdf";

        public const string OperationTypeConvertPdf = "Convert to Pdf";
        public const string OperationTypePageCount = "Count Page";
        public const string OperationTypeRemoveSecurity = "Remove Security";

        public const string Source = "Input Source";
        public const string SelectedOperations = "Selected Operations";

        /// <summary>
        /// App Config Keys
        /// </summary>
        public const string InputPath = "InputPath";
        public const string RootOutputPath = "OutputPath";
        public const string RootLogPath = "LogPath";
        public const string OutputPath = "Output Path";
        public const string LogPath = "Log Path";

        public const string EnableCombinedLogAndReport = "EnableCombinedLogAndReport";
        public const string CombinedOutputPath = "CombinedOutputPath";
        public const string CombinedSummaryLogPath = "CombinedSummaryLogPath";
        public const string CombinedErrorLogPath = "CombinedErrorLogPath";

        public const string LogUpdateTimeInSec = "LogUpdateTimeInSec";
    }
}
