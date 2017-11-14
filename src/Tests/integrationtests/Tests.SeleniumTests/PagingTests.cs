﻿namespace Tests.SeleniumTests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages;

    public class PagingTests
    {
        private AppSettingsProvider settings;

        [SetUp]
        public void SetUp()
        {
            this.settings = new AppSettingsProvider();
        }

        [Test]
        public void ExpectAppropriateNumberOfElement()
        {
            using (var driver = new ChromeDriver())
            {
                var homePage = new HomePage(driver, settings);
                var simpleDataPage = homePage.NavigateTo().SimpleDataPage();
                var rowElements = simpleDataPage.GetRowElements();

                Assert.AreEqual(10, rowElements.Count);

                simpleDataPage.SetPageLength(50);
                rowElements = simpleDataPage.GetRowElements();

                Assert.AreEqual(50, rowElements.Count);
            }
        }

        [Test]
        public void ExpectAppropriateElementsCollection()
        {
            using (var driver = new ChromeDriver())
            {
                var homePage = new HomePage(driver, settings);
                var simpleDataPage = homePage.NavigateTo().SimpleDataPage();

                var table = driver.FindElement(By.CssSelector("table"));
                var stringActualValues = TableHelpers.GetTableColumnValues(table, "String");

                var fullData = DataHelpers.GetSimpleDataFull(this.settings);
                var stringExpectedValues = fullData.OrderBy(x => x.String).Take(10).Select(x => x.String);
                var isAsExpected = stringActualValues.SequenceEqual(stringExpectedValues);

                Assert.IsTrue(isAsExpected);

                var pageNumber = 3;
                TableHelpers.ClickPageNumber(driver, pageNumber.ToString());
                stringExpectedValues = fullData.OrderBy(x => x.String).Page(pageNumber, 10).Select(x => x.String);
                stringActualValues = TableHelpers.GetTableColumnValues(table, "String");
                isAsExpected = stringActualValues.SequenceEqual(stringExpectedValues);

                Assert.IsTrue(isAsExpected);
            }
        }
    }
}