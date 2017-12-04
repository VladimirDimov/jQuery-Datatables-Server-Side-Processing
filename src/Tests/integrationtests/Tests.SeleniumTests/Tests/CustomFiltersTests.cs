namespace Tests.SeleniumTests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
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
            ExceptionsHandler.Hande(() =>
            {
                this.navigator.AllTypesDataPage().GoTo();

                var colName = this.GetColumnName(selector);
                var filterValue = this.GetRandomValue(selector.Compile());
                var table = new TableElement("table", this.driver);
                var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
                filterContainer.Eq(colName, filterValue.ToString());
                var columnFilteredValues = table.GetColumnRowValues(colName);

                Assert.IsTrue(columnFilteredValues.All(x => x.ToLower() == filterValue.ToString().ToLower()));
            },
            this.driver);
        }

        private static new object[] singleRangeConditionShouldWorkProperlyCases = new object[]
        {
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.gt, null  },
#if USE_CHARPROP
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
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
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.gte, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lt, null  },
#if USE_CHARPROP
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt, null  },
#endif
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt, null  },
            new object[]{ GetExpression(x => x.DateTimeProperty), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeNullable), new Func<string, IComparable>(x => DateTime.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTime)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetProperty), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            new object[]{ GetExpression(x => x.DateTimeOffsetNullable), new Func<string, IComparable>(x => DateTimeOffset.Parse(x)), RangeOperationTypesEnum.lt, new Func<object, string>(x => ((DateTimeOffset)x).ToString("r")) },
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte, null  },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lte, null  },
#if USE_CHARPROP
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
            ExceptionsHandler.Hande(() =>
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

                var columnFilteredValues = table.GetColumnRowValues(colName);
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
            },
            this.driver);
        }

        [Test]
        public void MultipleCustomFiltersOnSameColumn()
        {
            ExceptionsHandler.Hande(() =>
            {
                var column = "Integer";
                this.navigator.AllTypesDataPage().GoTo();
                var table = new TableElement("table", this.driver);
                var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
                filterContainer.Gt(column, "5");
                filterContainer.Lt(column, "10");

                table.ClickSortButton(column);
                var resultColumnValues = table.GetColumnRowValues(column).Select(x => int.Parse(x));
                Assert.IsTrue(resultColumnValues.All(x => 5 < x && x < 10));

                table.ClickSortButton(column);
                resultColumnValues = table.GetColumnRowValues(column).Select(x => int.Parse(x));
                Assert.IsTrue(resultColumnValues.All(x => 5 < x && x < 10));
            },
            this.driver);
        }

        [Test]
        public void MultipleCustomFiltersOnTwoColumns()
        {
            ExceptionsHandler.Hande(() =>
            {
                var firstColumn = nameof(AllTypesModel.Integer);
                var secondColumn = nameof(AllTypesModel.IntegerNullable);

                this.navigator.AllTypesDataPage().GoTo();
                var table = new TableElement("table", this.driver);
                var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");
                var firstLeft = 5;
                var firstRight = 60;
                var secondLeft = 10;
                var secondRight = 40;
                filterContainer.Gt(firstColumn, firstLeft.ToString());
                filterContainer.Lt(firstColumn, firstRight.ToString());
                filterContainer.Gte(secondColumn, secondLeft.ToString());
                filterContainer.Lte(secondColumn, secondRight.ToString());

                table.ClickSortButton(firstColumn);
                var resultFirstColumnValues = table.GetColumnRowValues(firstColumn).Select(x => int.Parse(x));
                Assert.IsNotEmpty(resultFirstColumnValues.ToList());
                Assert.IsTrue(resultFirstColumnValues.All(x => firstLeft < x && x < firstRight));

                table.ClickSortButton(firstColumn);
                resultFirstColumnValues = table.GetColumnRowValues(firstColumn).Select(x => int.Parse(x));
                Assert.IsTrue(resultFirstColumnValues.All(x => firstLeft < x && x < firstRight));

                table.ClickSortButton(secondColumn);
                var resultsecondColumnValues = table.GetColumnRowValues(secondColumn).Select(x => int.Parse(x));
                Assert.IsTrue(resultsecondColumnValues.All(x => secondLeft <= x && x <= secondRight));

                table.ClickSortButton(firstColumn);
                resultsecondColumnValues = table.GetColumnRowValues(secondColumn).Select(x => int.Parse(x));
                Assert.IsTrue(resultsecondColumnValues.All(x => secondLeft <= x && x <= secondRight));
            },
            this.driver);
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
            var item = lookInDataPart.First(x => this.IsNotNull(selector, x));
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

        private bool IsNotNull<T>(Func<AllTypesModel, T> selector, AllTypesModel obj)
        {
            try
            {
                return selector(obj) != null && selector.ToString() != string.Empty;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}