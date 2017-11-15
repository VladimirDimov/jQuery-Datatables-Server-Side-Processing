namespace Tests.SeleniumTests
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages;

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

        [Test]
        public void SortingSimpleDataShouldWorkProperForAllColumns()
        {
            var homePage = new HomePage(driver, settings);
            var simpleDataPage = homePage.GoTo().GoToSimpleDataPage();
        }

        [Test]
        public void SortingComplexDataShouldWorkProperForAllColumns()
        {
            Assert.Fail("Not completed test...");
        }
    }
}