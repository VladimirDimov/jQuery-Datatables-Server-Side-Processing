namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing.ColumnsFilterDataProcessing;
    using JQDT.DataProcessing.Common;
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
            this.filterSimpleModelProcessor = new ColumnsFilterDataProcessor<SimpleModel>(new SearchCommonProcessor());
            this.filterComplexModelProcessor = new ColumnsFilterDataProcessor<ComplexModel>(new SearchCommonProcessor());
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
        public void ColumnFilter_ShouldWorkAppropriateWithAllSupportedTypes()
        {
            var startDate = new DateTime(2017, 1, 1);
            var data = new List<SimpleModel>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new SimpleModel
                {
                    Boolean = i % 2 == 0,
                    Char = i % 3 == 0 ? 'a' : 'b',
                    CharNullable = i % 4 == 0 ? (char?)null : 'c',
                    DateTime = startDate.AddDays(i),
                    Double = 0 + i / 1000d,
                    Integer = i,
                    String = $"string_{i}"
                });
            }

            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            requestModel.TableParameters.Columns = new List<JQDT.Models.Column>();

            var processedData = this.filterSimpleModelProcessor.ProcessData(data, requestModel).ToList();

            Assert.AreEqual(data.Count(), processedData.Count);
        }
    }
}