namespace Tests.Integration.Mvc.Configuration
{
    using System.Configuration;

    public static class SettingsProvider
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}