using System.Linq;
using JQDT.DataProcessing;
using JQDT.DI;
using NUnit.Framework;
using Tests.UnitTests.Common;
using Tests.UnitTests.Models;

namespace Tests.UnitTests
{
    internal class FullCycleTests
    {
        private IDataProcess<SimpleModel> filter;
        private IQueryable<SimpleModel> data;
        private const int RangeConstant = 50;

        [SetUp]
        public void SetUp()
        {
            var resolver = new DependencyResolver();
            this.filter = resolver.GetCustomFiltersDataProcessor<SimpleModel>();
            this.data = DataGenerator.GenerateSimpleData(5000, RangeConstant);
        }
    }
}