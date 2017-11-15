namespace Tests.SeleniumTests.Common
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    /// <summary>
    /// Provide singleton instance of <see cref="IWebDriver"/>
    /// </summary>
    internal static class DriverProvider
    {
        private static IWebDriver driver;

        public static IWebDriver GetDriver()
        {
            return driver;
        }

        public static void Dispose()
        {
            if (driver == null)
            {
                return;
            }

            driver.Dispose();
        }

        static DriverProvider()
        {
            DriverProvider.driver = new ChromeDriver();
        }
    }
}