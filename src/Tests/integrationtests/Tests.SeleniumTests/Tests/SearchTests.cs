using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using TestData.Models;
using Tests.Helpers;
using Tests.SeleniumTests.Common;

namespace Tests.SeleniumTests.Tests
{
    public class SearchTests
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
        public void SearchByStringFiledShouldWorkProperlyAndCaseInsensitive()
        {
            Func<AllTypesModel, string> selector = x => x.StringProperty;

            for (int i = 0; i < 5; i++)
            {
                this.navigator.AllTypesDataPage().GoTo("?showChar=false");
                var tableElement = new TableElement("table", this.driver);
                var searchValue = this.GetRandomSubstringContainedInData(x => x.StringProperty).RandomiseCase();
                tableElement.TypeInSearchBox(searchValue);
                Thread.Sleep(GlobalConstants.GlobalThreadSleep);
                var filteredValues = tableElement.GetColumnRowValues("StringProperty");
                Assert.IsTrue(filteredValues.All(x => x.ToLower().Contains(searchValue.ToLower())));
            }
        }

        [Test]
        public void SearchByCharFiledShouldWorkProperly()
        {
            Assert.Warn(GlobalConstants.NotCompletedTest);
        }

        [Test]
        public void SearchShouldIncludeAllTextFields()
        {
            Assert.Warn(GlobalConstants.NotCompletedTest);
        }

        private string GetRandomSubstringContainedInData(Func<AllTypesModel, string> selector)
        {
            var random = new Random();
            var data = Data.GetDataFromServer<IEnumerable<AllTypesModel>>(GlobalConstants.ServerBaseUrl + GlobalConstants.AllTypesModelFullDataUrl);
            var dataLength = data.Count();
            var startIndex = random.Next((int)(0.3 * dataLength), (int)(0.7 * dataLength));
            var lookInDataPart = data.Skip(startIndex);
            var item = lookInDataPart.First(x => selector(x) != null && selector(x).Length >= 5);
            var str = selector(item);
            var substr = str.Substring(0, 2);

            return substr;
        }
    }
}