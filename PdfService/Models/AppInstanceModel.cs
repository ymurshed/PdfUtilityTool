using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfService.Constants;
using PdfService.Helper;

namespace PdfService.Models
{
    public class AppInstanceModel
    {
        public string InstanceId { get; set; }
        
        private static AppInstanceModel _instance;
        private static readonly object SyncLock = new object();

        #region Input files
        public int NonSupportedFileCount { get; set; }
        public List<string> AllInputFiles { get; set; }
        public List<string> AllInputImageFiles { get; set; }
        public List<string> AllInputDocFiles { get; set; }
        public List<string> AllInputPdfFiles { get; set; }
        public string CurrentProcessingFile { get; set; }
        #endregion

        private AppInstanceModel()
        {
            AllInputFiles = new List<string>();
            AllInputImageFiles = new List<string>();
            AllInputDocFiles = new List<string>();
            AllInputPdfFiles = new List<string>();

            var result = UtilityHelper.GetInputFilesToRead();

            CurrentProcessingFile = result.Item1;
            AllInputFiles = result.Item2.Where(x => !x.IsNonSupportedFile()).Select(x => x).ToList();
            NonSupportedFileCount = result.Item2.Count - AllInputFiles.Count;

            InstanceId = AppSettingsModel.Source == AppSettingsConstants.InputSource.List ? 
                                                    Path.GetFileNameWithoutExtension(CurrentProcessingFile) : 
                                                    Guid.NewGuid().ToString("N");

            AppSettingsModel.OutputPath = UtilityHelper.GetOutputPath(InstanceId);
            AppSettingsModel.LogPath = UtilityHelper.GetLogPath(InstanceId);
        }
        
        public static AppInstanceModel Instance
        {
            get
            {
                lock (SyncLock)
                {
                    return _instance ?? (_instance = new AppInstanceModel());
                }
            }
            set
            {
                lock (SyncLock)
                {
                    _instance = value;
                }
            } 
        }
    }
}
