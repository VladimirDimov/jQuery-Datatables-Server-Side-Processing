namespace Tests.SeleniumTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Tests.SeleniumTests.Common;
    using Tests.SeleniumTests.Pages;

    internal class FilterTests
    {
        private AppSettingsProvider settings;

        [SetUp]
        public void SetUp()
        {
            this.settings = new AppSettingsProvider();
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
            };

            using (var driver = new ChromeDriver())
            {
                foreach (var filter in filterValues)
                {
                    var page = new HomePage(driver, settings).GoToSimpleDataNoPagingPage();
                    IWebElement filterInputElement = page.GetFilterInputElement();

                    filterInputElement.SendKeys(filter);
                    Thread.Sleep(1000);
                    var rows = page.GetRowElements();
                    var rowsText = rows.Select(e => e.Text);

                    Assert.IsTrue(rowsText.All(x => x.ToLower().Contains(filter.ToLower())));
                }
            }
        }
    }
}