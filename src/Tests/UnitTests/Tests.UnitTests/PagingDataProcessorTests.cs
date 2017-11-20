namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.DataProcessing;
    using NUnit.Framework;
    using Tests.UnitTests.Common;
    using Tests.UnitTests.Models;

    internal class PagingDataProcessorTests
    {
        private PagingDataProcessor<SimpleModel> filter;
        private IQueryable<SimpleModel> simpleData;

        [SetUp]
        public void SetUp()
        {
            this.filter = new PagingDataProcessor<SimpleModel>();
            this.simpleData = new List<SimpleModel>().AsQueryable();
        }

        [Test]
        public void ShouldReturnAppropriateResultWhenLengthIsNegative()
        {
            var requestModel = TestHelpers.GetSimpleRequestInfoModel();

            requestModel.TableParameters.Length = -1;

            var actualExpr = this.filter.ProcessData(this.simpleData, requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(SimpleModel).FullName}]";

            Assert.AreEqual(expectedExprStr, actualExprStr);

            Assert.DoesNotThrow(() =>
            {
                var tmp = actualExpr.ToList();
            });
        }

        [Test]
        public void ShouldReturnAppropriateResultWhenAppropriatePagingParameters()
        {
            var requestModel = TestHelpers.GetSimpleRequestInfoModel();
            var data = new List<SimpleModel>();
            const int Start = 20;
            const int Length = 10;

            for (int i = 0; i < 30; i++)
            {
                data.Add(new SimpleModel
                {
                    Integer = i
                });
            }

            requestModel.TableParameters.Length = Length;
            requestModel.TableParameters.Start = Start;

            var actualExpr = this.filter.ProcessData(data.AsQueryable(), requestModel);
            var actualExprStr = actualExpr.Expression.ToString();
            var expectedExprStr = $"System.Collections.Generic.List`1[{typeof(SimpleModel).FullName}].Skip({Start}).Take({Length})";

            Assert.AreEqual(expectedExprStr, actualExprStr);

            var actualRange = actualExpr.ToList().Select(x => ((SimpleModel)x).Integer);
            var expectedRange = Enumerable.Range(Start, Length);
            Assert.IsTrue(expectedRange.SequenceEqual(actualRange));
        }
    }
}