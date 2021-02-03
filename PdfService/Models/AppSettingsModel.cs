using System.Collections.Generic;
using PdfService.Constants;

namespace PdfService.Models
{
    public class AppSettingsModel
    {
        public static AppSettingsConstants.InputSource Source { get; set; }
        public static List<string> SelectedOperations { get; set; } = new List<string>();
        public static string FileTypeForConvertToPdf { get; set; }

        public static string InputPath { get; set; }
        public static string RootOutputPath { get; set; }
        public static string RootLogPath { get; set; }
        public static string OutputPath { get; set; }
        public static string LogPath { get; set; }

        public static bool EnableCombinedLogAndReport { get; set; }
        public static string CombinedOutputPath { get; set; }
        public static string CombinedSummaryLogPath { get; set; }
        public static string CombinedErrorLogPath { get; set; }

        public static int LogUpdateTimeInSec { get; set; }
    }
}
