namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    public class SearchDataProcessorTests
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
            var filterProc = new DependencyResolver().GetSearchDataProcessor<SimpleModel>();
            var data = new List<SimpleModel>()
            {
                new SimpleModel{ StringProperty = "aaa", CharProperty = 'q' },
                new SimpleModel{ StringProperty = null, CharProperty = 'q', CharNullable = 'b' },
                new SimpleModel{ StringProperty = "bbb", CharNullable = null },
            }
            .AsQueryable();
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
                            Data = nameof(SimpleModel.StringProperty),
                            Searchable = true
                        },
                        new Column
                        {
                            Data = nameof(SimpleModel.CharProperty),
                            Searchable = true
                        },
                        new Column
                        {
                            Data = nameof(SimpleModel.CharNullable),
                            Searchable = true
                        }
                    }
                }
            })
            .ToList();

            Assert.AreEqual(1, processedData.Count);
        }

        [Test]
        public void SearchWithSingleSearchableProperty()
        {
            var filterProc = this.GetFilterDataProcessor<SimpleModel>();
            var data = new List<SimpleModel>()
            {
                new SimpleModel{StringProperty = "aaa"},
                new SimpleModel{StringProperty = null},
                new SimpleModel{StringProperty = "bbb"},
            }
            .AsQueryable();

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
                            Data = "StringProperty",
                            Searchable = true
                        }
                    }
                }
            })
            .ToList();

            Assert.AreEqual(1, processedData.Count);
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
                            Data = "StringProperty",
                            Searchable = false
                        }
                    }
                    }
                });
            });

            Assert.IsTrue(exception.Message.ToLower().Contains("no searchable properties"));
        }

        [Test]
        public void SearchBySingleNestedStringPropertyShouldWork()
        {
            var filterProc = this.GetFilterDataProcessor<SimpleModel>();
            var data = DataGenerator.GenerateSimpleData(5000);
            var searchValue = "z";

            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = searchValue
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "NestedModel.StringProperty",
                            Searchable = true
                        }
                    }
                }
            });

            Assert.IsTrue(processedData.All(x => x.NestedModel.StringProperty != null && x.NestedModel.StringProperty.ToLower().Contains(searchValue.ToLower())));
        }

        [Test]
        public void SearchBySingleNestedCharPropertyShouldWork()
        {
            var filterProc = this.GetFilterDataProcessor<SimpleModel>();
            var data = DataGenerator.GenerateSimpleData(5000);
            char searchValue = 'Z';

            var processedData = filterProc.ProcessData(data, new RequestInfoModel()
            {
                Helpers = new RequestHelpers { ModelType = typeof(SimpleModel) },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = searchValue.ToString()
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "NestedModel.CharProperty",
                            Searchable = true
                        }
                    }
                }
            });

            Assert.IsTrue(processedData.All(x => x.NestedModel.CharProperty.ToString().ToLower()[0] == searchValue));
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

        private IDataProcess<T> GetFilterDataProcessor<T>()
        {
            var filterProc = new DependencyResolver().GetSearchDataProcessor<T>();

            return filterProc;
        }
    }
}