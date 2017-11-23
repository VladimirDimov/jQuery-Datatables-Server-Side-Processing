namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.Exceptions;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class CustomFiltersDataProcessorTests
    {
        private IDataProcess<ComplexModel> filter;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filter = resolver.GetCustomFiltersDataProcessor<ComplexModel>();
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForSimpleInteger(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForInteger("Integer", filterType, x => ((ComplexModel)x).Integer);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForNestedInteger(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForInteger("NestedComplexModel.SimpleModel.Integer", filterType, x => ((ComplexModel)x).NestedComplexModel.SimpleModel.Integer);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForSimpleDouble(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDouble("Double", filterType, x => ((ComplexModel)x).Double);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForNestedDouble(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDouble("NestedComplexModel.Double", filterType, x => ((ComplexModel)x).NestedComplexModel.Double);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForSimpleDateTime(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDateTime("DateTime", filterType, x => ((ComplexModel)x).DateTime);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForNestedDateTime(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDateTime("NestedComplexModel.SimpleModel.DateTime", filterType, x => ((ComplexModel)x).NestedComplexModel.SimpleModel.DateTime);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForChar(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForChar("Char", filterType, x => ((ComplexModel)x).Char);
        }

        [Test]
        [TestCase(FilterTypes.gt)]
        [TestCase(FilterTypes.lt)]
        [TestCase(FilterTypes.gte)]
        [TestCase(FilterTypes.lte)]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForNestedChar(FilterTypes filterType)
        {
            this.AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForChar("SimpleModel.Char", filterType, x => ((ComplexModel)x).SimpleModel.Char);
        }

        [Test]
        public void ShouldThrowAppropriateExceptionIfIncorrectPropertyType()
        {
            var exception = Assert.Throws<InvalidTypeForOperationException>(() =>
            {
                var value = "l";

                this.AssertFilter(
                    new List<string> { "String" },
                    new Dictionary<string, IEnumerable<FilterModel>>
                    {
                    { "String", new List<FilterModel>{ new FilterModel { Type = FilterTypes.gt, Value = value } } }
                    },
                    x => true);
            });
        }

        [Test]
        public void ShouldReturnCorrectExpressionOnSingleColumnGreaterThanForChar()
        {
            var value = (char)50;

            this.AssertFilter(
                new List<string> { "Char" },
                new Dictionary<string, IEnumerable<FilterModel>>
                {
                    { "Char", new List<FilterModel>{ new FilterModel { Type = FilterTypes.gt, Value = value.ToString() } } }
                },
                x => ((ComplexModel)x).Char > value);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnCorrectExpressionOnSingleColumnStrictRangeForInteger(bool isStrict)
        {
            const int Min = 30;
            const int Max = 60;
            string col = "Integer";
            Func<object, int> propSelect = x => ((ComplexModel)x).Integer;
            this.AssertSingleColumnStrinctRange<int>(Min, Max, col, x => ((ComplexModel)x).Integer, isStrict);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnCorrectExpressionOnSingleColumnStrictRangeForNestedInteger(bool isStrict)
        {
            const int Min = 30;
            const int Max = 60;
            string col = "NestedComplexModel.Integer";
            Func<object, int> propSelect = x => ((ComplexModel)x).Integer;
            this.AssertSingleColumnStrinctRange<int>(Min, Max, col, x => ((ComplexModel)x).NestedComplexModel.Integer, isStrict);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnCorrectExpressionOnSingleColumnStrictRangeForDouble(bool isStrict)
        {
            const double Min = 30 / 1000d;
            const double Max = 60 / 1000d;
            string col = "Double";
            Func<object, double> propSelect = x => ((ComplexModel)x).Double;
            this.AssertSingleColumnStrinctRange(Min, Max, col, x => ((ComplexModel)x).Double, isStrict);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnCorrectExpressionOnSingleColumnStrictRangeForNestedDouble(bool isStrict)
        {
            const double Min = 30 / 1000d;
            const double Max = 60 / 1000d;
            string col = "NestedComplexModel.Double";
            Func<object, double> propSelect = x => ((ComplexModel)x).Double;
            this.AssertSingleColumnStrinctRange(Min, Max, col, x => ((ComplexModel)x).NestedComplexModel.Double, isStrict);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReturnCorrectResultForFilterByRangeByMultipleProperties(bool isStrict)
        {
            const int MinInt = 30;
            const int MaxInt = 55;
            const double MinDouble = 50 / 1000d;
            const double MaxDouble = 60 / 1000d;
            var columns = new List<string> { "Integer", "" };
            Func<object, int> selectInt = x => ((ComplexModel)x).Integer;
            Func<object, double> selectDouble = x => ((ComplexModel)x).NestedComplexModel.Double;

            var filterModels = new Dictionary<string, IEnumerable<FilterModel>>
            {
                {
                    "Integer",
                    new List<FilterModel>
                    {
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.gt : FilterTypes.gte, Value = MinInt.ToString()
                        },
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.lt : FilterTypes.lte, Value = MaxInt.ToString()
                        }
                    }
                },

                {
                    "NestedComplexModel.Double",
                    new List<FilterModel>
                    {
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.gt : FilterTypes.gte, Value = MinDouble.ToString()
                        },
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.lt : FilterTypes.lte, Value = MaxDouble.ToString()
                        }
                    }
                }
            };

            if (isStrict)
            {
                this.AssertFilter(
                    columns,
                    filterModels,
                    x =>
                        MinInt < selectInt(x) && selectInt(x) < MaxInt &&
                        MinDouble < selectDouble(x) && selectDouble(x) < MaxInt);
            }
            else
            {
                this.AssertFilter(
                    columns,
                    filterModels,
                    x =>
                        MinInt <= selectInt(x) && selectInt(x) <= MaxInt &&
                        MinDouble <= selectDouble(x) && selectDouble(x) <= MaxInt);
            }
        }

        private void AssertSingleColumnStrinctRange<T>(T min, T max, string col, Func<object, T> propSelect, bool isStrict)
            where T : IComparable
        {
            var columns = new List<string> { col };
            var filterModels = new Dictionary<string, IEnumerable<FilterModel>>
            {
                {
                    col,
                    new List<FilterModel>
                    {
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.gt : FilterTypes.gte, Value = min.ToString()
                        },
                        new FilterModel
                        {
                            Type = isStrict ? FilterTypes.lt : FilterTypes.lte, Value = max.ToString()
                        }
                    }
                }
            };

            if (isStrict)
            {
                this.AssertFilter(columns, filterModels, x => propSelect(x).CompareTo(min) > 0 && propSelect(x).CompareTo(max) < 0);
            }
            else
            {
                this.AssertFilter(columns, filterModels, x => propSelect(x).CompareTo(min) >= 0 && propSelect(x).CompareTo(max) <= 0);
            }
        }

        private void AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForChar(string col, FilterTypes filterType, Func<object, char> selectProp)
        {
            char value = (char)50;
            Func<object, bool> predicate = null;

            switch (filterType)
            {
                case FilterTypes.gte:
                    predicate = x => selectProp(x) >= value;
                    break;

                case FilterTypes.gt:
                    predicate = x => selectProp(x) > value;
                    break;

                case FilterTypes.lt:
                    predicate = x => selectProp(x) < value;
                    break;

                case FilterTypes.lte:
                    predicate = x => selectProp(x) <= value;
                    break;

                default:
                    break;
            }

            this.AssertFilter(
                new List<string> { col },
                new Dictionary<string, IEnumerable<FilterModel>>
                {
                    { col, new List<FilterModel>{ new FilterModel { Type = filterType, Value = value.ToString() } } }
                },
                predicate);
        }

        private void AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForInteger(string col, FilterTypes filterType, Func<object, int> selectProp)
        {
            double value = 50;

            Func<object, bool> predicate = null;

            switch (filterType)
            {
                case FilterTypes.gte:
                    predicate = x => selectProp(x) >= value;
                    break;

                case FilterTypes.gt:
                    predicate = x => selectProp(x) > value;
                    break;

                case FilterTypes.lt:
                    predicate = x => selectProp(x) < value;
                    break;

                case FilterTypes.lte:
                    predicate = x => selectProp(x) <= value;
                    break;

                default:
                    break;
            }

            this.AssertFilter(
                new List<string> { col },
                new Dictionary<string, IEnumerable<FilterModel>>
                {
                    { col, new List<FilterModel>{ new FilterModel { Type = filterType, Value = value.ToString() } } }
                },
                predicate);
        }

        private void AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDouble(string col, FilterTypes filterType, Func<object, double> selectProp)
        {
            double value = (50 / 1000d);

            Func<object, bool> predicate = null;

            switch (filterType)
            {
                case FilterTypes.gte:
                    predicate = x => selectProp(x) >= value;
                    break;

                case FilterTypes.gt:
                    predicate = x => selectProp(x) > value;
                    break;

                case FilterTypes.lt:
                    predicate = x => selectProp(x) < value;
                    break;

                case FilterTypes.lte:
                    predicate = x => selectProp(x) <= value;
                    break;

                default:
                    break;
            }

            this.AssertFilter(
                new List<string> { col },
                new Dictionary<string, IEnumerable<FilterModel>>
                {
                    { col, new List<FilterModel>{ new FilterModel { Type = filterType, Value = value.ToString() } } }
                },
                predicate);
        }

        private void AssertShouldReturnCorrectExpressionOnSingleColumnGreaterThanForDateTime(string col, FilterTypes filterType, Func<object, DateTime> selectProp)
        {
            var value = new DateTime(2017, 1, 15);
            Func<object, bool> predicate = null;

            switch (filterType)
            {
                case FilterTypes.gte:
                    predicate = x => selectProp(x) >= value;
                    break;

                case FilterTypes.gt:
                    predicate = x => selectProp(x) > value;
                    break;

                case FilterTypes.lt:
                    predicate = x => selectProp(x) < value;
                    break;

                case FilterTypes.lte:
                    predicate = x => selectProp(x) <= value;
                    break;

                default:
                    break;
            }

            this.AssertFilter(
                new List<string> { col },
                new Dictionary<string, IEnumerable<FilterModel>>
                {
                    { col, new List<FilterModel>{ new FilterModel { Type = filterType, Value = value.ToShortDateString() } } }
                },
                predicate);
        }

        private void AssertFilter(IEnumerable<string> columns, Dictionary<string, IEnumerable<FilterModel>> filterModels, Func<object, bool> predicate)
        {
            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Custom.Filters = filterModels;

            foreach (var col in columns)
            {
                requestModel.TableParameters.Columns.Add(new Column { Data = col });
            }

            var data = this.GenerateComplexData(100);
            var actualExpr = this.filter.ProcessData(data, requestModel);
            var processedData = actualExpr.ToList();
            Assert.IsTrue(processedData.All(predicate));
            Assert.IsFalse(data.Except(processedData).Any(predicate));
        }

        private IQueryable<ComplexModel> GenerateComplexData(int numberOfItems)
        {
            var data = new List<ComplexModel>();

            for (int i = 1; i <= numberOfItems; i++)
            {
                var boolean = i % 2 == 0;
                var dateTime = new DateTime(2017, 1, ((i - 1) % 30) + 1);
                var @double = i / 1000f;
                var integer = i;
                var @string = i % 2 == 0 ? "eVen" : "odD";

                data.Add(new ComplexModel
                {
                    Boolean = boolean,
                    DateTime = dateTime,
                    Double = @double,
                    Integer = integer,
                    String = @string,
                    Char = (char)i,
                    SimpleModel = new SimpleModel
                    {
                        Boolean = boolean,
                        DateTime = dateTime,
                        Double = @double,
                        Integer = integer,
                        String = @string,
                        Char = (char)i
                    },
                    NestedComplexModel = new ComplexModel
                    {
                        Boolean = boolean,
                        DateTime = dateTime,
                        Double = @double,
                        Integer = integer,
                        String = @string,
                        SimpleModel = new SimpleModel
                        {
                            Boolean = boolean,
                            DateTime = dateTime,
                            Double = @double,
                            Integer = integer,
                            String = @string,
                        }
                    }
                });
            }

            return data.AsQueryable();
        }
    }
}