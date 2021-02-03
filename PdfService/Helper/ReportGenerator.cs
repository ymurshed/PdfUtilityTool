using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using PdfService.Constants;
using PdfService.Models;

namespace PdfService.Helper
{
    public class ReportGenerator
    {
        public static string CsvOutputPath => AppSettingsModel.RootOutputPath;
        public static Dictionary<string, int[]> FileSummaryInfo = new Dictionary<string, int[]>();
        
        private static List<string> GetFiles(string ext)
        {
            return Directory.GetFiles(CsvOutputPath, ext, SearchOption.AllDirectories).ToList();
        }

        public static void GenerateErrorLog(string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(errorMsg) || !AppSettingsModel.EnableCombinedLogAndReport) return;
            FileLogger.WriteToFile(errorMsg, AppSettingsModel.CombinedErrorLogPath);
        }

        public static void GenerateSummaryLog()
        {
            if (!AppSettingsModel.EnableCombinedLogAndReport) return;

            try
            {
                var summaryLogPath = AppSettingsModel.CombinedSummaryLogPath;
                if (!File.Exists(summaryLogPath)) return;

                var allLines = File.ReadAllLines(summaryLogPath);
                if (allLines.Any())
                {
                    for (var i = 0; i < allLines.Length; i++)
                    {
                        string key;
                        if (i < 2) key = InfoConstants.NonPdf;
                        else if (i < 4) key = InfoConstants.Pdf;
                        else key = InfoConstants.CorruptedFiles;

                        var currentLine = allLines[i];
                        var nummber = Regex.Match(currentLine, @"\d+").Value;
                        var index = currentLine.Contains("found") ? 0 : 1;
                        if (FileSummaryInfo.ContainsKey(key))
                        {
                            FileSummaryInfo[key][index] += int.Parse(nummber);
                        }
                    }
                }

                var summaryFound = false;
                var summaryMsg = string.Empty;
                foreach (var info in FileSummaryInfo)
                {
                    if (info.Key.Contains(InfoConstants.CorruptedFiles))
                    {
                        if (info.Value[0] > 0) summaryFound = true;
                        summaryMsg += string.Format(InfoConstants.FoundMsg, info.Key, info.Value[0]);
                    }
                    else
                    {
                        if (info.Value[0] > 0 || info.Value[1] > 0) summaryFound = true;
                        summaryMsg += string.Format(InfoConstants.FoundMsg, info.Key, info.Value[0]) +
                                      string.Format(InfoConstants.ProcessedMsg, info.Key, info.Value[1]);
                    }
                }

                if (summaryFound)
                {
                    File.WriteAllText(summaryLogPath, summaryMsg);
                }
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.GenerateSummaryLog + ex.Message);
            }
        }

        public static void GenerateCombinedOutput()
        {
            if (!AppSettingsModel.EnableCombinedLogAndReport) return;

            try
            {
                var combinedLines = new List<string>();
                var csvFiles = GetFiles(InfoConstants.StarCsvExtension);
                var sortedFiles = UtilityHelper.GetSortedFiles(csvFiles, InfoConstants.CsvExtension);

                foreach (var file in sortedFiles)
                {
                    if (File.Exists(file))
                    {
                        var lines = File.ReadLines(file).ToList();
                        if (lines.Any())
                        {
                            lines.RemoveAt(0);
                        }

                        combinedLines.AddRange(lines);
                    }
                }

                if (combinedLines.Any())
                {
                    File.WriteAllLines(AppSettingsModel.CombinedOutputPath, combinedLines);
                } 
            }
            catch (Exception ex)
            {
                FileLogger.SetLog(ExceptionConstants.GenerateCombinedOutput + ex.Message);
            }
        }
    }
}
