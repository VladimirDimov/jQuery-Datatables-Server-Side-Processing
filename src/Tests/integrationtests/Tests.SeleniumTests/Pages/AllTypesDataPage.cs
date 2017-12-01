namespace Tests.SeleniumTests.Pages
{
    using Common;
    using OpenQA.Selenium;

    public class AllTypesDataPage : IWebPage
    {
        private readonly IWebDriver driver;

        public AllTypesDataPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void GoTo(string queryString = null)
        {
            this.driver.Navigate().GoToUrl(GlobalConstants.ServerBaseUrl + GlobalConstants.AllTypesDataPageRelativeUrl + queryString);
        }
    }
}