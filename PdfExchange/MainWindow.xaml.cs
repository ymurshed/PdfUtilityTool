using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PdfExchange.Constants;
using PdfExchange.Helper;
using PdfService.AppConfigManager;
using PdfService.Constants;
using ComboBox = System.Windows.Controls.ComboBox;

namespace PdfExchange
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Helper methods
        private Dictionary<string, string> GetAppConfigWriterDictionary()
        {
            return new Dictionary<string, string>
            {
                {AppSettingsConstants.RootOutputPath, TextBoxOutputPath.Text},
                {AppSettingsConstants.RootLogPath, TextBoxLogPath.Text},
                {AppSettingsConstants.EnableCombinedLogAndReport, CheckBoxEnableCombinedLogAndReport.IsChecked.ToString()},
                {AppSettingsConstants.CombinedOutputPath, TextBoxCombinedOutputPath.Text},
                {AppSettingsConstants.CombinedErrorLogPath, TextBoxCombinedErrorLogPath.Text},
                {AppSettingsConstants.CombinedSummaryLogPath, TextBoxCombinedSummaryLogPath.Text},
                {AppSettingsConstants.LogUpdateTimeInSec, TextBoxLogUpdateTimeInSec.Text}
            };
        }

        private void LoadDefaultLogSettings()
        {
            OperationExecutor.AppWindow = this;
            ListBoxLeft.ItemsSource = _dataList = FormControlHelper.LoadListBoxData();
            
            // Load Combobox by input sources
            var inputSources = Enum.GetValues(typeof(AppSettingsConstants.InputSource)).Cast<AppSettingsConstants.InputSource>();
            var enumerable = inputSources as AppSettingsConstants.InputSource[] ?? inputSources.ToArray();
            ComboBoxInputSource.ItemsSource = enumerable.Reverse().Skip(1); // Remove the single file input support
           
            // Setup from config
            TextBoxOutputPath.Text = AppConfigReader.GetRootOutputPath();
            TextBoxLogPath.Text = AppConfigReader.GetRootLogPath();
            CheckBoxEnableCombinedLogAndReport.IsChecked = AppConfigReader.EnableCombinedLogAndReport();
            TextBoxCombinedOutputPath.Text = AppConfigReader.GetCombinedOutputPath();
            TextBoxCombinedErrorLogPath.Text = AppConfigReader.GetCombinedErrorLogPath();
            TextBoxCombinedSummaryLogPath.Text = AppConfigReader.GetCombinedSummaryLogPath();
            var updateTimeInSec = AppConfigReader.GetLogUpdateTime() / 1000;
            TextBoxLogUpdateTimeInSec.Text = updateTimeInSec.ToString();
        }
        
        private void ApplyDataBinding()
        {
            ListBoxLeft.ItemsSource = null;
            ListBoxLeft.ItemsSource = _dataList;
        }

        private void SetStatusLabel(bool set = true)
        {
            if (set)
            {
                StackPanelOperations.IsEnabled = true;
                LabelStatusMessage.Content = ApplicationConstants.StatusMessageDone;
            }
            else
            {
                StackPanelOperations.IsEnabled = false;
                LabelStatusMessage.Content = ApplicationConstants.StatusMessageInProgress;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool IsAnySettingsTabControlInputNotGiven()
        {
            if (TextBoxOutputPath.Text.IsObjectEmpty() || 
                TextBoxLogPath.Text.IsObjectEmpty() ||
                TextBoxCombinedOutputPath.Text.IsObjectEmpty() ||
                TextBoxCombinedSummaryLogPath.Text.IsObjectEmpty() ||
                TextBoxCombinedErrorLogPath.Text.IsObjectEmpty() ||
                TextBoxLogUpdateTimeInSec.Text.IsObjectEmpty())
            {
                return true;
            }
            return false;
        }

        private bool IsAnyOptionsTabControlInputNotGiven()
        {
            if (ComboBoxInputSource.SelectedValue.IsObjectEmpty() ||
                TextBoxInputPath.Text.IsObjectEmpty() ||
                ListBoxRight.Items.SourceCollection.IsObjectEmpty())
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Private variables
        ArrayList _dataList = null;
        string _currentItemText;
        int _currentItemIndex;
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultLogSettings();
        }

        void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is System.Windows.Controls.TabControl)
            {
                //do work when tab is changed
            }
        }

        #region Settings tab
        private void ButtonOutputPath_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxOutputPath.Text = FormControlHelper.GetFolderPath();
        }

        private void ButtonLogPath_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxLogPath.Text = FormControlHelper.GetFolderPath();
        }

        private void ButtonCombinedOutputPath_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxCombinedOutputPath.Text = FormControlHelper.GetFilePath(AppConfigReader.GetCombinedOutputPath(), InfoConstants.StarCsvExtension);
        }

        private void ButtonCombinedSummaryLogPath_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxCombinedSummaryLogPath.Text = FormControlHelper.GetFilePath(AppConfigReader.GetCombinedSummaryLogPath());
        }

        private void ButtonCombinedErrorLogPath_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxCombinedErrorLogPath.Text = FormControlHelper.GetFilePath(AppConfigReader.GetCombinedErrorLogPath());
        }

        private void CheckBoxGroupBoxControl_OnClick(object sender, RoutedEventArgs e)
        {
            var isChecked = CheckBoxGroupBoxControl.IsChecked;
            if (isChecked.HasValue) GroupBoxLogSettings.IsEnabled = isChecked.Value;
        }

        private void ButtonShowSupportedExtensions_OnClick(object sender, RoutedEventArgs e)
        {
            var supportedExtensions = $"Supported extensions for pdf conversion are: \n {string.Join(",  ", PdfService.Helper.ExtensionHelper.SupportedExt)}";
            FormControlHelper.ShowSuccessAlertMsgBox(supportedExtensions);
        }

        private void ButtonSaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsAnySettingsTabControlInputNotGiven())
            {
                FormControlHelper.ShowWarningAlertMsgBox(ApplicationConstants.InputControlEmpty);
                return;
            }

            if (FormControlHelper.ShowConfirmationMsgBox())
            {
                var appConfig = GetAppConfigWriterDictionary();
                AppConfigWriter.UpdateAppConfigKeys(appConfig);

                FormControlHelper.ShowSuccessAlertMsgBox(ApplicationConstants.Done);
            }
        }
        #endregion

        #region Operations tab
        private void ButtonAddItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBoxLeft.SelectedValue == null) return;

            _currentItemText = ListBoxLeft.SelectedValue.ToString();
            _currentItemIndex = ListBoxLeft.SelectedIndex;

            ListBoxRight.Items.Add(_currentItemText);
            _dataList?.RemoveAt(_currentItemIndex);

            ApplyDataBinding();
        }

        private void ButtonRemoveItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBoxRight.SelectedValue == null) return;

            _currentItemText = ListBoxRight.SelectedValue.ToString();
            _currentItemIndex = ListBoxRight.SelectedIndex;
            
            _dataList.Add(_currentItemText);
            ListBoxRight.Items.RemoveAt(ListBoxRight.Items.IndexOf(ListBoxRight.SelectedItem));

            ApplyDataBinding();
        }

        private void ComboBoxInputSource_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox item)) return;

            if (item.SelectedValue.ToString() == AppSettingsConstants.InputSource.List.ToString() ||
                item.SelectedValue.ToString() == AppSettingsConstants.InputSource.Directory.ToString())
            {
                TextBoxInputPath.Text = FormControlHelper.GetFolderPath();
            }
            // Todo: handle single file
            else
            {
                TextBoxInputPath.Text = string.Empty;
            }
        }

        private async void ButtonExecute_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsAnyOptionsTabControlInputNotGiven())
            {
                FormControlHelper.ShowWarningAlertMsgBox(ApplicationConstants.InputControlEmpty);
                return;
            }

            AppConfigWriter.UpdateAppConfigKey(AppSettingsConstants.InputPath, TextBoxInputPath.Text);
            SetStatusLabel(false);
            
            var result = await Task.Run(() => OperationExecutor.ExecutePdfService());
            if (result)
            {
                SetStatusLabel();
                FormControlHelper.ShowSuccessAlertMsgBox(ApplicationConstants.Done);
            }
        }
        #endregion
    }
}
