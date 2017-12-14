namespace Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using JQDT.Application;
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
        public void OnDataProcessingEventShouldBeRaized()
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
        public void OnDataProcessedEventShouldBeRaized()
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
        public void OnSearchDataProcessingEventShouldBeRaized()
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

        [Test]
        public void OnSearchDataProcessedEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnSearchDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnColumnsFilterDataProcessingEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnColumnsFilterDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnColumnsFilterDataProcessedEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnColumnsFilterDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnCustomFiltersDataProcessingEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnCustomFiltersDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnCustomFiltersDataProcessedEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnCustomFiltersDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnPagingDataProcessingEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnPagingDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnPagingDataProcessedEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnPagingDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnSortDataProcessingEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnSortDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void OnSortDataProcessedEventShouldBeRaized()
        {
            var app = this.GetAppMock();
            bool flag = false;
            app.OnSortDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                Trace.WriteLine(flag);
                flag = true;
            };

            app.Execute();

            Assert.IsTrue(flag);
        }

        [Test]
        public void AppBaseEventsShouldBeCalledInCorrectOrder()
        {
            var app = this.GetAppMock();
            var flags = new List<string>();
            app.OnDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnDataProcessingEvent));
            };

            app.OnSearchDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnSearchDataProcessingEvent));
            };

            app.OnSearchDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnSearchDataProcessedEvent));
            };

            app.OnCustomFiltersDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnCustomFiltersDataProcessingEvent));
            };

            app.OnCustomFiltersDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnCustomFiltersDataProcessedEvent));
            };

            app.OnColumnsFilterDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnColumnsFilterDataProcessingEvent));
            };

            app.OnColumnsFilterDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnColumnsFilterDataProcessedEvent));
            };

            app.OnSortDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnSortDataProcessingEvent));
            };

            app.OnSortDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnSortDataProcessedEvent));
            };

            app.OnPagingDataProcessingEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnPagingDataProcessingEvent));
            };

            app.OnPagingDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnPagingDataProcessedEvent));
            };

            app.OnDataProcessedEvent += (ref object data, RequestInfoModel requestInfoModel) =>
            {
                flags.Add(nameof(ApplicationBase<IntModel>.OnDataProcessedEvent));
            };

            app.Execute();

            var expectedEventsCall = new List<string>
            {
                nameof(ApplicationBase<IntModel>.OnDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnSearchDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnSearchDataProcessedEvent),
                nameof(ApplicationBase<IntModel>.OnCustomFiltersDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnCustomFiltersDataProcessedEvent),
                nameof(ApplicationBase<IntModel>.OnColumnsFilterDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnColumnsFilterDataProcessedEvent),
                nameof(ApplicationBase<IntModel>.OnSortDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnSortDataProcessedEvent),
                nameof(ApplicationBase<IntModel>.OnPagingDataProcessingEvent),
                nameof(ApplicationBase<IntModel>.OnPagingDataProcessedEvent),
                nameof(ApplicationBase<IntModel>.OnDataProcessedEvent),
            };

            Assert.IsTrue(expectedEventsCall.SequenceEqual(flags));
        }

        public AppMock<IntModel> GetAppMock()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetSearchDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetColumnsFilterDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetCustomFiltersDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetPagingDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetSortDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());
            var formModelBinderMock = new Mock<IFormModelBinder>();
            formModelBinderMock.Setup(x => x.BindModel(It.IsAny<NameValueCollection>(), It.IsAny<IQueryable<IntModel>>())).Returns(new RequestInfoModel() { Helpers = new RequestHelpers(), TableParameters = new DataTableAjaxPostModel() });
            var app = new AppMock<IntModel>(serviceLocatorMock.Object, formModelBinderMock.Object);

            return app;
        }

        private Mock<IDataProcess<IntModel>> GetDataProcessMock()
        {
            var mock = new Mock<IDataProcess<IntModel>>();
            mock.Setup(x => x.ProcessData(It.IsAny<IQueryable<IntModel>>(), It.IsAny<RequestInfoModel>())).Returns(new List<IntModel>().AsQueryable());
            //mock.Raise(x => x.OnDataProcessedEvent += delegate { });
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