using System.Configuration;

namespace Tests.SeleniumTests.Common
{
    internal class AppSettingsProvider : ISettingsProvider
    {
        public string this[string index]
        {
            get
            {
                return ConfigurationManager.AppSettings[index];
            }
        }
    }
}