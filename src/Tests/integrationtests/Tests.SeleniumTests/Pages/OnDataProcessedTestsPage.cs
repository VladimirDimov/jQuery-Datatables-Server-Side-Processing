namespace Tests.SeleniumTests.Pages
{
    using Common;
    using OpenQA.Selenium;

    public class OnDataProcessedTestsPage : IWebPage
    {
        private readonly IWebDriver driver;

        public OnDataProcessedTestsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void GoTo(string queryString = null)
        {
            this.driver.Navigate().GoToUrl(GlobalConstants.ServerBaseUrl + GlobalConstants.OnDataProcessedTestPageRelativeUrl + queryString);
        }
    }
}