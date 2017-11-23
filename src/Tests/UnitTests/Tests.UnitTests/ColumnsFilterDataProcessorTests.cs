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
        [TestCase("Integer", "50")]
        [TestCase("Integer", "2147483647")]
        [TestCase("Integer", "-2147483648")]
        [TestCase("IntegerNullable", "50")]
        [TestCase("UInt", "30")]
        [TestCase("UIntNullable", "30")]
        [TestCase("Long", "100")]
        [TestCase("Long", "9223372036854775807")]
        [TestCase("Long", "-9223372036854775808")]
        [TestCase("LongNullable", "-100")]
        [TestCase("ULong", "100")]
        [TestCase("ULong", "18446744073709551615")]
        [TestCase("ULongNullable", "100")]
        [TestCase("Short", "-30")]
        [TestCase("ShortNullable", "30")]
        [TestCase("UShort", "30")]
        [TestCase("UShortNullable", "30")]
        [TestCase("Byte", "20")]
        [TestCase("ByteNullable", "20")]
        [TestCase("SByte", "-20")]
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
            var data = DataGenerator.GenerateSimpleData(10000);

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
            Trace.WriteLine(nameof(ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypesNoNestedProperties) + $" number of items: {processedData.Count}  case: {column}");
        }
    }
}