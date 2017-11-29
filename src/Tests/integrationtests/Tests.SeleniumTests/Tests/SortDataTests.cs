using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Tests.Helpers;
using Tests.SeleniumTests.Common;
using Tests.SeleniumTests.Enumerations;

namespace Tests.SeleniumTests.Tests
{
    public class SortDataTests
    {
        private IWebDriver driver;
        private Navigator navigator;

        [SetUp]
        public void SetUp()
        {
            this.driver = DriverSingletonProvider.GetDriver();
            this.navigator = new Navigator(driver);
        }

        [Test]
        [TestCase("StringProperty", SortDirectionsEnum.Asc)]
        [TestCase("StringProperty", SortDirectionsEnum.Desc)]
        [TestCase("CharProperty", SortDirectionsEnum.Asc)]
        [TestCase("CharProperty", SortDirectionsEnum.Desc)]
        [TestCase("CharNullable", SortDirectionsEnum.Asc)]
        [TestCase("CharNullable", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model StringProperty", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model StringProperty", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model CharProperty", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model CharProperty", SortDirectionsEnum.Desc)]
        [TestCase("Nested Model CharNullable", SortDirectionsEnum.Asc)]
        [TestCase("Nested Model CharNullable", SortDirectionsEnum.Desc)]
        public void Sort_SholdWorkAppropriateForTextTypes(string columnName, SortDirectionsEnum direction)
        {
            this.navigator.AllTypesDataPage().GoTo();
            var tableElement = new TableElement("table", this.driver);
            tableElement.ClickSortButton(columnName, direction);

            AssertTextPropertyOrder(columnName, direction, tableElement);
            tableElement.GoToLastPage();
            AssertTextPropertyOrder(columnName, direction, tableElement);
        }

        public void Sort_SholdWorkAppropriateForNonTextTypes(string columnName, SortDirectionsEnum direction)
        {
            this.navigator.AllTypesDataPage().GoTo();
            var tableElement = new TableElement("table", this.driver);
            tableElement.ClickSortButton(columnName, direction);

            var actualColumnValues = tableElement.GetColumnRowValues(columnName);

            var expectedColumnValues = direction == SortDirectionsEnum.Asc ?
                actualColumnValues.OrderBy(x => x) :
                actualColumnValues.OrderByDescending(x => x);

            Assert.IsNotEmpty(actualColumnValues);
            Assert.IsTrue(expectedColumnValues.SequenceEqual(actualColumnValues));
        }

        private static void AssertTextPropertyOrder(string columnName, SortDirectionsEnum direction, TableElement tableElement)
        {
            var actualColumnValues = tableElement.GetColumnRowValues(columnName);

            var expectedColumnValues = direction == SortDirectionsEnum.Asc ?
                actualColumnValues.OrderBy(x => x) :
                actualColumnValues.OrderByDescending(x => x);

            Assert.IsNotEmpty(actualColumnValues);
            Assert.IsTrue(expectedColumnValues.SequenceEqual(actualColumnValues));
        }

        private void AssertTextPropertiesOrder(IEnumerable<string> actualColumnValues)
        {
        }
    }
}