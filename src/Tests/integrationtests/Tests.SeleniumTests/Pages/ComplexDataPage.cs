namespace Tests.SeleniumTests.Pages
{
    using System.Collections.Generic;
    using OpenQA.Selenium;

    public class ComplexDataPage
    {
        private IWebDriver driver;

        public ComplexDataPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        internal IWebElement GetFilterInputElement()
        {
            return this.driver.FindElement(By.CssSelector("#table-simple_filter input"));
        }

        internal IReadOnlyCollection<IWebElement> GetRowElements()
        {
            return this.driver.FindElements(By.CssSelector("tbody tr"));
        }
    }
}