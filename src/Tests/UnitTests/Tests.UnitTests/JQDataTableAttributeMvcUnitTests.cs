namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using JQDT.Application;
    using JQDT.DI;
    using JQDT.ModelBinders;
    using JQDT.Models;
    using JQDT.MVC;
    using Moq;
    using NUnit.Framework;
    using Tests.UnitTests.Mocks;
    using Tests.UnitTests.Models;

    public class JQDataTableAttributeMvcUnitTests
    {
        [Test]
        [TestCase(nameof(JQDataTableAttribute.OnDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnSearchDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnSearchDataProcessed))]
        [TestCase(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessed))]
        [TestCase(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessed))]
        [TestCase(nameof(JQDataTableAttribute.OnSortDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnSortDataProcessed))]
        [TestCase(nameof(JQDataTableAttribute.OnPagingDataProcessing))]
        [TestCase(nameof(JQDataTableAttribute.OnPagingDataProcessed))]
        [TestCase(nameof(JQDataTableAttribute.OnDataProcessed))]
        public void AllEventsInsideMvcJQDTAttributeShouldBeRaized(string eventName)
        {
            var serviceLocatorMock = this.GetServiceLocatorMock();
            var executeFunctionMock = this.GetExecuteFunctionProviderMock(serviceLocatorMock);
            var contextMock = this.GetHttpContextMock();

            var calledEvents = new List<string>();
            var testAttr = new JQDataTableTestAttribute(serviceLocatorMock.Object, executeFunctionMock.Object, ref calledEvents);

            testAttr.OnActionExecuted(contextMock.Object);

            Assert.IsTrue(calledEvents.Contains(eventName));
        }

        [Test]
        public void AllEventsInsideMvcJQDTAttributeShouldBeCalledInCorrectOrder()
        {
            var serviceLocatorMock = this.GetServiceLocatorMock();
            var executeFunctionMock = this.GetExecuteFunctionProviderMock(serviceLocatorMock);
            var contextMock = this.GetHttpContextMock();

            var calledEvents = new List<string>();
            var testAttr = new JQDataTableTestAttribute(serviceLocatorMock.Object, executeFunctionMock.Object, ref calledEvents);

            testAttr.OnActionExecuted(contextMock.Object);

            var expectedEventsCalls = new List<string>
            {
                nameof(JQDataTableAttribute.OnDataProcessing),
                nameof(JQDataTableAttribute.OnSearchDataProcessing),
                nameof(JQDataTableAttribute.OnSearchDataProcessed),
                nameof(JQDataTableAttribute.OnCustomFiltersDataProcessing),
                nameof(JQDataTableAttribute.OnCustomFiltersDataProcessed),
                nameof(JQDataTableAttribute.OnColumnsFilterDataProcessing),
                nameof(JQDataTableAttribute.OnColumnsFilterDataProcessed),
                nameof(JQDataTableAttribute.OnSortDataProcessing),
                nameof(JQDataTableAttribute.OnSortDataProcessed),
                nameof(JQDataTableAttribute.OnPagingDataProcessing),
                nameof(JQDataTableAttribute.OnPagingDataProcessed),
                nameof(JQDataTableAttribute.OnDataProcessed),
            };

            Assert.IsTrue(expectedEventsCalls.SequenceEqual(calledEvents));
        }

        private Mock<IServiceLocator> GetServiceLocatorMock()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetSearchDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetColumnsFilterDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetCustomFiltersDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetPagingDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetSortDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());

            return serviceLocatorMock;
        }

        private Mock<IExecuteFunctionProvider<ActionExecutedContext>> GetExecuteFunctionProviderMock(Mock<IServiceLocator> serviceLocatorMock)
        {
            var modelBinderMock = new Mock<IFormModelBinder>();
            modelBinderMock.Setup(x => x.BindModel<IQueryable<IntModel>>(It.IsAny<NameValueCollection>(), It.IsAny<IQueryable<IntModel>>())).Returns(new RequestInfoModel() { TableParameters = new DataTableAjaxPostModel { Draw = 0 } });

            var executeFunctionProviderMock = new Mock<IExecuteFunctionProvider<ActionExecutedContext>>();
            executeFunctionProviderMock.Setup(x => x.GetAppInicializationFunc(It.IsAny<Type>(), It.IsAny<Type>())).Returns(new Func<ActionExecutedContext, IServiceLocator, IFormModelBinder, IApplicationBase>((ac, sl, fmb) => { return new TestApplication<IntModel>(serviceLocatorMock.Object, modelBinderMock.Object); }));

            return executeFunctionProviderMock;
        }

        private JQDataTableAttribute HttpGetAttribute(Mock<IServiceLocator> serviceLocatorMock, Mock<IExecuteFunctionProvider<ActionExecutedContext>> executeFunctionProviderMock)
        {
            var attribute = new JQDT.MVC.JQDataTableAttribute(serviceLocatorMock.Object, executeFunctionProviderMock.Object);

            return attribute;
        }

        private Mock<ActionExecutedContext> GetHttpContextMock()
        {
            var request = new Mock<HttpRequestBase>();

            request.SetupGet(r => r.HttpMethod).Returns("GET");
            request.SetupGet(r => r.Url).Returns(new Uri("http://somesite/action"));
            request
                .SetupGet(x => x.Headers)
                .Returns(new WebHeaderCollection()
                {
                    {"X-Requested-With", "XMLHttpRequest"}
                });

            var httpContext = new Mock<HttpContextBase>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);

            var actionExecutedContextMock = new Mock<ActionExecutedContext>();
            actionExecutedContextMock.SetupGet(c => c.Controller).Returns(new TestControllerFake());
            actionExecutedContextMock.SetupGet(c => c.HttpContext).Returns(httpContext.Object);

            return actionExecutedContextMock;
        }
    }

    public class TestControllerFake : ControllerBase
    {
        public TestControllerFake()
        {
            base.ViewData = new ViewDataDictionary() { Model = new List<IntModel>().AsQueryable() };
        }

        protected override void ExecuteCore()
        {
            throw new NotImplementedException();
        }
    }

    public class TestApplication<T> : ApplicationBase<T>
    {
        public TestApplication(IServiceLocator serviceLocator, IFormModelBinder modelBinder)
            : base(serviceLocator, modelBinder)
        {
        }

        protected override NameValueCollection GetAjaxForm()
        {
            return new NameValueCollection();
        }

        protected override IQueryable<T> GetData()
        {
            return new List<T>().AsQueryable();
        }
    }

    public class JQDataTableTestAttribute : JQDataTableAttribute
    {
        private readonly List<string> calledEvents;

        internal JQDataTableTestAttribute(IServiceLocator serviceLocator, IExecuteFunctionProvider<ActionExecutedContext> executeFunctionProvider, ref List<string> calledEvents)
            : base(serviceLocator, executeFunctionProvider)
        {
            this.calledEvents = calledEvents;
        }

        public override void OnDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnDataProcessing));
        }

        public override void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnDataProcessed));
        }

        public override void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnSearchDataProcessing));
        }

        public override void OnSearchDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnSearchDataProcessed));
        }

        public override void OnCustomFiltersDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessing));
        }

        public override void OnCustomFiltersDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessed));
        }

        public override void OnColumnsFilterDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessing));
        }

        public override void OnColumnsFilterDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessed));
        }

        public override void OnSortDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnSortDataProcessing));
        }

        public override void OnSortDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnSortDataProcessed));
        }

        public override void OnPagingDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnPagingDataProcessing));
        }

        public override void OnPagingDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.calledEvents.Add(nameof(JQDataTableAttribute.OnPagingDataProcessed));
        }
    }
}