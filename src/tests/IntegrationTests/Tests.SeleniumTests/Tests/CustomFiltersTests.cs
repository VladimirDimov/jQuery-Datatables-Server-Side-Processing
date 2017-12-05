namespace Tests.SeleniumTests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Threading;
    using global::Tests.SeleniumTests.Enumerations;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using SeleniumTests.Common;
    using TestData.Models;

    public class CustomFiltersTests
    {
        private IWebDriver driver;
        private Navigator navigator;

        [SetUp]
        public void SetUp()
        {
            this.driver = DriverSingletonProvider.GetDriver();
            this.navigator = new Navigator(driver);
        }

        private static new object[] equalTestCases = new object[]
        {
            new object[]{ GetExpression(x => x.Integer) },
            new object[]{ GetExpression(x => x.IntegerNullable) },
            new object[]{ GetExpression(x => x.StringProperty) },
            new object[]{ GetExpression(x => x.CharProperty) },
            new object[]{ GetExpression(x => x.CharNullable) },

            new object[]{ GetExpression(x => x.NestedModel.Integer) },
        };

        [Test, TestCaseSource(nameof(equalTestCases))]
        public void EqualShouldWorkProperly(Expression<Func<AllTypesModel, IComparable>> selector)
        {
            this.navigator.AllTypesDataPage().GoTo();

            var colName = this.GetColumnName(selector);
            var filterValue = this.GetRandomValue(selector.Compile());
            var table = new TableElement("table", this.driver);
            var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
            filterContainer.Eq(colName, filterValue.ToString());
            var columnFilteredValues = table.GetColumnRowValues(colName);

            Assert.IsTrue(columnFilteredValues.All(x => x.ToLower() == filterValue.ToString().ToLower()));
        }

        private static new object[] singleRangeConditionShouldWorkProperlyCases = new object[]
        {
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.gt, null  },
#if USE_CHARTYPE
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
#if USE_DTOFFSET
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
#endif
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte, null  },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.gte, null  },
#if USE_CHARTYPE
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gte , null },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gte, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
#if USE_DTOFFSET
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
#endif
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lt, null  },
#if USE_CHARTYPE
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
#if USE_DTOFFSET
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
#endif
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lte, null  },
#if USE_CHARTYPE
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lte, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lte, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lte, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
#if USE_DTOFFSET
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
#endif
        };

        [Test, TestCaseSource(nameof(singleRangeConditionShouldWorkProperlyCases))]
        public void SingleRangeConditionShouldWorkProperly(Expression<Func<AllTypesModel, IComparable>> selector, Func<string, IComparable> parseFunc, RangeOperationTypesEnum operationType, Func<object, string> inputFormat = null)
        {
            this.navigator.AllTypesDataPage().GoTo();

            var colName = this.GetColumnName(selector);
            var filterValue = this.GetRandomValue(selector.Compile());
            var table = new TableElement("table", this.driver);
            var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
            inputFormat = inputFormat ?? new Func<object, string>(x => x.ToString());
            switch (operationType)
            {
                case RangeOperationTypesEnum.gt:
                    filterContainer.Gt(colName, inputFormat(filterValue));
                    break;

                case RangeOperationTypesEnum.gte:
                    filterContainer.Gte(colName, inputFormat(filterValue));
                    break;

                case RangeOperationTypesEnum.lt:
                    filterContainer.Lt(colName, inputFormat(filterValue));
                    break;

                case RangeOperationTypesEnum.lte:
                    filterContainer.Lte(colName, inputFormat(filterValue));
                    break;

                case RangeOperationTypesEnum.eq:
                    filterContainer.Eq(colName, inputFormat(filterValue));
                    break;

                default:
                    break;
            }

            Thread.Sleep(GlobalConstants.GlobalThreadSleep);
            var columnFilteredValues = table.GetColumnRowValuesUntilAny(colName);
            var parsedValues = columnFilteredValues.Select(parseFunc);

            Assert.IsNotEmpty(parsedValues);
            var filterparsedValue = parseFunc(filterValue.ToString());
            switch (operationType)
            {
                case RangeOperationTypesEnum.gt:
                    Assert.IsTrue(parsedValues.All(x => x.CompareTo(filterparsedValue) > 0));
                    break;

                case RangeOperationTypesEnum.gte:
                    Assert.IsTrue(parsedValues.All(x => x.CompareTo(filterparsedValue) >= 0));
                    break;

                case RangeOperationTypesEnum.lt:
                    Assert.IsTrue(parsedValues.All(x => x.CompareTo(filterparsedValue) < 0));
                    break;

                case RangeOperationTypesEnum.lte:
                    Assert.IsTrue(parsedValues.All(x => x.CompareTo(filterparsedValue) <= 0));
                    break;

                case RangeOperationTypesEnum.eq:
                    Assert.IsTrue(parsedValues.All(x => x.CompareTo(filterparsedValue) == 0));
                    break;

                default:
                    break;
            }
        }

        [Test]
        public void MultipleCustomFiltersOnSameColumn()
        {
            var column = "Integer";
            this.navigator.AllTypesDataPage().GoTo();
            var table = new TableElement("table", this.driver);
            var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
            filterContainer.Gt(column, "5");
            filterContainer.Lt(column, "10");

            table.ClickSortButton(column);
            Thread.Sleep(GlobalConstants.GlobalThreadSleep);
            var resultColumnValues = table.GetColumnRowValuesUntilAny(column).Select(x => int.Parse(x));
            Assert.IsTrue(resultColumnValues.All(x => 5 < x && x < 10));

            table.ClickSortButton(column);
            Thread.Sleep(GlobalConstants.GlobalThreadSleep);
            resultColumnValues = table.GetColumnRowValuesUntilAny(column).Select(x => int.Parse(x));
            Assert.IsTrue(resultColumnValues.All(x => 5 < x && x < 10));
        }

        [Test]
        public void MultipleCustomFiltersOnTwoColumns()
        {
            var firstColumn = "Integer";
            var secondColumn = "IntegerNullable";

            this.navigator.AllTypesDataPage().GoTo();
            var table = new TableElement("table", this.driver);
            var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
            filterContainer.Gt(firstColumn, "5");
            filterContainer.Lt(firstColumn, "70");
            filterContainer.Gte(secondColumn, "20");
            filterContainer.Lte(secondColumn, "50");

            table.ClickSortButton(firstColumn);
            var resultFirstColumnValues = table.GetColumnRowValuesUntilAny(firstColumn).Select(x => int.Parse(x));
            Assert.IsTrue(resultFirstColumnValues.All(x => 5 < x && x < 70));

            table.ClickSortButton(firstColumn);
            resultFirstColumnValues = table.GetColumnRowValuesUntilAny(firstColumn).Select(x => int.Parse(x));
            Assert.IsTrue(resultFirstColumnValues.All(x => 5 < x && x < 70));

            table.ClickSortButton(secondColumn);
            var resultsecondColumnValues = table.GetColumnRowValuesUntilAny(secondColumn).Select(x => int.Parse(x));
            Assert.IsTrue(resultsecondColumnValues.All(x => 20 <= x && x <= 50));

            table.ClickSortButton(firstColumn);
            resultsecondColumnValues = table.GetColumnRowValuesUntilAny(secondColumn).Select(x => int.Parse(x));
            Assert.IsTrue(resultsecondColumnValues.All(x => 20 <= x && x <= 50));
        }

        private static Expression<Func<AllTypesModel, IComparable>> GetExpression(Expression<Func<AllTypesModel, IComparable>> expr)
        {
            return expr;
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
    }
}