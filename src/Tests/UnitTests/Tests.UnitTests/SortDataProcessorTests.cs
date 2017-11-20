namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class SortDataProcessorTests
    {
        private SortDataProcessor<SimpleModel> simpleFilter;
        private SortDataProcessor<ComplexModel> complexFilter;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.simpleFilter = new SortDataProcessor<SimpleModel>();
            this.complexFilter = new SortDataProcessor<ComplexModel>();
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortBySingleNonNestedPropertyShouldReturnCorrectExpression(bool isAsc)
        {
            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column
                {
                    Data = "String"
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
            var actualExprStr = actualExpr.Expression.ToString();
            var postfix = isAsc ? string.Empty : "Descending";
            var expectedExprStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].OrderBy(x => x.String)";

            Assert.AreEqual(expectedExprStr, actualExprStr);

            Assert.DoesNotThrow(() =>
            {
                var tmp = actualExpr.ToList();
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortBySingleNestedPropertyShouldReturnCorrectExpression(bool isAsc)
        {
            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column
                {
                    Data = "NestedComplexModel.NestedComplexModel.SimpleModel.String"
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
            var actualExprStr = actualExpr.Expression.ToString();
            var postfix = isAsc ? string.Empty : "Descending";
            var expectedExprStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].OrderBy(x => x.NestedComplexModel.NestedComplexModel.SimpleModel.String)";

            Assert.AreEqual(expectedExprStr, actualExprStr);

            Assert.DoesNotThrow(() =>
            {
                var tmp = actualExpr.ToList();
            });
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortByMultiplePropertiesShouldReturnCorrectExpression(bool isAsc)
        {
            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column
                {
                    Data = "NestedComplexModel.String",
                    Orderable = true
                },
                new Column
                {
                    Data = "NestedComplexModel.NestedComplexModel.String",
                    Orderable = false
                },
                new Column
                {
                    Data = "NestedComplexModel.NestedComplexModel.SimpleModel.DateTime",
                    Orderable = true
                },
            };

            requestModel.TableParameters.Order = new List<Order>
            {
                new Order
                {
                    Column = 0,
                    Dir = isAsc ? "asc" : "desc"
                },
                new Order
                {
                    Column = 1,
                    Dir = isAsc ? "asc" : "desc"
                },
                new Order
                {
                    Column = 2,
                    Dir = isAsc ? "asc" : "desc"
                }
            };

            var actualExpr = this.complexFilter.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].OrderBy(x => x.NestedComplexModel.String).ThenBy(x => x.NestedComplexModel.NestedComplexModel.String).ThenBy(x => x.NestedComplexModel.NestedComplexModel.SimpleModel.DateTime)";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                var tmp = actualExpr.ToList();
            });
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