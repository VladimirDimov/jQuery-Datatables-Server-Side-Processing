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
        public void EqualShouldWorkProperly(Expression<Func<AllTypesModel, object>> selector)
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
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.gt },
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gt },
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gt },
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.gte },
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gte },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.gte },
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.gte },
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lt },
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lt },
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lt },
            // =================================================================================================================================================
            new object[]{ GetExpression(x => x.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte },
            new object[]{ GetExpression(x => x.DecimalNullable), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte },
            new object[]{ GetExpression(x => x.DoubleProperty), new Func<string, IComparable>(x => double.Parse(x)), RangeOperationTypesEnum.lte },
            new object[]{ GetExpression(x => x.CharProperty), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lte },
            new object[]{ GetExpression(x => x.CharNullable), new Func<string, IComparable>(x => char.Parse(x)), RangeOperationTypesEnum.lte },
            new object[]{ GetExpression(x => x.NestedModel.DecimalProperty), new Func<string, IComparable>(x => decimal.Parse(x)), RangeOperationTypesEnum.lte },

        };

        [Test, TestCaseSource(nameof(singleRangeConditionShouldWorkProperlyCases))]
        public void SingleRangeConditionShouldWorkProperly(Expression<Func<AllTypesModel, IComparable>> selector, Func<string, IComparable> parseFunc, RangeOperationTypesEnum operationType)
        {
            this.navigator.AllTypesDataPage().GoTo();

            var colName = this.GetColumnName(selector);
            var filterValue = this.GetRandomValue(selector.Compile());
            var table = new TableElement("table", this.driver);
            var filterContainer = new CustomFilterContainer(this.driver, "#custom-filters-container");

            switch (operationType)
            {
                case RangeOperationTypesEnum.gt:
                    filterContainer.Gt(colName, filterValue.ToString());
                    break;

                case RangeOperationTypesEnum.gte:
                    filterContainer.Gte(colName, filterValue.ToString());
                    break;

                case RangeOperationTypesEnum.lt:
                    filterContainer.Lt(colName, filterValue.ToString());
                    break;

                case RangeOperationTypesEnum.lte:
                    filterContainer.Lte(colName, filterValue.ToString());
                    break;

                case RangeOperationTypesEnum.eq:
                    filterContainer.Eq(colName, filterValue.ToString());
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