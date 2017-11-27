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
    using TestData.Data;
    using TestData.Models;
    using Tests.UnitTests.Common;

    internal class ColumnsFilterDataProcessorTests
    {
        // TODO: Add case "Filter by more than one columns"

        private IDataProcess<AllTypesModel> filterSimpleModelProcessor;
        private IDataProcess<ComplexModel> filterComplexModelProcessor;
        private IQueryable<AllTypesModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filterSimpleModelProcessor = resolver.GetColumnsFilterDataProcessor<AllTypesModel>();
            this.filterComplexModelProcessor = resolver.GetColumnsFilterDataProcessor<ComplexModel>();
            this.simpleData = new List<AllTypesModel>().AsQueryable();
            this.complexData = new List<ComplexModel>().AsQueryable();
        }

        [Test]
        public void ColumnFilter_ShouldReturnUntouchedDataIfNoColumnFilters()
        {
            var data = new List<AllTypesModel>
            {
                new AllTypesModel{CharNullable = null, Integer = 1},
                new AllTypesModel{CharNullable = null, Integer = 2},
                new AllTypesModel{CharNullable = null, Integer = 3},
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
        [TestCase("ByteProperty", null)]
        [TestCase("ByteNullable", null)]
        [TestCase("SByteProperty", null)]
        [TestCase("SByteNullable", null)]
        [TestCase("DoubleProperty", null)]
        [TestCase("DoubleNullable", null)]
        [TestCase("DecimalProperty", null)]
        [TestCase("DecimalProperty", "79228162514264337593543950335")]
        [TestCase("DecimalProperty", "-79228162514264337593543950335")]
        [TestCase("DecimalNullable", null)]
        [TestCase("DateTimeProperty", null)]
        [TestCase("DateTimeNullable", null)]
        [TestCase("DateTimeOffsetProperty", null)]
        [TestCase("DateTimeOffsetNullable", null)]
        [TestCase("BooleanProperty", "true")]
        [TestCase("BooleanNullable", "false")]
        [TestCase("CharProperty", null)]
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