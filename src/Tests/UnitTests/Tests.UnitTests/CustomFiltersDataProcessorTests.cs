namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Dynamic;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.Models;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class CustomFiltersDataProcessorTests
    {
        private IDataProcess<SimpleModel> filter;
        private IQueryable<SimpleModel> data;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filter = resolver.GetCustomFiltersDataProcessor<SimpleModel>();
            this.data = DataGenerator.GenerateSimpleData(5000, 50);
        }

        [Test]
        // ---------------------------------------------------------------
        [TestCase("Integer", FilterTypes.gt, "25")]
        [TestCase("Integer", FilterTypes.gte, "25")]
        [TestCase("Integer", FilterTypes.lt, "25")]
        [TestCase("Integer", FilterTypes.lte, "25")]
        [TestCase("Integer", FilterTypes.eq, "25")]
        [TestCase("Integer", FilterTypes.eq, "-25")]
        [TestCase("Integer", FilterTypes.eq, int.MaxValue)]
        [TestCase("Integer", FilterTypes.eq, int.MinValue)]
        [TestCase("NestedModel.Integer", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.Integer", FilterTypes.eq, int.MaxValue)]
        [TestCase("NestedModel.Integer", FilterTypes.eq, int.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("IntegerNullable", FilterTypes.gt, "25")]
        [TestCase("IntegerNullable", FilterTypes.gte, "25")]
        [TestCase("IntegerNullable", FilterTypes.lt, "25")]
        [TestCase("IntegerNullable", FilterTypes.lte, "25")]
        [TestCase("IntegerNullable", FilterTypes.eq, "25")]
        [TestCase("IntegerNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, int.MaxValue)]
        [TestCase("NestedModel.IntegerNullable", FilterTypes.eq, int.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UInt", FilterTypes.gt, "25")]
        [TestCase("UInt", FilterTypes.gte, "25")]
        [TestCase("UInt", FilterTypes.lt, "25")]
        [TestCase("UInt", FilterTypes.lte, "25")]
        [TestCase("UInt", FilterTypes.eq, "25")]
        [TestCase("UInt", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UInt", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UInt", FilterTypes.eq, uint.MaxValue)]
        [TestCase("NestedModel.UInt", FilterTypes.eq, uint.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("UIntNullable", FilterTypes.gt, "25")]
        [TestCase("UIntNullable", FilterTypes.gte, "25")]
        [TestCase("UIntNullable", FilterTypes.lt, "25")]
        [TestCase("UIntNullable", FilterTypes.lte, "25")]
        [TestCase("UIntNullable", FilterTypes.eq, "25")]
        [TestCase("UIntNullable", FilterTypes.eq, "99999999")]
        [TestCase("UIntNullable", FilterTypes.eq, uint.MaxValue)]
        [TestCase("UIntNullable", FilterTypes.eq, uint.MinValue)]
        [TestCase("NestedModel.UIntNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, "99999999")]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, uint.MaxValue)]
        [TestCase("NestedModel.UIntNullable", FilterTypes.eq, uint.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("Long", FilterTypes.gt, "25")]
        [TestCase("Long", FilterTypes.gte, "25")]
        [TestCase("Long", FilterTypes.lt, "25")]
        [TestCase("Long", FilterTypes.lte, "25")]
        [TestCase("Long", FilterTypes.eq, "25")]
        [TestCase("Long", FilterTypes.eq, "-25")]
        [TestCase("Long", FilterTypes.eq, "99999999")]
        [TestCase("Long", FilterTypes.eq, "0")]
        [TestCase("Long", FilterTypes.eq, long.MaxValue)]
        [TestCase("Long", FilterTypes.eq, long.MinValue)]
        [TestCase("NestedModel.Long", FilterTypes.gt, "25")]
        [TestCase("NestedModel.Long", FilterTypes.gte, "25")]
        [TestCase("NestedModel.Long", FilterTypes.lt, "25")]
        [TestCase("NestedModel.Long", FilterTypes.lte, "25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.Long", FilterTypes.eq, "0")]
        [TestCase("NestedModel.Long", FilterTypes.eq, long.MaxValue)]
        [TestCase("NestedModel.Long", FilterTypes.eq, long.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("LongNullable", FilterTypes.gt, "25")]
        [TestCase("LongNullable", FilterTypes.gte, "25")]
        [TestCase("LongNullable", FilterTypes.lt, "25")]
        [TestCase("LongNullable", FilterTypes.lte, "25")]
        [TestCase("LongNullable", FilterTypes.eq, "25")]
        [TestCase("LongNullable", FilterTypes.eq, "-25")]
        [TestCase("LongNullable", FilterTypes.eq, "99999999")]
        [TestCase("LongNullable", FilterTypes.eq, "0")]
        [TestCase("LongNullable", FilterTypes.eq, long.MaxValue)]
        [TestCase("LongNullable", FilterTypes.eq, long.MinValue)]
        [TestCase("NestedModel.LongNullable", FilterTypes.gt, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.gte, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.lt, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.lte, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "-25")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, "0")]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, long.MaxValue)]
        [TestCase("NestedModel.LongNullable", FilterTypes.eq, long.MinValue)]
        // ---------------------------------------------------------------
        [TestCase("ULong", FilterTypes.gt, "25")]
        [TestCase("ULong", FilterTypes.gte, "25")]
        [TestCase("ULong", FilterTypes.lt, "25")]
        [TestCase("ULong", FilterTypes.lte, "25")]
        [TestCase("ULong", FilterTypes.eq, "25")]
        [TestCase("ULong", FilterTypes.eq, "99999999")]
        [TestCase("ULong", FilterTypes.eq, "0")]
        [TestCase("ULong", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("ULong", FilterTypes.eq, ulong.MinValue)]
        [TestCase("NestedModel.ULong", FilterTypes.gt, ulong.MaxValue)]
        [TestCase("NestedModel.ULong", FilterTypes.gte, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.lt, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.lte, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, "25")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, "0")]
        [TestCase("NestedModel.ULong", FilterTypes.eq, ulong.MaxValue)]
        [TestCase("NestedModel.ULong", FilterTypes.eq, ulong.MinValue)]
        public void CustomFilters_ShouldWorkProperlyForRangeWithAllSupportedTypes(string column, FilterTypes filterType, object value)
        {
            var requestModel = new RequestInfoModel
            {
                TableParameters = new DataTableAjaxPostModel
                {
                    Custom = new Custom
                    {
                        Filters = new Dictionary<string, IEnumerable<FilterModel>>
                        {
                            {
                                column, new List<FilterModel>
                                {
                                    new FilterModel { Type = filterType, Value = value.ToString() }
                                }
                            }
                        }
                    }
                }
            };

            var processedData = filter.ProcessData(data, requestModel);
            var predicate = $"{column} {this.GetFilterCsRepresentation(filterType)} {value.ToString()}";
            var expectedData = data.Where(predicate);

            Trace.WriteLine($"Number of results: {processedData.Count()}");
            Assert.AreEqual(processedData.Count(), expectedData.Count());
        }

        private string GetFilterCsRepresentation(FilterTypes filterType)
        {
            switch (filterType)
            {
                case FilterTypes.gte:
                    return ">=";

                case FilterTypes.gt:
                    return ">";

                case FilterTypes.lt:
                    return "<";

                case FilterTypes.lte:
                    return "<=";

                case FilterTypes.eq:
                    return "==";

                default:
                    throw new ArgumentException();
            }
        }
    }
}