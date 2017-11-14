namespace Tests.SeleniumTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;

    public static class TableHelpers
    {
        public static IEnumerable<string> GetTableColumnValues(IWebElement table, string column)
        {
            var headers = table.FindElements(By.CssSelector("thead th"));
            var colIndex = headers.IndexOf(x => x.Text.ToLower() == column.ToLower());

            return table
                .FindElements(By.CssSelector($"tbody tr td:nth-child({colIndex + 1})"))
                .Select(x => x.Text);
        }

        internal static void ClickPageNumber(IWebDriver driver, string page)
        {
            var pageElements = driver.FindElements(By.CssSelector(".dataTables_paginate .paginate_button"));
            var pageElement = pageElements.First(x => x.Text.ToLower() == page.ToLower());
            pageElement.Click();
        }
    }
}