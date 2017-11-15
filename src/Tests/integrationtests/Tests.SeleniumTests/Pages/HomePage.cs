namespace Tests.SeleniumTests.Pages
{
    using OpenQA.Selenium;
    using Tests.SeleniumTests.Common;

    internal class HomePage
    {
        private IWebDriver driver;
        private ISettingsProvider settings;

        public HomePage(IWebDriver driver, ISettingsProvider settings)
        {
            this.driver = driver;
            this.settings = settings;
        }

        public HomePage GoTo()
        {
            driver.Navigate().GoToUrl(this.settings["serverUrl"]);

            return this;
        }

        public SimpleDataNoPagingPage GoToSimpleDataNoPagingPage()
        {
            driver.Navigate().GoToUrl(this.settings["serverUrl"] + "/home/SimpleDataNoPagingTestPage");

            return new SimpleDataNoPagingPage(this.driver);
        }

        public SimpleDataPage GoToSimpleDataPage()
        {
            this.driver.Navigate().GoToUrl(this.settings["serverUrl"] + "home/SimpleDataTestPage");

            return new SimpleDataPage(driver);
        }

        public ComplexDataPage GoToComplexDataNoPagingPage()
        {
            this.driver.Navigate().GoToUrl(this.settings["serverUrl"] + "home/ComplexDataTestPage?isPaged=false");

            return new ComplexDataPage(driver);
        }

        internal ComplexDataPage GoToComplexDataPage()
        {
            this.driver.Navigate().GoToUrl(this.settings["serverUrl"] + "home/ComplexDataTestPage");

            return new ComplexDataPage(driver);
        }
    }
}