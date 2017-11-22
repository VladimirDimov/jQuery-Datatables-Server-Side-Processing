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
    }
}