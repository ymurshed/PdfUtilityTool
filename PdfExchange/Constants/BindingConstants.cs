using System.Windows;
using System.Windows.Media;

namespace PdfExchange.Constants
{
    public static class BindingConstants
    {
        #region Color & Margin
        public static Brush WindowBgColor => Brushes.GhostWhite;
        public static Brush SettingsTabSpBgColor => Brushes.LightSkyBlue;
        public static Brush OperationsTabSpBgColor => Brushes.LightSkyBlue;
        
        public static Brush ButtonBgColor => Brushes.AliceBlue;
        public static Brush CheckboxBgColor => Brushes.White;
        public static Brush TextboxBgColor => Brushes.White;
        public static Brush ComboboxBgColor => Brushes.White;
        public static Brush ListboxBgColor => Brushes.White;
        public static Brush StatusLblColor => Brushes.LightGoldenrodYellow;

        public static Thickness CommonMargin => new Thickness(5, 5, 5, 5);
        public static Thickness AddButtonMargin => new Thickness(5, 45, 5, 5);
        public static Thickness RemoveButtonMargin => new Thickness(5, 5, 5, 45);
        #endregion

        #region Common Constants
        public const double WindowHeight = 500;
        public const double WindowWidth = 600;
        public const double HeightLessSize = 50;
        public const double WidthLessSize = 25;

        public const double TabHeight = WindowHeight - HeightLessSize;
        public const double TabWidth = WindowWidth - WidthLessSize;
        public const double TabHeightLessSize = 80;
        public const double TabWidthLessSize = 30;
        #endregion

        #region Settings Constants
        public const int SettingsTotalRow = 8;
        public const int SettingsTotalColumn = 2;

        public const double SettingsGridColumn0WidthPercent = 40.0 / 100;
        public const double SettingsGridColumn1WidthPercent = 60.0 / 100;

        public static GridLength SettingsGridRowHeight => new GridLength((TabHeight - TabHeightLessSize) / SettingsTotalRow, GridUnitType.Pixel);
        public static GridLength SettingsGridColumn0Width => new GridLength((TabWidth -TabWidthLessSize) * SettingsGridColumn0WidthPercent, GridUnitType.Pixel);
        public static GridLength SettingsGridColumn1Width => new GridLength((TabWidth - TabWidthLessSize) * SettingsGridColumn1WidthPercent, GridUnitType.Pixel);
        #endregion

        #region Operations Constants
        public const int OperationTotalRow = 5;
        public const int OperationTotalColumn = 4;

        public const double AddRemoveButtonHeight = 40;

        public const double ExecuteButtonHeight = 35;
        public const double ExecuteButtonWidth = 250;

        public const double ComboboxWidth = 175;
        public const double GroupboxConvertTypeWidth = 120;

        public static GridLength OperationGridRowHeight => SettingsGridRowHeight;
        public static GridLength OperationGridColumnWidth => new GridLength((TabWidth - TabWidthLessSize) / OperationTotalColumn);
        public static GridLength ListboxHeight => new GridLength(180);
        #endregion
    }
}
