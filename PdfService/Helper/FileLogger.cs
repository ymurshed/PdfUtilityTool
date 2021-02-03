using System;
using System.IO;
using PdfService.Constants;
using PdfService.Models;

namespace PdfService.Helper
{
    public class FileLogger
    {
        public static void SetLog(string msg)
        {
            if (msg.Contains(InfoConstants.ExceptionOccurred))
            {
                Console.WriteLine(msg + "\n");
            }
            else
            {
                Console.WriteLine(msg);
            }

            WriteToFile(msg);
        }

        public static void WriteToFile(string msg, string filePath = "")
        {
            var path = AppSettingsModel.LogPath;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                path = filePath;
            }

            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    File.CreateText(path).Close();
                }

                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (var sw = File.AppendText(path))
                {
                    sw.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ExceptionConstants.WriteToFile + ex.Message);
            }
        }
    }
}