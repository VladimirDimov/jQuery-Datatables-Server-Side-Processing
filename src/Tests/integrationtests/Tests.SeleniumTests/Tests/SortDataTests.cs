using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using OpenQA.Selenium;
using TestData.Models;
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

            // Assert that rows are in correct order for the first page
            AssertTextPropertyOrder(columnName, direction, tableElement);
            tableElement.GoToLastPage();
            // Assert that rows are in correct order for the last page
            AssertTextPropertyOrder(columnName, direction, tableElement);
        }

        [Test]
        [TestCase(nameof(AllTypesModel.Integer), SortDirectionsEnum.Asc, typeof(int))]
        [TestCase(nameof(AllTypesModel.Integer), SortDirectionsEnum.Desc, typeof(int))]
        [TestCase(nameof(AllTypesModel.IntegerNullable), SortDirectionsEnum.Asc, typeof(int?))]
        [TestCase(nameof(AllTypesModel.UInt), SortDirectionsEnum.Asc, typeof(uint))]
        [TestCase(nameof(AllTypesModel.UIntNullable), SortDirectionsEnum.Asc, typeof(uint?))]
        //TODO: Add other types to test cases
        public void Sort_SholdWorkAppropriateForNonTextTypes(string columnName, SortDirectionsEnum direction, Type propertyType)
        {
            this.navigator.AllTypesDataPage().GoTo();
            var tableElement = new TableElement("table", this.driver);
            tableElement.ClickSortButton(columnName, direction);

            AssertNonTextPropertyOrder(columnName, direction, propertyType, tableElement);
            tableElement.GoToLastPage();
            AssertNonTextPropertyOrder(columnName, direction, propertyType, tableElement);
        }

        private void AssertNonTextPropertyOrder(string columnName, SortDirectionsEnum direction, Type propertyType, TableElement tableElement)
        {
            var actualColumnValues = tableElement.GetColumnRowValues(columnName);

            var stringParseFunction = this.GetStringParseFunction(columnName, propertyType);
            var expectedColumnValues = direction == SortDirectionsEnum.Asc ?
                actualColumnValues.OrderBy(stringParseFunction) :
                actualColumnValues.OrderByDescending(stringParseFunction);

            Assert.IsNotEmpty(actualColumnValues);
            Assert.IsTrue(expectedColumnValues.SequenceEqual(actualColumnValues));
        }

        private Func<string, object> GetStringParseFunction(string property, Type toType)
        {
            if (toType == typeof(uint?))
            {
                return new Func<string, object>(x =>
                {
                    uint i;
                    return uint.TryParse(x, out i) ? (uint?)i : null;
                });
            }
            else if (toType == typeof(int?))
            {
                return new Func<string, object>(x =>
                {
                    int i;
                    return int.TryParse(x, out i) ? (int?)i : null;
                });
            }

            var stringParam = Expression.Parameter(typeof(string), "x");
            var parseMethodInfo = toType.GetMethods().Where(x => x.Name == "Parse").First(x => x.GetParameters().Count() == 1);
            var parseMethodCallExpr = Expression.Call(null, parseMethodInfo, stringParam);
            var convertExpr = Expression.Convert(parseMethodCallExpr, typeof(object));
            var lambda = Expression.Lambda(convertExpr, stringParam);

            return (Func<string, object>)lambda.Compile();
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
    }
}