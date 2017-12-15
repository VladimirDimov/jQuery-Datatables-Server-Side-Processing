namespace Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using JQDT.DI;
    using Moq;
    using NUnit.Framework;
    using Tests.UnitTests.Mocks;
    using Tests.UnitTests.Models;

    public class JQDataTableAttributeMvcUnitTests
    {
        [Test]
        public void OnDataProcessingEventShouldBeRaized()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetSearchDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetColumnsFilterDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetCustomFiltersDataProcessor<IntModel>()).Returns(new DataProcessorFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetPagingDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());
            serviceLocatorMock.Setup(x => x.GetSortDataProcessor<IntModel>()).Returns(new DataProcessorNotFilterFake<IntModel>());

            var attribute = new JQDT.MVC.JQDataTableAttribute(serviceLocatorMock.Object);
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

            attribute.OnActionExecuted(actionExecutedContextMock.Object);
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
}