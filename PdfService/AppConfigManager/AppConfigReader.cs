using System.Configuration;
using System.IO;
using PdfService.Constants;

namespace PdfService.AppConfigManager
{
    public static class AppConfigReader
    {
        private static string GetConfigurationValue(string key)
        {
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings;
            var value = appSettings.Settings[key].Value;
            return value;
        }

        public static string GetInputPath()
        {
            return GetConfigurationValue(AppSettingsConstants.InputPath);
        }

        public static string GetRootOutputPath()
        {
            var outputPath = GetConfigurationValue(AppSettingsConstants.RootOutputPath); 
            if (!Directory.Exists(outputPath))
            {
                var di = Directory.CreateDirectory(outputPath);
            }
            return outputPath;
        }

        public static string GetRootLogPath()
        {
            var logPath = GetConfigurationValue(AppSettingsConstants.RootLogPath);
            if (!Directory.Exists(logPath))
            {
                var di = Directory.CreateDirectory(logPath);
            }
            return logPath;
        }

        public static bool EnableCombinedLogAndReport()
        {
            bool.TryParse(GetConfigurationValue(AppSettingsConstants.EnableCombinedLogAndReport), out var result);
            return result;
        }

        public static string GetCombinedOutputPath()
        {
            var combinedOutputPath = GetConfigurationValue(AppSettingsConstants.CombinedOutputPath);
            if (!File.Exists(combinedOutputPath))
            {
                File.CreateText(combinedOutputPath).Close();
            }
            return combinedOutputPath;
        }

        public static string GetCombinedSummaryLogPath()
        {
            var combinedSummaryLogPath = GetConfigurationValue(AppSettingsConstants.CombinedSummaryLogPath);
            if (!File.Exists(combinedSummaryLogPath))
            {
                File.CreateText(combinedSummaryLogPath).Close();
            }
            return combinedSummaryLogPath;
        }

        public static string GetCombinedErrorLogPath()
        {
            var combinedErrorLogPath = GetConfigurationValue(AppSettingsConstants.CombinedErrorLogPath);
            if (!File.Exists(combinedErrorLogPath))
            {
                File.CreateText(combinedErrorLogPath).Close();
            }
            return combinedErrorLogPath;
        }

        public static int GetLogUpdateTime()
        {
            var time = int.Parse(GetConfigurationValue(AppSettingsConstants.LogUpdateTimeInSec));
            return time * 1000;
        }
    }
}