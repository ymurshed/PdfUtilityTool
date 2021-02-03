namespace PdfService.Operations.Abstraction
{
    public interface ISecurityRemover
    {
        void ProcessSecuritySettingsInPdf();
        void RemoveSecurityFromPdf(string file);
    }
}
