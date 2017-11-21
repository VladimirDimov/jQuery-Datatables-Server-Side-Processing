namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DataProcessing.Common;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Models;

    public class FilterDataProcessorTests
    {
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        public void SearchWithTwoSearchableProperties()
        {
            var filterProc = new FilterDataProcessor<SimpleModel>(new FiltersCommonProcessor(new FilterDataProcessorEnumerableQueryBridge()));
            var data = new List<SimpleModel>().AsQueryable();
            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = "aaa"
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = true
                        },
                        new Column
                        {
                            Data = "Integer",
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.SimpleModel].Where(model => (((model.String != null) AndAlso model.String.ToString().ToLower().Contains(\"aaa\")) OrElse model.Integer.ToString().ToLower().Contains(\"aaa\")))";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        [Test]
        public void SearchWithSingleSearchableProperty()
        {
            var filterProc = this.GetFilterDataProcessor<SimpleModel>();
            var data = new List<SimpleModel>().AsQueryable();
            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = "aaa"
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.SimpleModel].Where(model => ((model.String != null) AndAlso model.String.ToString().ToLower().Contains(\"aaa\")))";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        [Test]
        public void ShouldThrowAppropriateExceptionIfSearchValueWithNoSearchableProperties()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var filterProc = this.GetFilterDataProcessor<SimpleModel>();
                var data = new List<SimpleModel>().AsQueryable();
                var processedData = filterProc.ProcessData(data, new RequestInfoModel()
                {
                    Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                    TableParameters = new DataTableAjaxPostModel
                    {
                        Search = new Search
                        {
                            Value = "aaa"
                        },
                        Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Searchable = false
                        }
                    }
                    }
                });
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("no searchable properties"));
        }

        [Test]
        public void SearchBySingleNestedPropertyShouldWork()
        {
            var filterProc = this.GetFilterDataProcessor<ComplexModel>();
            var data = new List<ComplexModel>().AsQueryable();
            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(ComplexModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = "aaa"
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "NestedComplexModel.NestedComplexModel.SimpleModel.String",
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(model => (((((model.NestedComplexModel != null) AndAlso (model.NestedComplexModel.NestedComplexModel != null)) AndAlso (model.NestedComplexModel.NestedComplexModel.SimpleModel != null)) AndAlso (model.NestedComplexModel.NestedComplexModel.SimpleModel.String != null)) AndAlso model.NestedComplexModel.NestedComplexModel.SimpleModel.String.ToString().ToLower().Contains(\"aaa\")))";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        [Test]
        public void ShouldReturnUntouchedDataIfNoSeachValue()
        {
            var filterProc = this.GetFilterDataProcessor<ComplexModel>();
            var data = new List<ComplexModel>().AsQueryable();
            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(ComplexModel) },
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
                            Searchable = true
                        }
                    }
                }
            });

            var expression = ((System.Linq.IQueryable)processedData).Expression;
            var actualExpressionStr = expression.ToString();
            var expectedExpressionStr = $"System.Collections.Generic.List`1[{typeof(ComplexModel).FullName}]";

            Assert.AreEqual(expectedExpressionStr, actualExpressionStr);
        }

        private FilterDataProcessor<T> GetFilterDataProcessor<T>()
        {
            var filterProc = new FilterDataProcessor<T>(new FiltersCommonProcessor(new FilterDataProcessorEnumerableQueryBridge()));

            return filterProc;
        }
    }
}