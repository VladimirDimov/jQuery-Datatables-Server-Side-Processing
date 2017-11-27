namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing.SortDataProcessing;
    using JQDT.Models;
    using NUnit.Framework;
    using TestData.Models;
    using Tests.Helpers;
    using Tests.UnitTests.Common;

    internal class SortDataProcessorTests
    {
        private SortDataProcessor<AllTypesModel> filter;
        private SortDataProcessor<ComplexModel> complexFilter;
        private IQueryable<AllTypesModel> data;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.filter = new SortDataProcessor<AllTypesModel>();
            this.complexFilter = new SortDataProcessor<ComplexModel>();
            this.data = new List<AllTypesModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        [TestCase(nameof(AllTypesModel.Integer), true)]
        [TestCase(nameof(AllTypesModel.Integer), false)]
        public void SortBySingleNonNestedPropertyShouldReturnCorrectExpression(string column, bool isAsc)
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
            var expectedData = this.data.OrderBy($"{column}");

            Assert.IsTrue(processedData.SequenceEqual(expectedData));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortBySingleNestedPropertyShouldReturnCorrectExpression(bool isAsc)
        {
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortByMultiplePropertiesShouldReturnCorrectExpression(bool isAsc)
        {
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