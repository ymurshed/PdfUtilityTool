namespace PdfService.Constants
{
    public class ExceptionConstants
    {
        // FileLogger Class
        public const string WriteToFile = "Exception occurred inside WriteToFile(): exception msg: ";

        // ReportGenerator
        public const string GenerateSummaryLog = "Exception occurred inside GenerateSummaryLog(), exception msg: ";
        public const string GenerateCombinedOutput = "Exception occurred inside GenerateCombinedOutput(), exception msg: ";

        // BasicOperation
        public const string LoadAllInputFiles = "Exception occurred inside LoadAllInputFiles(), exception msg: "; 
        public const string DeleteProcessedInputFile = "Exception occurred inside DeleteProcessedInputFile(), exception msg: ";
        public const string BasicOperationDoTask = "Exception occurred inside BasicOperation, DoTask(), exception msg: ";

        // PdfConverter
        public const string PdfConverterDoTask = "Exception occurred inside PdfConverter, DoTask(), exception msg: ";
        public const string ConvertImageToPdf = "Exception occurred inside ConvertImageToPdf(), for the file: {0}, exception msg: {1}";
        public const string ConvertDocToPdf = "Exception occurred inside ConvertDocToPdf(), for the file: {0}, exception msg: {1}";
        public const string ConvertToPdf = "Exception occurred inside ConvertToPdf(), for the file: {0}, exception msg: {1}";

        // PageCounter
        public const string PageCounterDoTask = "Exception occurred inside PageCounter, DoTask(), exception msg: ";
        public const string SavePdfFileInfoModels = "Exception occurred inside SavePdfFileInfoModels(), exception msg: ";
        public const string HeaderForCorruptedFiles = "Exception occurred inside SetPdfFileInfoModels(), for the corrupted files: \n";

        // SecurityRemover
        public const string SecurityRemoverDoTask = "Exception occurred inside SecurityRemover, DoTask(), exception msg: ";
        public const string ErrorProcessingSecuritySettings = "Exception occurred inside ProcessSecuritySettingsInPdf(), for the error-ed files: \n";
    }
}
