using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfService.Constants;
using PdfService.Models;

namespace PdfService.Helper
{
    public static class UtilityHelper
    {
        #region Private methods
        private static string GetPath(string path, string fileNameWithoutExt, string ext)
        {
            return Path.Combine(path, $"{fileNameWithoutExt}{ext}");
        } 

        private static List<string> GetFilesToRead(List<string> filePaths, ref string firstFile, string ext = "*")
        {
            var sortedFiles = GetSortedFiles(filePaths, ext);
            if (!sortedFiles.Any()) return null;

            if (AppSettingsModel.Source == AppSettingsConstants.InputSource.Directory ||
                AppSettingsModel.Source == AppSettingsConstants.InputSource.SingleFile)
            {
                return sortedFiles;
            }

            firstFile = sortedFiles.First();
            var allLines = File.ReadAllLines(firstFile);
            return allLines.ToList();
        }
        #endregion

        public static string GetPathWithOrWithoutTimestamp(string instanceId, string path, string ext, bool addTimestamp = false)
        {
            var now = DateTime.Now;
            var currentTime = $"{now.Day}-{now.Month}-{now.Year}__{now.Hour}h-{now.Minute}m-{now.Second}s";

            var fullPath = addTimestamp
                ? Path.Combine(path, $"{instanceId}-{currentTime}{ext}")
                : Path.Combine(path, $"{instanceId}{ext}");
            return fullPath;
        }

        public static Tuple<string, List<string>> GetInputFilesToRead()
        {
            var firstFile = string.Empty;
            List<string> filesToRead;
            
            if (AppSettingsModel.Source == AppSettingsConstants.InputSource.List)
            {
                var filePaths = Directory.GetFiles(AppSettingsModel.InputPath, InfoConstants.StarTxtExtension, SearchOption.AllDirectories).ToList();
                filesToRead = GetFilesToRead(filePaths, ref firstFile, InfoConstants.TxtExtension);
            }
            else
            {
                var filePaths = Directory.GetFiles(AppSettingsModel.InputPath, InfoConstants.Star, SearchOption.AllDirectories).ToList();
                filesToRead = GetFilesToRead(filePaths, ref firstFile);
            }

            return Tuple.Create(firstFile, filesToRead);
        }
        
        public static List<string> GetSortedFiles(List<string> filePaths, string ext, bool allFileNameWithDigitOnly = true)
        {
            var fileIds = new List<int>();
            var sortedFileList = new List<string>();
            var rootPath = string.Empty;

            // Disable the sorting process by file id
            if (AppSettingsModel.Source == AppSettingsConstants.InputSource.Directory ||
                AppSettingsModel.Source == AppSettingsConstants.InputSource.SingleFile)
            {
                allFileNameWithDigitOnly = false;
            }

            if (allFileNameWithDigitOnly)
            {
                foreach (var filePath in filePaths)
                {
                    rootPath = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileNameWithoutExtension(filePath)?.Trim();

                    if (string.IsNullOrWhiteSpace(fileName)) continue;

                    if (fileName.Any(x => !char.IsLetter(x)))
                    {
                        var id = int.Parse(fileName);
                        fileIds.Add(id);
                    }
                }

                fileIds.Sort();
                sortedFileList.AddRange(fileIds.Select(x => GetPath(rootPath, x.ToString(), ext)));
                return sortedFileList;
            }

            filePaths.Sort();
            sortedFileList.AddRange(filePaths);
            return sortedFileList;
        }

        public static string GetOutputPath(string instanceId)
        {
            return GetPathWithOrWithoutTimestamp(instanceId, AppSettingsModel.RootOutputPath, InfoConstants.CsvExtension);
        }

        public static string GetLogPath(string instanceId)
        {
            return GetPathWithOrWithoutTimestamp(instanceId, AppSettingsModel.RootLogPath, InfoConstants.TxtExtension);
        }
    }
}
