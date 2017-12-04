namespace Tests.SeleniumTests.ExtensionMethods
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using OpenQA.Selenium;

    public static class SearchContextExtensionMethods
    {
        public static IWebElement FindElementByCssSelector(this ISearchContext context, string selector)
        {
            Exception outerEx = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    return context.FindElement(By.CssSelector(selector));
                }
                catch (System.Exception ex)
                {
                    outerEx = ex;
                    Thread.Sleep(1000);
                }
            }

            throw outerEx;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByCssSelector(this ISearchContext context, string selector)
        {
            Exception outerEx = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    return context.FindElements(By.CssSelector(selector));
                }
                catch (System.Exception ex)
                {
                    outerEx = ex;
                    Thread.Sleep(1000);
                }
            }

            throw outerEx;
        }
    }
}