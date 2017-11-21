namespace Tests.SeleniumTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages;

    internal class FilterTests
    {
        private AppSettingsProvider settings;
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            this.settings = new AppSettingsProvider();
            this.driver = DriverProvider.GetDriver();
        }

        [Test]
        public void ShouldFilterSimpleDataProperly()
        {
            var allData = DataHelpers.GetSimpleDataFull(this.settings);
            var randomModel = allData.First(x => x.String.Length >= 6 && x.Double.ToString().Length > 6);
            var filterValues = new List<string>()
            {
                randomModel.String.Substring(1, 3),
                randomModel.String.Substring(1, 3).ToLower(),
                randomModel.String.Substring(1, 3).ToUpper(),
                randomModel.Integer.ToString(),
                randomModel.Double.ToString().Substring(0, 4),
                true.ToString(),
                true.ToString().ToLower(),
                true.ToString().ToUpper(),
                randomModel.Integer.ToString(),
            };

            foreach (var filter in filterValues)
            {
                var page = new HomePage(driver, settings).GoToSimpleDataNoPagingPage();
                IWebElement filterInputElement = page.GetFilterInputElement();

                filterInputElement.SendKeys(filter);
                Thread.Sleep(1000);
                var rows = page.GetRowElements();
                var rowsText = rows.Select(e => e.Text);

                Assert.IsTrue(rowsText.All(x => x.ToLower().Contains(filter.ToLower())), $"Test failed for filter value: {filter}");
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldFilterByNestedPropertiesProperly(bool isPaged)
        {
            var allData = DataHelpers.GetComplexDataFull(this.settings);
            var randomModel = allData.First(x => x.String.Length >= 6 && x.Double.ToString().Length > 6);
            var filterValues = new List<string>()
            {
                randomModel.ComplexModel.String.Substring(1,3),
                randomModel.ComplexModel.SimpleModel.String,
                randomModel.ComplexModel.SimpleModel.Integer.ToString(),
                randomModel.ComplexModel.SimpleModel.Double.ToString().Substring(0, randomModel.ComplexModel.SimpleModel.Double.ToString().Length - 1),

                randomModel.String.Substring(1, 3),
                randomModel.String.Substring(1, 3).ToLower(),
                randomModel.String.Substring(1, 3).ToUpper(),
                randomModel.Integer.ToString(),
                randomModel.Double.ToString().Substring(0, 4),
                true.ToString(),
                randomModel.Integer.ToString(),
            };

            foreach (var filter in filterValues)
            {
                var page = isPaged ?
                    new HomePage(driver, settings).GoToComplexDataPage() :
                    new HomePage(driver, settings).GoToComplexDataNoPagingPage();
                IWebElement filterInputElement = page.GetFilterInputElement();

                filterInputElement.SendKeys(filter);
                Thread.Sleep(1000);
                var rows = page.GetRowElements();
                var rowsText = rows.Select(e => e.Text);

                Assert.IsTrue(rowsText.All(x => x.ToLower().Contains(filter.ToLower())), $"Test failed for filter value: {filter}");
            }
        }
    }
}