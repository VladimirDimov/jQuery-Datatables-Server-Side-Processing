namespace Tests.SeleniumTests.ExtensionMethods
{
    using System.Collections.ObjectModel;
    using OpenQA.Selenium;

    public static class SearchContextExtensionMethods
    {
        public static IWebElement FindElementByCssSelector(this ISearchContext context, string selector)
        {
            return context.FindElement(By.CssSelector(selector));
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByCssSelector(this ISearchContext context, string selector)
        {
            return context.FindElements(By.CssSelector(selector));
        }
    }
}