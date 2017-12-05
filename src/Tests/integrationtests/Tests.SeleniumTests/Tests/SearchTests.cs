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

        private static readonly object[] searchByStringTestCases =
        {
            new object[]{ 1, new Func<AllTypesModel, string>(x => x.StringProperty)},
            new object[]{ 2, new Func<AllTypesModel, string>(x => x.NestedModel.StringProperty)},
        };

        [Test, TestCaseSource(nameof(searchByStringTestCases))]
        public void SearchByStringFiledShouldWorkProperlyAndCaseInsensitive(int caseNumber, Func<AllTypesModel, string> selector)
        {
            var columnName1 = "StringProperty";
            var columnName2 = "Nested Model StringProperty";

            for (int i = 0; i < 5; i++)
            {
                this.navigator.AllTypesDataPage().GoTo("?showChar=false");
                var tableElement = new TableElement("table", this.driver);
                var searchValue = this.GetRandomSubstringContainedInData(selector).RandomiseCase();
                tableElement.TypeInSearchBox(searchValue);
                Thread.Sleep(GlobalConstants.GlobalThreadSleep);
                var filteredValues1 = tableElement.GetColumnRowValues(columnName1).ToList();
                var filteredValues2 = tableElement.GetColumnRowValues(columnName2).ToList();
                var joinedColumns = new List<string>();
                for (int j = 0; j < filteredValues1.Count(); j++)
                {
                    joinedColumns.Add(filteredValues1[j] + filteredValues2[j]);
                }

                Assert.IsTrue(joinedColumns.All(x => x.ToLower().Contains(searchValue.ToLower())));
            }
        }

        private static readonly object[] searchByCharTestCases =
       {
            new object[]{ 1, new Func<AllTypesModel, string>(x => x.CharProperty.ToString())},
            new object[]{ 2, new Func<AllTypesModel, string>(x => x.CharNullable.ToString())},
            new object[]{ 3, new Func<AllTypesModel, string>(x => x.NestedModel.CharProperty.ToString())},
            new object[]{ 4, new Func<AllTypesModel, string>(x => x.NestedModel.CharNullable.ToString())},
        };

        [Test, TestCaseSource(nameof(searchByCharTestCases))]
        public void SearchByCharFiledShouldWorkProperlyAndCaseInsensitive(int testCase, Func<AllTypesModel, string> selector)
        {
            var colNames = new string[]
            {
                "CharProperty", "CharNullable", "Nested Model CharProperty", "Nested Model CharNullable"
            };

            for (int i = 0; i < 2; i++)
            {
                this.navigator.AllTypesDataPage().GoTo("?showString=false");
                var tableElement = new TableElement("table", this.driver);
                var searchValue = this.GetRandomCharContainedInDataSwitchedCase(selector).ToString().RandomiseCase();
                tableElement.TypeInSearchBox(searchValue);
                Thread.Sleep(GlobalConstants.GlobalThreadSleep);
                var columnValuesCollection = new List<IList<string>>();
                foreach (var colName in colNames)
                {
                    columnValuesCollection.Add(tableElement.GetColumnRowValues(colName).ToList());
                }

                var joinedColumns = this.ConcatItems(columnValuesCollection.ToArray());

                Assert.IsNotEmpty(joinedColumns);
                Assert.IsTrue(joinedColumns.All(x => x.ToLower().Contains(searchValue.ToLower())));
            }
        }

        private IEnumerable<string> ConcatItems(params IList<string>[] collections)
        {
            var result = new string[collections.First().Count()];
            foreach (var collection in collections)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    result[i] = result[i] == null ? collection[i] : result[i] + collection[i];
                }
            }

            return result.ToList();
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

        /// <summary>
        /// Gets a random character contained in data and switches it's case. If it is a lowercase returns upper case and vice versa.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns><see cref="char"/></returns>
        private char GetRandomCharContainedInDataSwitchedCase(Func<AllTypesModel, string> selector)
        {
            var random = new Random();
            var data = Data.GetDataFromServer<IEnumerable<AllTypesModel>>(GlobalConstants.ServerBaseUrl + GlobalConstants.AllTypesModelFullDataUrl);
            var dataLength = data.Count();
            var startIndex = random.Next((int)(0.3 * dataLength), (int)(0.7 * dataLength));
            var lookInDataPart = data.Skip(startIndex);
            var item = lookInDataPart.First(x => selector(x) != null && selector(x).ToString() != string.Empty);
            var character = selector(item)[0];
            var switchedCaseChar = Char.IsLower(character) ? Char.ToUpper(character) : Char.ToLower(character);

            return switchedCaseChar;
        }
    }
}