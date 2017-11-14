namespace Tests.SeleniumTests.Pages
{
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    internal class SimpleDataPage
    {
        private IWebDriver driver;

        public SimpleDataPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IReadOnlyCollection<IWebElement> GetRowElements()
        {
            return driver.FindElements(By.CssSelector("tbody tr"));
        }

        internal void SetPageLength(int length)
        {
            var pageLengthDropDownElement = driver.FindElement(By.CssSelector("[name=table-simple_length]"));
            var pageLengthSelect = new SelectElement(pageLengthDropDownElement);
            pageLengthSelect.SelectByValue(length.ToString());
        }
    }
}