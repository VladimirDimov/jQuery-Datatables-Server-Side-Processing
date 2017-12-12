namespace Tests.SeleniumTests.Common
{
    using OpenQA.Selenium;
    using SeleniumTests.Pages;

    public class Navigator
    {
        private readonly IWebDriver driver;

        public Navigator(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebPage AllTypesDataPage()
        {
            return new AllTypesDataPage(this.driver);
        }

        public IWebPage OnDataExecutedEventTestsPage()
        {
            return new OnDataProcessedTestsPage(this.driver);
        }
    }
}