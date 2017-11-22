namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing.ColumnsFilterDataProcessing;
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
            this.filterSimpleModel = new ColumnsFilterDataProcessor<SimpleModel>(new SearchCommonProcessor());
            this.filterComplexModel = new ColumnsFilterDataProcessor<ComplexModel>(new SearchCommonProcessor());
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
        [TestCase("String", "Asd", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => ((m.String != null) AndAlso m.String.ToString().ToLower().Contains(\"asd\")))")]
        [TestCase("Integer", "123", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => m.Integer.ToString().ToLower().Contains(\"123\"))")]
        [TestCase("Boolean", "true", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => m.Boolean.ToString().ToLower().Contains(\"true\"))")]
        [TestCase("Double", "0.123", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => m.Double.ToString().ToLower().Contains(\"0.123\"))")]
        [TestCase("SimpleModel.String", "Asd", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => (((m.SimpleModel != null) AndAlso (m.SimpleModel.String != null)) AndAlso m.SimpleModel.String.ToString().ToLower().Contains(\"asd\")))")]
        [TestCase("SimpleModel.Integer", "123", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => ((m.SimpleModel != null) AndAlso m.SimpleModel.Integer.ToString().ToLower().Contains(\"123\")))")]
        [TestCase("NestedComplexModel.Boolean", "true", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => ((m.NestedComplexModel != null) AndAlso m.NestedComplexModel.Boolean.ToString().ToLower().Contains(\"true\")))")]
        [TestCase("NestedComplexModel.SimpleModel.Double", "0.123", "System.Collections.Generic.List`1[Tests.UnitTests.Models.ComplexModel].Where(m => (((m.NestedComplexModel != null) AndAlso (m.NestedComplexModel.SimpleModel != null)) AndAlso m.NestedComplexModel.SimpleModel.Double.ToString().ToLower().Contains(\"0.123\")))")]
        public void ShouldReturnCorrectExpressionOnSingleColumnFilter(string column, string search, string expectedExprStr)
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