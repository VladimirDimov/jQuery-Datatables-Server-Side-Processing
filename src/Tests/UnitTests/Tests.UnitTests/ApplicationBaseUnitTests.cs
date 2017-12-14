namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.ModelBinders;
    using JQDT.Models;
    using Moq;
    using NUnit.Framework;
    using Tests.UnitTests.Mocks;
    using Tests.UnitTests.Models;

    public class ApplicationBaseUnitTests
    {
        [Test]
        public void OnDataProcessingEventShouldBeRized()
        {
            var app = this.GetAppMock();

            bool flag = false;
            app.OnDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnDataProcessedEventShouldBeRized()
        {
            var app = this.GetAppMock();

            bool flag = false;
            app.OnDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnSearchDataProcessingEventShouldBeRized()
        {
            var app = this.GetAppMock();

            bool flag = false;
            app.OnSearchDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        public AppMock<IntModel> GetAppMock()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetSearchDataProcessor<IntModel>()).Returns(this.GetDataFilterMock().Object);
            serviceLocatorMock.Setup(x => x.GetColumnsFilterDataProcessor<IntModel>()).Returns(this.GetDataFilterMock().Object);
            serviceLocatorMock.Setup(x => x.GetCustomFiltersDataProcessor<IntModel>()).Returns(this.GetDataFilterMock().Object);
            serviceLocatorMock.Setup(x => x.GetPagingDataProcessor<IntModel>()).Returns(this.GetDataProcessMock().Object);
            serviceLocatorMock.Setup(x => x.GetSortDataProcessor<IntModel>()).Returns(this.GetDataProcessMock().Object);
            var formModelBinderMock = new Mock<IFormModelBinder>();
            formModelBinderMock.Setup(x => x.BindModel(It.IsAny<NameValueCollection>(), It.IsAny<IQueryable<IntModel>>())).Returns(new RequestInfoModel() { Helpers = new RequestHelpers(), TableParameters = new DataTableAjaxPostModel() });
            var app = new AppMock<IntModel>(serviceLocatorMock.Object, formModelBinderMock.Object);

            return app;
        }

        private Mock<IDataProcess<IntModel>> GetDataProcessMock()
        {
            var mock = new Mock<IDataProcess<IntModel>>();
            mock.Setup(x => x.ProcessData(It.IsAny<IQueryable<IntModel>>(), It.IsAny<RequestInfoModel>())).Returns(new List<IntModel>().AsQueryable());
            mock.Raise(x => x.OnDataProcessedEvent += delegate { });
            return mock;
        }

        private Mock<IDataProcess<IntModel>> GetDataFilterMock()
        {
            var dataFilterMock = new Mock<IDataFilter>();
            var mock = dataFilterMock.As<IDataProcess<IntModel>>();
            mock.Setup(x => x.ProcessData(It.IsAny<IQueryable<IntModel>>(), It.IsAny<RequestInfoModel>())).Returns(new List<IntModel>().AsQueryable());
            mock.Raise(x => x.OnDataProcessedEvent += delegate { });

            return mock;
        }
    }
}