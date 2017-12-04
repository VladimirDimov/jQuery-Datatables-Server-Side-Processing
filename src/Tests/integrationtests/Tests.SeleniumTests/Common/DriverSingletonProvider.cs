using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests.SeleniumTests.Common
{
    internal static class DriverSingletonProvider
    {
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            if (driver == null)
            {
                driver = GetNewDriverInstance();
                driver.Manage().Timeouts().PageLoad = new System.TimeSpan(0, 0, 5);
            }

            return driver;
        }

        public static void Dispose()
        {
            if (driver == null)
            {
                return;
            }

            driver.Dispose();
            driver = null;
        }

        private static IWebDriver GetNewDriverInstance()
        {
            var options = new ChromeOptions
            {
                UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss,
                AcceptInsecureCertificates = true,
            };

            return new ChromeDriver(options);
        }
    }
}