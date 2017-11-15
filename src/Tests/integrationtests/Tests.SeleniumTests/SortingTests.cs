namespace Tests.SeleniumTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TestData.Data.Models;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages;

    internal class SortingTests
    {
        private AppSettingsProvider settings;
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            this.settings = new AppSettingsProvider();
            this.driver = DriverProvider.GetDriver();
        }

        [Test]
        public void SortingSimpleDataByStringNoPagingShouldWorkProperly()
        {
            List<string> expectedData = null;

            this.AssserSorting((isAsc, fullData, colValues) =>
            {
                if (isAsc)
                {
                    expectedData = fullData.OrderBy(x => x.String).Select(x => x.String).ToList();
                }
                else
                {
                    expectedData = fullData.OrderByDescending(x => x.String).Select(x => x.String).ToList();
                }

                Assert.IsTrue(colValues.SequenceEqual(expectedData));
            },
            "String");
        }

        [Test]
        public void SortingSimpleDataByIntegerNoPagingShouldWorkProperly()
        {
            IEnumerable<string> expectedData = null;

            this.AssserSorting((isAsc, fullData, colValues) =>
            {
                if (isAsc)
                {
                    expectedData = fullData.OrderBy(x => x.Integer).Select(x => x.Integer.ToString());
                }
                else
                {
                    expectedData = fullData.OrderByDescending(x => x.Integer).Select(x => x.Integer.ToString());
                }

                Assert.IsTrue(colValues.SequenceEqual(expectedData));
            },
            "Integer");
        }

        [Test]
        public void SortingSimpleDataByDoubleNoPagingShouldWorkProperly()
        {
            List<string> expectedData = null;

            this.AssserSorting((isAsc, fullData, colValues) =>
            {
                if (isAsc)
                {
                    expectedData = fullData.OrderBy(x => x.Double).Select(x => x.Double.ToString()).ToList();
                }
                else
                {
                    expectedData = fullData.OrderByDescending(x => x.Double).Select(x => x.Double.ToString()).ToList();
                }

                for (int i = 0; i < expectedData.Count(); i++)
                {
                    var difference = double.Parse(expectedData[i]) - Double.Parse(colValues[i]);
                    Assert.IsTrue(-0.000001 < difference && difference < 0.000001);
                }
            },
            "Double");
        }

        private void AssserSorting(Action<bool, List<SimpleDataModel>, List<string>> assert, string colName)
        {
            var homePage = new HomePage(driver, settings);
            var simpleDataPage = homePage.GoTo().GoToSimpleDataNoPagingPage();
            var columnHeaderElements = simpleDataPage.GetColumnHeaderElements();
            var tableElement = simpleDataPage.GetTable();
            var allData = DataHelpers.GetSimpleDataFull(this.settings).ToList();
            var headerElement = columnHeaderElements.Single(x => x.Text == colName);
            for (int i = 0; i < 2; i++)
            {
                headerElement.Click();
                var headerText = headerElement.Text;
                var columnValues = TableHelpers.GetTableColumnValues(tableElement, headerText).ToList();
                var isAsc = headerElement.HasClass("sorting_asc");
                assert(isAsc, allData, columnValues);
            }
        }
    }
}