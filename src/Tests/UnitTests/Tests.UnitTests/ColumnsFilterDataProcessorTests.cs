namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class ColumnsFilterDataProcessorTests
    {
        // TODO: Add case "Filter by more than one columns"

        private IDataProcess<SimpleModel> filterSimpleModelProcessor;
        private IDataProcess<ComplexModel> filterComplexModelProcessor;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filterSimpleModelProcessor = resolver.GetColumnsFilterDataProcessor<SimpleModel>();
            this.filterComplexModelProcessor = resolver.GetColumnsFilterDataProcessor<ComplexModel>();
            this.simpleData = new List<SimpleModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        public void ColumnFilter_ShouldReturnUntouchedDataIfNoColumnFilters()
        {
            var data = new List<SimpleModel>
            {
                new SimpleModel{CharNullable = null, Integer = 1},
                new SimpleModel{CharNullable = null, Integer = 2},
                new SimpleModel{CharNullable = null, Integer = 3},
            }
            .AsQueryable();

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<JQDT.Models.Column>();

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.AreEqual(data.Count(), processedData.Count);
        }

        [Test]
        [TestCase("Integer", null)]
        [TestCase("Integer", "2147483647")]
        [TestCase("Integer", "-2147483648")]
        [TestCase("IntegerNullable", null)]
        [TestCase("UInt", null)]
        [TestCase("UIntNullable", null)]
        [TestCase("Long", null)]
        [TestCase("Long", "9223372036854775807")]
        [TestCase("Long", "-9223372036854775808")]
        [TestCase("LongNullable", null)]
        [TestCase("ULong", null)]
        [TestCase("ULong", "18446744073709551615")]
        [TestCase("ULongNullable", null)]
        [TestCase("Short", null)]
        [TestCase("ShortNullable", null)]
        [TestCase("UShort", null)]
        [TestCase("UShortNullable", null)]
        [TestCase("Byte", null)]
        [TestCase("ByteNullable", null)]
        [TestCase("SByte", null)]
        [TestCase("SByteNullable", null)]
        [TestCase("Double", null)]
        [TestCase("DoubleNullable", null)]
        [TestCase("Decimal", null)]
        [TestCase("Decimal", "79228162514264337593543950335")]
        [TestCase("Decimal", "-79228162514264337593543950335")]
        [TestCase("DecimalNullable", null)]
        [TestCase("DateTime", null)]
        [TestCase("DateTimeNullable", null)]
        [TestCase("DateTimeOffset", null)]
        [TestCase("DateTimeOffsetNullable", null)]
        [TestCase("Boolean", "true")]
        [TestCase("BooleanNullable", "false")]
        [TestCase("Char", null)]
        [TestCase("CharNullable", null)]
        public void ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypesNoNestedProperties(string column, string searchValue)
        {
            var data = DataGenerator.GenerateSimpleData(1000);

            if (string.IsNullOrEmpty(searchValue))
            {
                searchValue = TestHelpers.GetRandomPropertyValue(data, column).ToString();
            }

            var random = new Random();

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column{ Data = column, Search = new Search{ Value = searchValue } }
            };

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.IsTrue(processedData.All(x => x.GetType().GetProperty(column).GetValue(x).ToString().ToLower() == searchValue.ToLower()));
            Trace.WriteLine(nameof(ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypesNoNestedProperties) + $" number of items: {processedData.Count}  case: {column} / {searchValue}");
        }

        [Test]
        public void ColumnFilter_ShouldWorkAppropriateWithNestedNullable()
        {
            var data = DataGenerator.GenerateSimpleData(1000);
            var column = "NestedModel.IntegerNullable";

            var searchValue = data.First(x => x.NestedModel.IntegerNullable.HasValue).NestedModel.IntegerNullable.Value.ToString();

            var random = new Random();

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column{ Data = column, Search = new Search{ Value = searchValue } }
            };

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.IsTrue(processedData.All(x => x.NestedModel.IntegerNullable.HasValue && x.NestedModel.IntegerNullable.ToString().ToLower() == searchValue.ToLower()));
            Trace.WriteLine(nameof(ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypesNoNestedProperties) + $" number of items: {processedData.Count}  case: {column} / {searchValue}");
        }

        [Test]
        public void ColumnFilter_ShouldWorkAppropriateWithNestedNullableDateTime()
        {
            var data = DataGenerator.GenerateSimpleData(1000);
            var column = "NestedModel.DateTimeNullable";

            var searchValue = data.First(x => x.NestedModel.DateTimeNullable.HasValue).NestedModel.DateTimeNullable.Value.ToString();

            var random = new Random();

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column{ Data = column, Search = new Search{ Value = searchValue } }
            };

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.IsTrue(processedData.All(x => x.NestedModel.DateTimeNullable.HasValue && x.NestedModel.DateTimeNullable.ToString().ToLower() == searchValue.ToLower()));
            Trace.WriteLine(nameof(ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypesNoNestedProperties) + $" number of items: {processedData.Count}  case: {column} / {searchValue}");
        }
    }
}