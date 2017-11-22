namespace Tests.UnitTests
{
    using System;
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
        private ColumnsFilterDataProcessor<SimpleModel> filterSimpleModelProcessor;
        private ColumnsFilterDataProcessor<ComplexModel> filterComplexModelProcessor;
        private IQueryable<SimpleModel> simpleData;
        private IQueryable<ComplexModel> complexData;

        [SetUp]
        public void SetUp()
        {
            this.filterSimpleModelProcessor = new ColumnsFilterDataProcessor<SimpleModel>(new CommonSearchProcessor());
            this.filterComplexModelProcessor = new ColumnsFilterDataProcessor<ComplexModel>(new CommonSearchProcessor());
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
        public void ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypes(string column, string searchValue)
        {
            var data = DataGenerator.GenerateSimpleData(10000);
            var random = new Random();

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<Column>
            {
                new Column{ Data = column, Search = new Search{ Value = searchValue } }
            };

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.IsTrue(processedData.All(x => x.Integer.ToString() == searchValue));
        }
    }
}