using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Tests.SeleniumTests.Pages
{
    public class SimpleDataNoPagingPage
    {
        private IWebDriver driver;

        public SimpleDataNoPagingPage(IWebDriver driver)
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