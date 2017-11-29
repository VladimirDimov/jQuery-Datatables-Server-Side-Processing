using OpenQA.Selenium;
using Tests.SeleniumTests.Pages;

namespace Tests.SeleniumTests.Common
{
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
    }
}