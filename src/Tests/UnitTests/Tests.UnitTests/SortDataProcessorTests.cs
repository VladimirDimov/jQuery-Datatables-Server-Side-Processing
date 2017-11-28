namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DataProcessing.SortDataProcessing;
    using JQDT.DI;
    using JQDT.Models;
    using NUnit.Framework;
    using TestData.Data;
    using TestData.Models;
    using Tests.Helpers;
    using Tests.UnitTests.Common;

    internal class SortDataProcessorTests
    {
        private IDataProcess<AllTypesModel> filter;
        private SortDataProcessor<ComplexModel> complexFilter;
        private IQueryable<AllTypesModel> data;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filter = resolver.GetSortDataProcessor<AllTypesModel>();
            this.complexFilter = new SortDataProcessor<ComplexModel>();
            this.data = DataGenerator.GenerateSimpleData(500, 500);
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        [TestCase(nameof(AllTypesModel.Integer), true)]
        [TestCase(nameof(AllTypesModel.Integer), false)]
        [TestCase(nameof(AllTypesModel.IntegerNullable), true)]
        [TestCase(nameof(AllTypesModel.IntegerNullable), false)]
        [TestCase(nameof(AllTypesModel.UInt), true)]
        [TestCase(nameof(AllTypesModel.UInt), false)]
        [TestCase(nameof(AllTypesModel.UIntNullable), true)]
        [TestCase(nameof(AllTypesModel.UIntNullable), false)]
        [TestCase(nameof(AllTypesModel.Long), true)]
        [TestCase(nameof(AllTypesModel.Long), false)]
        [TestCase(nameof(AllTypesModel.LongNullable), true)]
        [TestCase(nameof(AllTypesModel.LongNullable), false)]
        [TestCase(nameof(AllTypesModel.ULong), true)]
        [TestCase(nameof(AllTypesModel.ULong), false)]
        [TestCase(nameof(AllTypesModel.ULongNullable), true)]
        [TestCase(nameof(AllTypesModel.ULongNullable), false)]
        [TestCase(nameof(AllTypesModel.Short), true)]
        [TestCase(nameof(AllTypesModel.Short), false)]
        [TestCase(nameof(AllTypesModel.ShortNullable), true)]
        [TestCase(nameof(AllTypesModel.ShortNullable), false)]
        [TestCase(nameof(AllTypesModel.UShort), true)]
        [TestCase(nameof(AllTypesModel.UShort), false)]
        [TestCase(nameof(AllTypesModel.UShortNullable), true)]
        [TestCase(nameof(AllTypesModel.UShortNullable), false)]
        [TestCase(nameof(AllTypesModel.ByteProperty), true)]
        [TestCase(nameof(AllTypesModel.ByteProperty), false)]
        [TestCase(nameof(AllTypesModel.ByteNullable), true)]
        [TestCase(nameof(AllTypesModel.ByteNullable), false)]
        [TestCase(nameof(AllTypesModel.SByteProperty), true)]
        [TestCase(nameof(AllTypesModel.SByteProperty), false)]
        [TestCase(nameof(AllTypesModel.SByteNullable), true)]
        [TestCase(nameof(AllTypesModel.SByteNullable), false)]
        [TestCase(nameof(AllTypesModel.DoubleProperty), true)]
        [TestCase(nameof(AllTypesModel.DoubleProperty), false)]
        [TestCase(nameof(AllTypesModel.DoubleNullable), true)]
        [TestCase(nameof(AllTypesModel.DoubleNullable), false)]
        [TestCase(nameof(AllTypesModel.DecimalProperty), true)]
        [TestCase(nameof(AllTypesModel.DecimalProperty), false)]
        [TestCase(nameof(AllTypesModel.DecimalNullable), true)]
        [TestCase(nameof(AllTypesModel.DecimalNullable), false)]
        [TestCase(nameof(AllTypesModel.DateTimeProperty), true)]
        [TestCase(nameof(AllTypesModel.DateTimeProperty), false)]
        [TestCase(nameof(AllTypesModel.DateTimeNullable), true)]
        [TestCase(nameof(AllTypesModel.DateTimeNullable), false)]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetProperty), true)]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetProperty), false)]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetNullable), true)]
        [TestCase(nameof(AllTypesModel.DateTimeOffsetNullable), false)]
        [TestCase(nameof(AllTypesModel.BooleanProperty), true)]
        [TestCase(nameof(AllTypesModel.BooleanProperty), false)]
        [TestCase(nameof(AllTypesModel.BooleanNullable), true)]
        [TestCase(nameof(AllTypesModel.BooleanNullable), false)]
        [TestCase(nameof(AllTypesModel.CharProperty), true)]
        [TestCase(nameof(AllTypesModel.CharProperty), false)]
        [TestCase(nameof(AllTypesModel.CharNullable), true)]
        [TestCase(nameof(AllTypesModel.CharNullable), false)]
        [TestCase(nameof(AllTypesModel.StringProperty), true)]
        [TestCase(nameof(AllTypesModel.StringProperty), false)]
        // ---------------------------------------------------------------------
        [TestCase("NestedModel.Integer", false)]
        [TestCase("NestedModel.IntegerNullable", true)]
        [TestCase("NestedModel.IntegerNullable", false)]
        [TestCase("NestedModel.UInt", true)]
        [TestCase("NestedModel.UInt", false)]
        [TestCase("NestedModel.UIntNullable", true)]
        [TestCase("NestedModel.UIntNullable", false)]
        [TestCase("NestedModel.Integer", true)]
        [TestCase("NestedModel.Long", true)]
        [TestCase("NestedModel.Long", false)]
        [TestCase("NestedModel.LongNullable", true)]
        [TestCase("NestedModel.LongNullable", false)]
        [TestCase("NestedModel.ULong", true)]
        [TestCase("NestedModel.ULong", false)]
        [TestCase("NestedModel.ULongNullable", true)]
        [TestCase("NestedModel.ULongNullable", false)]
        [TestCase("NestedModel.Short", true)]
        [TestCase("NestedModel.Short", false)]
        [TestCase("NestedModel.ShortNullable", true)]
        [TestCase("NestedModel.ShortNullable", false)]
        [TestCase("NestedModel.UShort", true)]
        [TestCase("NestedModel.UShort", false)]
        [TestCase("NestedModel.UShortNullable", true)]
        [TestCase("NestedModel.UShortNullable", false)]
        [TestCase("NestedModel.ByteProperty", true)]
        [TestCase("NestedModel.ByteProperty", false)]
        [TestCase("NestedModel.ByteNullable", true)]
        [TestCase("NestedModel.ByteNullable", false)]
        [TestCase("NestedModel.SByteProperty", true)]
        [TestCase("NestedModel.SByteProperty", false)]
        [TestCase("NestedModel.SByteNullable", true)]
        [TestCase("NestedModel.SByteNullable", false)]
        [TestCase("NestedModel.DoubleProperty", true)]
        [TestCase("NestedModel.DoubleProperty", false)]
        [TestCase("NestedModel.DoubleNullable", true)]
        [TestCase("NestedModel.DoubleNullable", false)]
        [TestCase("NestedModel.DecimalProperty", true)]
        [TestCase("NestedModel.DecimalProperty", false)]
        [TestCase("NestedModel.DecimalNullable", true)]
        [TestCase("NestedModel.DecimalNullable", false)]
        [TestCase("NestedModel.DateTimeProperty", true)]
        [TestCase("NestedModel.DateTimeProperty", false)]
        [TestCase("NestedModel.DateTimeNullable", true)]
        [TestCase("NestedModel.DateTimeNullable", false)]
        [TestCase("NestedModel.DateTimeOffsetProperty", true)]
        [TestCase("NestedModel.DateTimeOffsetProperty", false)]
        [TestCase("NestedModel.DateTimeOffsetNullable", true)]
        [TestCase("NestedModel.DateTimeOffsetNullable", false)]
        [TestCase("NestedModel.BooleanProperty", true)]
        [TestCase("NestedModel.BooleanProperty", false)]
        [TestCase("NestedModel.BooleanNullable", true)]
        [TestCase("NestedModel.BooleanNullable", false)]
        [TestCase("NestedModel.CharProperty", true)]
        [TestCase("NestedModel.CharProperty", false)]
        [TestCase("NestedModel.CharNullable", true)]
        [TestCase("NestedModel.CharNullable", false)]
        [TestCase("NestedModel.StringProperty", true)]
        [TestCase("NestedModel.StringProperty", false)]
        public void SortBySinglePropertyShouldReturnCorrectData(string column, bool isAsc)
        {
            var requestModel = new RequestInfoModel
            {
                TableParameters = new DataTableAjaxPostModel
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Data = column
                        }
                    },
                    Order = new List<Order>
                    {
                        new Order
                        {
                            Column = 0,
                            Dir = isAsc ? "asc" : "desc"
                        }
                    }
                }
            };

            var processedData = this.filter.ProcessData(this.data, requestModel);
            var expectedData = isAsc ? this.data.OrderBy(column) : this.data.OrderByDescending(column);

            Assert.IsTrue(processedData.SequenceEqual(expectedData));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void SortByMultiplePropertiesShouldReturnCorrectExpression(int testCaseKey)
        {
            var testCases = new Dictionary<int, RequestInfoModel>
            {
                {
                    1,
                    new RequestInfoModel
                    {
                        TableParameters = new DataTableAjaxPostModel
                        {
                            Columns = new List<Column>
                            {
                                new Column{ Data = nameof(AllTypesModel.Integer) },
                                new Column{ Data = nameof(AllTypesModel.StringProperty) },
                                new Column{ Data = nameof(AllTypesModel.BooleanNullable) },
                            },
                            Order = new List<Order>
                            {
                                new Order { Column = 0, Dir = "asc" },
                                new Order { Column = 2, Dir = "desc" },
                                new Order { Column = 1, Dir = "asc" },
                            }
                        }
                    }
                },

                {
                    2,
                    new RequestInfoModel
                    {
                        TableParameters = new DataTableAjaxPostModel
                        {
                            Columns = new List<Column>
                            {
                                new Column{ Data = nameof(AllTypesModel.Integer) },
                                new Column{ Data = nameof(AllTypesModel.IntegerNullable) },
                                new Column{ Data = nameof(AllTypesModel.Integer) },
                            },
                            Order = new List<Order>
                            {
                                new Order { Column = 0, Dir = "desc" },
                                new Order { Column = 1, Dir = "desc" },
                                new Order { Column = 2, Dir = "asc" },
                            }
                        }
                    }
                },

                {
                    3,
                    new RequestInfoModel
                    {
                        TableParameters = new DataTableAjaxPostModel
                        {
                            Columns = new List<Column>
                            {
                                new Column{ Data = nameof(AllTypesModel.IntegerNullable) },
                                new Column{ Data = nameof(AllTypesModel.CharNullable) },
                                new Column{ Data = nameof(AllTypesModel.StringProperty) },
                            },
                            Order = new List<Order>
                            {
                                new Order { Column = 0, Dir = "asc" },
                                new Order { Column = 1, Dir = "asc" },
                                new Order { Column = 2, Dir = "asc" },
                            }
                        }
                    }
                }
            };

            var requestInfoModel = testCases[testCaseKey];
            var processedData = this.filter.ProcessData(this.data, requestInfoModel);
            IQueryable<AllTypesModel> expectedData = this.data;
            var columns = requestInfoModel.TableParameters.Columns;
            var orders = requestInfoModel.TableParameters.Order;
            bool isFirst = true;
            foreach (var order in orders)
            {
                var column = columns[order.Column];
                if (order.Dir == "asc")
                {
                    if (isFirst)
                    {
                        expectedData = expectedData.OrderBy(column.Data);
                    }
                    else
                    {
                        expectedData = ((IOrderedQueryable<AllTypesModel>)expectedData).ThenBy(column.Data);
                    }
                }
                else
                {
                    if (isFirst)
                    {
                        expectedData = expectedData.OrderByDescending(column.Data);
                    }
                    else
                    {
                        expectedData = ((IOrderedQueryable<AllTypesModel>)expectedData).ThenByDescending(column.Data);
                    }
                }

                isFirst = false;
            }

            Assert.IsTrue(processedData.SequenceEqual(expectedData));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortByIncorrectColumnNameShouldThroAppropriateException(bool isAsc)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var requestModel = TestHelpers.GetComplexRequestInfoModel();
                requestModel.TableParameters.Columns = new List<Column>
                {
                    new Column
                    {
                        Data = "NestedComplexModel.InvalidProperty"
                    }
                };

                requestModel.TableParameters.Order = new List<Order>
                {
                    new Order
                    {
                        Column = 0,
                        Dir = isAsc ? "asc" : "desc"
                    }
                };

                var actualExpr = this.complexFilter.ProcessData(this.complexData, requestModel);
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("invalid property name"));
        }

        [Test]
        public void ShouldThrowAppropriateExceptionWhenSortByComplexProperty()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var requestModel = TestHelpers.GetComplexRequestInfoModel();
                requestModel.TableParameters.Columns = new List<Column>
                {
                    new Column
                    {
                        Data = "NestedComplexModel.SimpleModel"
                    }
                };

                requestModel.TableParameters.Order = new List<Order>
                {
                    new Order
                    {
                        Column = 0,
                        Dir = "asc"
                    }
                };

                var actualExpr = this.complexFilter.ProcessData(this.complexData, requestModel);
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("invalid property type"));
        }

        [Test]
        public void ShouldThrowAppropriateExceptionWhenColumnNameIsMissing()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var requestModel = TestHelpers.GetComplexRequestInfoModel();
                requestModel.TableParameters.Columns = new List<Column>
                {
                    new Column
                    {
                        Data = string.Empty
                    }
                };

                requestModel.TableParameters.Order = new List<Order>
                {
                    new Order
                    {
                        Column = 0,
                        Dir = "asc"
                    }
                };

                var actualExpr = this.complexFilter.ProcessData(this.complexData, requestModel);
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("missing column name"));
        }
    }
}