using System.Collections.Generic;
using System.Configuration;

namespace PdfService.AppConfigManager
{
    public static class AppConfigWriter
    {
        public static void UpdateAppConfigKey(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(key);
        }

        public static void UpdateAppConfigKeys(Dictionary<string, string> configKeys)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var configKey in configKeys)
            {
                config.AppSettings.Settings.Remove(configKey.Key);
                config.AppSettings.Settings.Add(configKey.Key, configKey.Value);
            }

            config.Save(ConfigurationSaveMode.Modified);
            foreach (var key in configKeys.Keys)
            {
                ConfigurationManager.RefreshSection(key);
            }
        }
    }
}
