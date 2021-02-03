namespace PdfService.FileHandler
{
    public interface IFileHandler
    {
        void ConvertToPdf(string source, string destination);
    }
}