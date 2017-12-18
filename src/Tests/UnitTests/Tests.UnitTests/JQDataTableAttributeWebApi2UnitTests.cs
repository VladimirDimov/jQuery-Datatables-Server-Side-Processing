using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;
using JQDT.Application;
using JQDT.DI;
using JQDT.ModelBinders;
using JQDT.Models;
using JQDT.WebAPI;
using Moq;
using NUnit.Framework;
using Tests.UnitTests.Mocks;
using Tests.UnitTests.Models;

namespace Tests.UnitTests
{
    public class JQDataTableAttributeWebApi2UnitTests
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
        public void AllEventsInsideWebApiJQDTAttributeShouldBeRaized(string eventName)
        {
            var serviceLocatorMock = this.GetServiceLocatorMock();
            var executeFunctionMock = this.GetExecuteFunctionProviderMock(serviceLocatorMock);
            var contextFake = this.GetHttpContextFake();

            var testAttr = new JQDataTableWebApi2TestAttribute(serviceLocatorMock.Object, executeFunctionMock.Object);

            testAttr.OnActionExecuted(contextFake);

            Assert.IsTrue(testAttr.CalledEvents.Contains(eventName));
        }

        [Test]
        public void AllEventsInsideWebApi2JQDTAttributeShouldBeCalledInCorrectOrder()
        {
            var serviceLocatorMock = this.GetServiceLocatorMock();
            var executeFunctionMock = this.GetExecuteFunctionProviderMock(serviceLocatorMock);
            var contextFake = this.GetHttpContextFake();

            var testAttr = new JQDataTableWebApi2TestAttribute(serviceLocatorMock.Object, executeFunctionMock.Object);

            testAttr.OnActionExecuted(contextFake);

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

            Assert.IsTrue(expectedEventsCalls.SequenceEqual(testAttr.CalledEvents));
        }

        private HttpActionExecutedContext GetHttpContextFake()
        {
            var contextFake = new HttpActionExecutedContext(
            new HttpActionContext(
                new HttpControllerContext(
                    new HttpConfiguration(),
                    new Mock<IHttpRouteData>().Object,
                    new HttpRequestMessage()),
            new Mock<HttpActionDescriptor>().Object),
            null);

            contextFake.Response = new HttpResponseMessage()
            {
                Content = new ObjectContent(typeof(IQueryable<IntModel>), new List<IntModel>().AsQueryable(), new JsonMediaTypeFormatter()),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            return contextFake;
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

        private Mock<IExecuteFunctionProvider<HttpActionExecutedContext>> GetExecuteFunctionProviderMock(Mock<IServiceLocator> serviceLocatorMock)
        {
            var modelBinderMock = new Mock<IFormModelBinder>();
            modelBinderMock.Setup(x => x.BindModel<IQueryable<IntModel>>(It.IsAny<NameValueCollection>(), It.IsAny<IQueryable<IntModel>>())).Returns(new RequestInfoModel() { TableParameters = new DataTableAjaxPostModel { Draw = 0 } });

            var executeFunctionProviderMock = new Mock<IExecuteFunctionProvider<HttpActionExecutedContext>>();
            executeFunctionProviderMock.Setup(x => x.GetAppInicializationFunc(It.IsAny<Type>(), It.IsAny<Type>())).Returns(new Func<HttpActionExecutedContext, IServiceLocator, IFormModelBinder, IApplicationBase>((ac, sl, fmb) => { return new TestApplication<IntModel>(serviceLocatorMock.Object, modelBinderMock.Object); }));

            return executeFunctionProviderMock;
        }
    }

    public class JQDataTableWebApi2TestAttribute : JQDataTableAttribute
    {
        public List<string> CalledEvents { get; private set; }

        internal JQDataTableWebApi2TestAttribute(IServiceLocator serviceLocator, IExecuteFunctionProvider<HttpActionExecutedContext> executeFunctionProvider)
            : base(serviceLocator, executeFunctionProvider)
        {
            this.CalledEvents = new List<string>();
        }

        public override void OnDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnDataProcessing));
        }

        public override void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnDataProcessed));
        }

        public override void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnSearchDataProcessing));
        }

        public override void OnSearchDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnSearchDataProcessed));
        }

        public override void OnCustomFiltersDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessing));
        }

        public override void OnCustomFiltersDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnCustomFiltersDataProcessed));
        }

        public override void OnColumnsFilterDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessing));
        }

        public override void OnColumnsFilterDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnColumnsFilterDataProcessed));
        }

        public override void OnSortDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnSortDataProcessing));
        }

        public override void OnSortDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnSortDataProcessed));
        }

        public override void OnPagingDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnPagingDataProcessing));
        }

        public override void OnPagingDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            this.CalledEvents.Add(nameof(JQDataTableAttribute.OnPagingDataProcessed));
        }
    }
}