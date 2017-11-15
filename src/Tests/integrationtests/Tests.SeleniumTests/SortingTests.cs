namespace Tests.SeleniumTests
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Tests.SeleniumTests.Common;

    internal class SortingTests
    {
        private AppSettingsProvider settings;
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            this.settings = new AppSettingsProvider();
            this.driver = DriverProvider.GetDriver();
        }
    }
}