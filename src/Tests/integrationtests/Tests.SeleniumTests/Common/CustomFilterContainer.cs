using System.Threading;
using OpenQA.Selenium;
using Tests.SeleniumTests.ExtensionMethods;

namespace Tests.SeleniumTests.Common
{
    public class CustomFilterContainer
    {
        private readonly IWebDriver driver;
        private readonly IWebElement filtersContainer;

        public CustomFilterContainer(IWebDriver driver, string selector)
        {
            this.driver = driver;
            this.filtersContainer = this.driver.FindElementByCssSelector(selector);
        }

        public void Eq(string property, string text)
        {
            this.GenericInput(property, text, "eq");
        }

        public void Gt(string property, string text)
        {
            this.GenericInput(property, text, "gt");
        }

        public void Gte(string property, string text)
        {
            this.GenericInput(property, text, "gte");
        }

        public void Lt(string property, string text)
        {
            this.GenericInput(property, text, "lt");
        }

        public void Lte(string property, string text)
        {
            this.GenericInput(property, text, "lte");
        }

        private void GenericInput(string property, string text, string filterType)
        {
            property = property.Replace('.', '-');
            var inputElement = this.driver.FindElementByCssSelector($"#custom-filter-{property} [data-{filterType}]");
            inputElement.SendKeys(text);
            inputElement.SendKeys(Keys.Enter);
            Thread.Sleep(GlobalConstants.GlobalThreadSleep);
        }
    }
}