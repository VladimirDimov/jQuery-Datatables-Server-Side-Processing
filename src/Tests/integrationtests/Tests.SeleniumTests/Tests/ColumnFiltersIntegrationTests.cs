namespace Tests.SeleniumTests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using global::Tests.SeleniumTests.Enumerations;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using SeleniumTests.Common;
    using TestData.Models;

    internal class ColumnFiltersIntegrationTests
    {
        private IWebDriver driver;
        private Navigator navigator;

        [SetUp]
        public void SetUp()
        {
            this.driver = DriverSingletonProvider.GetDriver();
            this.navigator = new Navigator(driver);
        }

        private static readonly object[] searchByColumnShouldWorkProperlyNonDateTimesParameters =
        {
            new object[]  { GetExpression(x => x.StringProperty) , ComparissonTypesEnum.Contains },
            new object[]  { GetExpression(x => x.IntegerNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.IntegerNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.UInt) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.UIntNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.Long) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.LongNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.ULong) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.ULongNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.Short) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.ShortNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.UShort) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.UShortNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.ByteProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.ByteNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.SByteProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.SByteNullable) , ComparissonTypesEnum.Equal },
            //new object[]  { GetExpression(x => x.DoubleProperty) , ComparissonTypesEnum.Equal },
            //new object[]  { GetExpression(x => x.DoubleNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.DecimalProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.DecimalNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.BooleanProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.BooleanNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.CharProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.CharNullable) , ComparissonTypesEnum.Equal },
            // ===================================================================================
            new object[]  { GetExpression(x => x.NestedModel.StringProperty) , ComparissonTypesEnum.Contains },
            new object[]  { GetExpression(x => x.NestedModel.IntegerNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.IntegerNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.UInt) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.UIntNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.Long) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.LongNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.ULong) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.ULongNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.Short) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.ShortNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.UShort) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.UShortNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.ByteProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.ByteNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.SByteProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.SByteNullable) , ComparissonTypesEnum.Equal },
            //new object[]  { GetExpression(x => x.NestedModel.DoubleProperty) , ComparissonTypesEnum.Equal },
            //new object[]  { GetExpression(x => x.NestedModel.DoubleNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.DecimalProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.DecimalNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.BooleanProperty) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.BooleanNullable) , ComparissonTypesEnum.Equal },
            new object[]  { GetExpression(x => x.NestedModel.CharProperty) , ComparissonTypesEnum.Contains },
            new object[]  { GetExpression(x => x.NestedModel.CharNullable) , ComparissonTypesEnum.Contains },
        };

        [Test, TestCaseSource(nameof(searchByColumnShouldWorkProperlyNonDateTimesParameters))]
        public void FilterByColumnShouldWorkProperlyForNonDateTimes(Expression<Func<AllTypesModel, object>> selector, ComparissonTypesEnum comparissonType)
        {
            string columnName = this.GetColumnName(selector);
            if (selector.ToString().ToLower().Contains("nestedmodel"))
            {
                columnName = "Nested Model " + columnName;
            }

            this.navigator.AllTypesDataPage().GoTo();
            var inputId = "column-search-" + columnName.Replace(' ', '-');
            var table = new TableElement("table", this.driver);
            var filterValue = this.GetRandomValue(selector.Compile());
            table.TypeInInput($"#{inputId}", filterValue.ToString());
            var columnValues = table.GetColumnRowValues(columnName);
            this.AssertColumnValues(columnValues, filterValue, comparissonType);
        }

        private static readonly object[] searchByColumnShouldWorkProperlyDateTimesParameters =
        {
            new object[]  { GetExpression(x => x.DateTimeProperty) , ComparissonTypesEnum.DateTime },
            new object[]  { GetExpression(x => x.DateTimeNullable) , ComparissonTypesEnum.DateTime },
            new object[]  { GetExpression(x => x.DateTimeOffsetProperty) , ComparissonTypesEnum.DateTimeOffset },
            new object[]  { GetExpression(x => x.DateTimeOffsetNullable) , ComparissonTypesEnum.DateTimeOffset },
        };

        [Test, TestCaseSource(nameof(searchByColumnShouldWorkProperlyDateTimesParameters))]
        public void FilterByColumnShouldWorkProperlyForDateTimes(Expression<Func<AllTypesModel, object>> selector, ComparissonTypesEnum comparissonType)
        {
            string columnName = this.GetColumnName(selector);
            this.navigator.AllTypesDataPage().GoTo();
            var inputId = "column-search-" + columnName.Replace(' ', '-');
            var table = new TableElement("table", this.driver);
            var filterValueObj = this.GetRandomValue(selector.Compile());
            var filterValueDT = comparissonType == ComparissonTypesEnum.DateTime ? (DateTime)filterValueObj : ((DateTimeOffset)filterValueObj).DateTime;

            table.TypeInInput($"#{inputId}", filterValueDT.ToString("r"));
            var columnValues = table.GetColumnRowValues(columnName);
            this.AssertColumnValues(columnValues, filterValueObj, comparissonType);
        }

        private static readonly object[] filterByMultipleColumnShouldWorkProperlyParameters =
        {
            new object[]  { "Two numerics", new Expression<Func<AllTypesModel, object>>[] {x => x.Integer,x => x.Long} },
            new object[]  { "Numeric and DateTime", new Expression<Func<AllTypesModel, object>>[] {x => x.Integer,x => x.DateTimeProperty} },
            new object[]  { "String and numeric", new Expression<Func<AllTypesModel, object>>[] {x => x.StringProperty,x => x.Long} },
        };

        [Test, TestCaseSource(nameof(filterByMultipleColumnShouldWorkProperlyParameters))]
        public void FilterByMultipleColumnShouldWorkProperly(string testCase, Expression<Func<AllTypesModel, object>>[] selectors)
        {
            this.navigator.AllTypesDataPage().GoTo();

            string firstColumnName = this.GetColumnName(selectors.First());
            var firstInputId = "column-search-" + firstColumnName.Replace(' ', '-');
            var table = new TableElement("table", this.driver);
            var filterValue = this.GetRandomValue(selectors.First().Compile());
            table.TypeInInput($"#{firstInputId}", filterValue.ToString());

            string secondColumnName = this.GetColumnName(selectors.Last());
            var secondColumnValues = table.GetColumnRowValues(secondColumnName);
            var secondFilterValue = secondColumnValues.First();
            var secondInputId = "column-search-" + secondColumnName.Replace(' ', '-');
            table.TypeInInput($"#{secondInputId}", secondFilterValue);

            var columnValues = table.GetColumnRowValues(firstColumnName);
            this.AssertColumnValues(columnValues, filterValue, ComparissonTypesEnum.Equal);
            columnValues = table.GetColumnRowValues(secondColumnName);
            this.AssertColumnValues(columnValues, secondFilterValue, ComparissonTypesEnum.Equal);
        }

        private static Expression<Func<AllTypesModel, object>> GetExpression(Expression<Func<AllTypesModel, object>> expr)
        {
            return expr;
        }

        private string GetColumnName(Expression selector)
        {
            var nodeType = selector.NodeType;
            switch (nodeType)
            {
                case ExpressionType.Lambda:
                    var body = ((LambdaExpression)selector).Body;
                    return GetColumnName(body);

                case ExpressionType.MemberAccess:
                    var member = ((MemberExpression)selector).Member;
                    return member.Name;

                case ExpressionType.Convert:
                    var convExpr = ((UnaryExpression)selector).Operand;
                    return GetColumnName(convExpr);

                default:
                    break;
            }

            throw new ArgumentException();
        }

        private void AssertColumnValues(IEnumerable<string> columnValues, object filterValue, ComparissonTypesEnum comparissonType)
        {
            Assert.IsNotEmpty(columnValues);

            switch (comparissonType)
            {
                case ComparissonTypesEnum.Equal:
                    foreach (var colValue in columnValues)
                    {
                        Assert.IsTrue(colValue.ToLower() == filterValue.ToString().ToLower());
                    }

                    break;

                case ComparissonTypesEnum.Contains:
                    foreach (var colValue in columnValues)
                    {
                        Assert.IsTrue(colValue.ToLower().Contains(filterValue.ToString().ToLower()));
                    }

                    break;

                case ComparissonTypesEnum.DateTime:
                    var compareToDT = ((DateTime)filterValue).ToUniversalTime();

                    Assert.IsTrue(columnValues.All(x =>
                    {
                        var parsedItem = DateTime.Parse(x).ToUniversalTime();

                        return
                            parsedItem.Year == compareToDT.Year &&
                            parsedItem.Month == compareToDT.Month &&
                            parsedItem.Day == compareToDT.Day &&
                            parsedItem.Hour == compareToDT.Hour &&
                            parsedItem.Minute == compareToDT.Minute &&
                            parsedItem.Second == compareToDT.Second;
                    }));
                    break;

                case ComparissonTypesEnum.DateTimeOffset:
                    var compareToDTOfS = ((DateTimeOffset)filterValue).UtcDateTime;

                    Assert.IsTrue(columnValues.All(x =>
                    {
                        var parsedItem = DateTime.Parse(x).ToUniversalTime();

                        return
                            parsedItem.Year == compareToDTOfS.Year &&
                            parsedItem.Month == compareToDTOfS.Month &&
                            parsedItem.Day == compareToDTOfS.Day &&
                            parsedItem.Hour == compareToDTOfS.Hour &&
                            parsedItem.Minute == compareToDTOfS.Minute &&
                            parsedItem.Second == compareToDTOfS.Second;
                    }));
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        private T GetRandomValue<T>(Func<AllTypesModel, T> selector)
        {
            var random = new Random();
            var data = Data.GetDataFromServer<IEnumerable<AllTypesModel>>(GlobalConstants.ServerBaseUrl + GlobalConstants.AllTypesModelFullDataUrl);
            var dataLength = data.Count();
            var startIndex = random.Next((int)(0.3 * dataLength), (int)(0.7 * dataLength));
            var lookInDataPart = data.Skip(startIndex);
            var item = lookInDataPart.First(x => selector(x) != null && selector.ToString() != string.Empty);
            var value = selector(item);

            return value;
        }
    }
}