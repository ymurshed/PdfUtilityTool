namespace PdfService.Models
{
    public class PdfModel
    {
        public string FileName { get; set; }
        public string PageCount { get; set; }
        public string FilePath { get; set; }

        public PdfModel(string fileName, int pageCount, string filePath)
        {
            FileName = fileName;
            PageCount = pageCount.ToString();
            FilePath = filePath;
        }
    }
}
