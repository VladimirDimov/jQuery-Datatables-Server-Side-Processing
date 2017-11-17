namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class ColumnsFilterDataProcessorTests
    {
        private ColumnsFilterDataProcessor filter;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.filter = new ColumnsFilterDataProcessor();
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        public void ShouldReturnUntouchedDataIfNoColumnFilters()
        {
            var requestModel = TestHelpers.GetSimpleRequestInfoModel();

            var actualExpr = this.filter.ProcessData(this.simpleData, requestModel);
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

            var actualExpr = this.filter.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(ComplexModel).FullName}].Where(m => Convert(m).{column}.ToString().ToLower().Contains(\"{search}\"))";

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

            var actualExpr = this.filter.ProcessData(this.simpleData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(SimpleModel).FullName}].Where(m => ((Convert(m).{StringColumn}.ToString().ToLower().Contains(\"{StringColumnSearch}\") And Convert(m).{DateTimeColumn}.ToString().ToLower().Contains(\"{DateTimeColumnSearch}\")) And Convert(m).{IntegerColumn}.ToString().ToLower().Contains(\"{IntegerColumnSearch}\")))";

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

            var actualExpr = this.filter.ProcessData(this.complexData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(ComplexModel).FullName}].Where(m => ((Convert(m).{StringColumn}.ToString().ToLower().Contains(\"{StringColumnSearch}\") And Convert(m).{DateTimeColumn}.ToString().ToLower().Contains(\"{DateTimeColumnSearch}\")) And Convert(m).{IntegerColumn}.ToString().ToLower().Contains(\"{IntegerColumnSearch}\")))";

            Assert.AreEqual(expectedExprStr, actualExprStr);
            Assert.DoesNotThrow(() =>
            {
                actualExpr.ToList();
            });
        }
    }
}