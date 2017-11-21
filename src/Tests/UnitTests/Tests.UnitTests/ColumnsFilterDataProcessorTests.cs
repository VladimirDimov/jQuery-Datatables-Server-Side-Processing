namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing.ColumnsFilter;
    using JQDT.DataProcessing.Common;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class ColumnsFilterDataProcessorTests
    {
        private ColumnsFilterDataProcessor<SimpleModel> filterSimpleModel;
        private ColumnsFilterDataProcessor<ComplexModel> filterComplexModel;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.filterSimpleModel = new ColumnsFilterDataProcessor<SimpleModel>(new FiltersCommonProcessor(new FilterDataProcessorEnumerableQueryBridge()));
            this.filterComplexModel = new ColumnsFilterDataProcessor<ComplexModel>(new FiltersCommonProcessor(new FilterDataProcessorEnumerableQueryBridge()));
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        public void ShouldReturnUntouchedDataIfNoColumnFilters()
        {
            var requestModel = TestHelpers.GetSimpleRequestInfoModel();

            var actualExpr = this.filterSimpleModel.ProcessData(this.simpleData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(SimpleModel).FullName}]";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                actualExpr.ToList();
            });
        }

        [Test]
        [TestCase("String", "Asd")]
        [TestCase("Integer", "123")]
        [TestCase("Boolean", "true")]
        [TestCase("DateTime", "12/24/2017")]
        [TestCase("Double", "0.123")]
        [TestCase("SimpleModel.String", "Asd")]
        [TestCase("SimpleModel.Integer", "123")]
        [TestCase("NestedComplexModel.Boolean", "true")]
        [TestCase("NestedComplexModel.SimpleModel.DateTime", "12/24/2017")]
        [TestCase("NestedComplexModel.SimpleModel.Double", "0.123")]
        public void ShouldReturnCorrectExpressionOnSingleColumnFilter(string column, string search)
        {
            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Columns.Add(new Column
            {
                Data = column,
                Search = new Search
                {
                    Value = search
                }
            });

            var actualExpr = this.filterComplexModel.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => ((m.NestedComplexModel != null) AndAlso m.NestedComplexModel.Boolean.ToString().ToLower().Contains(\"true\")))";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                actualExpr.ToList();
            });
        }

        [Test]
        public void ShouldReturnCorrectExpressionOnMultipleSimpleColumnFilters()
        {
            const string StringColumn = "String";
            const string StringColumnSearch = "asd";
            const string DateTimeColumn = "DateTime";
            const string DateTimeColumnSearch = "sad324";
            const string IntegerColumn = "Integer";
            const string IntegerColumnSearch = "213hjv321uvg";

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column
                {
                    Data = StringColumn,
                    Search = new Search
                    {
                        Value = StringColumnSearch
                    }
                },
                new Column
                {
                    Data = DateTimeColumn,
                    Search = new Search
                    {
                        Value = DateTimeColumnSearch
                    }
                },
                new Column
                {
                    Data = IntegerColumn,
                    Search = new Search
                    {
                        Value = IntegerColumnSearch
                    }
                }
            };

            var actualExpr = this.filterSimpleModel.ProcessData(this.simpleData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = "System.Collections.Generic.List`1[Tests.UnitTests.Models.SimpleModel].Where(m => ((((m.String != null) AndAlso m.String.ToString().ToLower().Contains(\"asd\")) AndAlso m.DateTime.ToString().ToLower().Contains(\"sad324\")) AndAlso m.Integer.ToString().ToLower().Contains(\"213hjv321uvg\")))";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                actualExpr.ToList();
            });
        }

        [Test]
        public void ShouldReturnCorrectExpressionOnMultipleComplexColumnFilters()
        {
            const string StringColumn = "SimpleModel.String";
            const string StringColumnSearch = "asd";
            const string DateTimeColumn = "NestedComplexModel.SimpleModel.DateTime";
            const string DateTimeColumnSearch = "sad324";
            const string IntegerColumn = "NestedComplexModel.SimpleModel.Integer";
            const string IntegerColumnSearch = "213hjv321uvg";

            var requestModel = TestHelpers.GetComplexRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column
                {
                    Data = StringColumn,
                    Search = new Search
                    {
                        Value = StringColumnSearch
                    }
                },
                new Column
                {
                    Data = DateTimeColumn,
                    Search = new Search
                    {
                        Value = DateTimeColumnSearch
                    }
                },
                new Column
                {
                    Data = IntegerColumn,
                    Search = new Search
                    {
                        Value = IntegerColumnSearch
                    }
                }
            };

            var actualExpr = this.filterComplexModel.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => (((((m.SimpleModel != null) AndAlso (m.SimpleModel.String != null)) AndAlso m.SimpleModel.String.ToString().ToLower().Contains(\"asd\")) AndAlso (((m.NestedComplexModel != null) AndAlso (m.NestedComplexModel.SimpleModel != null)) AndAlso m.NestedComplexModel.SimpleModel.DateTime.ToString().ToLower().Contains(\"sad324\"))) AndAlso (((m.NestedComplexModel != null) AndAlso (m.NestedComplexModel.SimpleModel != null)) AndAlso m.NestedComplexModel.SimpleModel.Integer.ToString().ToLower().Contains(\"213hjv321uvg\"))))";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                actualExpr.ToList();
            });
        }
    }
}