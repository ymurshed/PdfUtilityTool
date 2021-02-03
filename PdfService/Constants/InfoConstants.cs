namespace PdfService.Constants
{
    public class InfoConstants
    {
        // General
        public const string Star = "*";
        public const string StarCsvExtension = "*.csv";
        public const string StarTxtExtension = "*.txt";
        public const string StarPdfExtension = "*.pdf";
        public const string CsvExtension = ".csv";
        public const string TxtExtension = ".txt";
        public const string PdfExtension = ".pdf";

        public const string Doc = "doc";
        public const string Image = "image";
        public const string AllButPdf = "all";
        public const string NonPdf = "non pdf";
        public const string Pdf = "pdf";
        public const string DocImageBoth = "both";
        public const string CorruptedFiles = "corrupted files";

        public const string CsvHeader1 = "File Name";
        public const string CsvHeader2 = "Page Count";
        public const string CsvHeader3 = "File Path";

        public const string MaxFileCountExceed = "Total {0} file count {1} exceeded maximum allowed range {2}";
        public const string MaxAllowedFileCountExceed = "Total file count {0} exceeded maximum allowed range {1}";
        public const string ExceptionOccurred = "Exception occurred";
        
        // ReportGenerator
        public const string FoundMsg = "Total {0} file found = {1}\n";
        public const string ProcessedMsg = "Total {0} file processed = {1}\n";

        // AFileOperation
        public const string ElapsedEvent = "The Elapsed event was raised at {0:HH:mm:ss.fff}";
        public const string TimeRequiredInHrs = "Total time required: {0} hours\n";
        public const string TimeRequiredInMins = "Total time required: {0} mins\n";
        public const string TimeRequiredInSecs = "Total time required: {0} secs\n";

        // BasicOperation
        public const string NonSupportedFileFound  = "Total non-supported file found = {0}\n";
        public const string LoadAllInputFiles  = "Loading up all input files .......... Total files found = {0}\n";

        // PdfConverter
        public const string FileConvertedToPdf = "{0}/{1} files are converted to pdf. Task completed = {2}%";
        public const string StartingNonPdfFileConversion = "---> Starting {0} files conversion. Total {1} found = {2}";
        public const string CompleteNonPdfFileConversion = "---> Conversion of {0} files completed";
        public const string StartingFileConversion = "---> Starting supported files conversion. Total files found = {0}";
        public const string CompleteFileConversion = "---> Conversion of supported files completed";
        
        // PageCounter
        public const string PageCountForPdf = "{0}/{1} pdf page count are done. Task completed = {2}%";
        public const string StartingPageCount = "---> Starting {0} page count operation. Total {1} found = {2}";
        public const string CompletePageCount = "---> Pdf page count operation completed. Total pdf found = {0}, Total pdf processed = {1}, Error occurred = {2}";

        // SecurityRemover
        public const string RemoveSecurityForPdf = "{0}/{1} pdf security are removed. Task completed = {2}%";
        public const string StartingRemoveSecurity = "---> Starting {0} security remove operation. Total {1} found = {2}";
        public const string CompleteRemoveSecurity = "---> Pdf security remove operation completed. Total pdf found = {0}, Total pdf processed = {1}, Error occurred = {2}";

        public const string ProcessingSecuritySettings = "Processing security settings in pdf";
        public const string ProcessingSecuritySettingsDone = "Done pdf security settings processing";
        public const string CreatingPdfFileInfo = "Creating pdf file info dictionary";
        public const string CreatingPdfFileInfoDone = "Pdf file info dictionary created successfully";
        public const string StartWritingDataIntoCsv = "Start writing data into csv file";
        public const string StartWritingDataIntoCsvDone = "Data writing into csv file completed";
        public const string NoDataFoundToWriteIntoCsv = "No data found to write into csv file";
        public const string LogInParallelFor = "index: {0}, \tprocessed: {1}";
    }
}
