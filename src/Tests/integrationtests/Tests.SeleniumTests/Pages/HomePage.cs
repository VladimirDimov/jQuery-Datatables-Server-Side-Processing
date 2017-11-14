namespace Tests.SeleniumTests.Pages
{
    using OpenQA.Selenium;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages.Contracts;

    internal class HomePage : IWebPage<HomePage>
    {
        private IWebDriver driver;
        private ISettingsProvider settings;

        public HomePage(IWebDriver driver, ISettingsProvider settings)
        {
            this.driver = driver;
            this.settings = settings;
        }

        public HomePage NavigateTo()
        {
            driver.Navigate().GoToUrl(this.settings["serverUrl"]);

            return this;
        }

        public SimpleDataPage SimpleDataPage()
        {
            this.driver.Navigate().GoToUrl(this.settings["serverUrl"] + "home/SimpleDataTestPage");

            return new SimpleDataPage(driver);
        }
    }
}