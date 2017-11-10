namespace JQDT.Tests.IntegrationTests
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using JQDT.Tests.Mocks;
    using JQDT.Tests.Mocks.DataModels;
    using NUnit.Framework;

    public class SimpleSort
    {
        private IQueryable<SimpleDataModel> data;
        private Application application;
        private AjaxFormBuilder ajaxFormBuilder;

        [SetUp]
        public void Init()
        {
            this.ajaxFormBuilder = new AjaxFormBuilder();
            var dataGenerator = new DataGenerator();
            this.data = dataGenerator.GenerateSimpleData(5000, DateTime.Now.AddYears(-5), DateTime.Now);
            this.application = new Application();
        }

        [Test]
        public void SimpleSortShouldWorkOnInteger()
        {
            var ajaxData = this.ajaxFormBuilder.BuildForm(typeof(SimpleDataModel));

            var result = application.Execute(ajaxData, this.data);
        }
    }
}