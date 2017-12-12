using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Tests.SeleniumTests.Common;

namespace Tests.SeleniumTests.Tests
{
    public class OnActionExecutedIntegrationTests
    {
        private IWebDriver driver;

        private Navigator navigator;

        [SetUp]
        public void SetUp()
        {
            this.driver = DriverSingletonProvider.GetDriver();
            this.navigator = new Navigator(driver);
        }

        [Test]
        public void ExpectToAddAdditionalDataFromOnActionExecutedEvent()
        {
            navigator.OnDataExecutedEventTestsPage().GoTo();
            var table = new TableElement("table", this.driver);
            var actualValues = table.GetColumnRowValuesUntilAny("Test");

            var expectedValues = Enumerable.Range(1, 10).Select(x => x.ToString());
            Assert.IsTrue(expectedValues.SequenceEqual(actualValues));
        }
    }
}