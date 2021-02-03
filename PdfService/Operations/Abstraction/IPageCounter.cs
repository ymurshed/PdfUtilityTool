namespace PdfService.Operations.Abstraction
{
    public interface IPageCounter
    {
        int GetPdfPageCount(string file);
        void SetPdfFileInfoModels();
        void SavePdfFileInfoModels();
    }
}
