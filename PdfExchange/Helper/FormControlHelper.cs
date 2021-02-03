using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using PdfExchange.Constants;
using PdfService.Constants;
using MessageBox = System.Windows.MessageBox;

namespace PdfExchange.Helper
{
    public class FormControlHelper
    {
        public static ArrayList LoadListBoxData()
        {
            return new ArrayList
            {
                AppSettingsConstants.OperationTypeConvertPdf,
                AppSettingsConstants.OperationTypePageCount,
                AppSettingsConstants.OperationTypeRemoveSecurity
            };
        }

        public static List<string> ConvertToList(IEnumerable items, Dictionary<int, string> sortBy = null)
        {
            var listItems = (from object item in items select item.ToString()).ToList();
            if (sortBy == null) return listItems;

            var sortedListItems = new List<string>();
            foreach (var element in sortBy)
            {
                var operationType = sortBy[element.Key];
                if (listItems.Contains(operationType)) sortedListItems.Add(operationType);
            }

            if (!sortedListItems.Any()) sortedListItems = listItems;
            return sortedListItems;
        }

        public static bool EnableFileTypeGroupBox(IEnumerable items)
        {
            var listItems = ConvertToList(items);
            return listItems.Exists(x => x.Contains(AppSettingsConstants.OperationTypeConvertPdf));
        }

        public static string GetFolderPath()
        {
            var rootPath = ApplicationConstants.Root;
            var path = rootPath;

            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = rootPath;
                if (folderBrowser.ShowDialog() == DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    path = folderBrowser.SelectedPath;
                }
            }
            return path;
        }

        public static string GetFilePath(string defaultFile, string filter = InfoConstants.StarTxtExtension)
        {
            var path = defaultFile;
            using (var fileBrowser = new OpenFileDialog())
            {
                fileBrowser.Filter = $@"txt files ({filter})|{filter}";

                if (fileBrowser.ShowDialog() == DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(fileBrowser.FileName))
                {
                    path = fileBrowser.FileName;
                }
            }
            return path;
        }

        public static bool ShowConfirmationMsgBox()
        {
            return MessageBox.Show(ApplicationConstants.Sure, ApplicationConstants.Confirmation,
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public static bool ShowSuccessAlertMsgBox(string message)
        {
            return MessageBox.Show(message, ApplicationConstants.Success,
                    MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK;
        }

        public static bool ShowWarningAlertMsgBox(string message)
        {
            return MessageBox.Show(message, ApplicationConstants.Warning,
                    MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK;
        }
    }
}
