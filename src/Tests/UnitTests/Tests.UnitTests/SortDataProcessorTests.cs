using System.Collections.Generic;
using System.Linq;
using JQDT.DataProcessing;
using JQDT.Models;
using NUnit.Framework;
using Tests.UnitTests.Models;

namespace Tests.UnitTests
{
    internal class SortDataProcessorTests
    {
        private SortDataProcessor filter;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.filter = new SortDataProcessor();
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortBySingleNonNestedPropertyShouldReturnCorrectExpression(bool isAsc)
        {
            var requestModel = this.GetComplexRequestInfoModel();
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

            var actualExpr = this.filter.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var postfix = isAsc ? string.Empty : "Descending";
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(ComplexModel).FullName}].OrderBy{postfix}(x => Convert(x).String)";

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
            var requestModel = this.GetComplexRequestInfoModel();
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

            var actualExpr = this.filter.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var postfix = isAsc ? string.Empty : "Descending";
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(ComplexModel).FullName}].OrderBy{postfix}(x => Convert(x).NestedComplexModel.NestedComplexModel.SimpleModel.String)";

            Assert.AreEqual(expectedExprStr, actualExprStr);

            Assert.DoesNotThrow(() =>
            {
                var tmp = actualExpr.ToList();
            });
        }

        [Test]
        public void SortByMultiplePropertiesShouldReturnCorrectExpression()
        {
            Assert.Fail("Not completed");
        }

        [Test]
        public void SortByIncorrectColumnNameShouldThroAppropriateException()
        {
            Assert.Fail("Not completed");
        }

        private RequestInfoModel GetSimpleRequestInfoModel()
        {
            return new RequestInfoModel()
            {
                Helpers = new RequestHelpers
                {
                    ModelType = typeof(SimpleModel)
                },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = ""
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Orderable = true
                        },
                        new Column
                        {
                            Data = "Integer",
                            Orderable = true
                        }
                    }
                }
            };
        }

        private RequestInfoModel GetComplexRequestInfoModel()
        {
            return new RequestInfoModel()
            {
                Helpers = new RequestHelpers
                {
                    ModelType = typeof(ComplexModel)
                },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = ""
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "NestedComplexModel.NestedComplexModel.SimpleModel.String",
                            Orderable = true
                        }
                    }
                }
            };
        }
    }
}