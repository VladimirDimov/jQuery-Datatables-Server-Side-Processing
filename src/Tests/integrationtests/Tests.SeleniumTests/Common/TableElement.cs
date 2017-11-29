using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using Tests.SeleniumTests.Enumerations;
using Tests.SeleniumTests.ExtensionMethods;

namespace Tests.SeleniumTests.Common
{
    internal class TableElement
    {
        private readonly string selector;
        private readonly IWebDriver driver;
        private readonly IWebElement table;

        public TableElement(string selector, IWebDriver driver)
        {
            this.selector = selector;
            this.driver = driver;
            this.table = driver.FindElementByCssSelector(selector);
        }

        /// <summary>
        /// Clicks the sort button. If no <paramref name="isAsc"/> value is provided the buton will be clicked single time.
        /// Otherwise the button will be clicked until the order direction is set to asc or desc.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="isAsc">The is asc.</param>
        public void ClickSortButton(string column, SortDirectionsEnum direction = SortDirectionsEnum.Default)
        {
            string classToHave = direction == SortDirectionsEnum.Default ?
                "" :
                (direction == SortDirectionsEnum.Asc ?
                    "sorting_asc" :
                    "sorting_desc");

            IWebElement th;
            do
            {
                var thElements = this.table.FindElementsByCssSelector("thead th");
                th = thElements.Where(x => x.Text.ToLower() == column.ToLower()).Single();

                th.Click();
                Thread.Sleep(1000);
            } while (direction != SortDirectionsEnum.Default && !th.GetAttribute("class").Contains(classToHave));
        }

        public IEnumerable<string> GetColumnRowValues(string columnName)
        {
            var columns = this.table.FindElementsByCssSelector("thead th");
            var colIndex = columns.FirstIndexWhere(x => x.Text.Trim().ToLower() == columnName.Trim().ToLower());
            var rows = this.GetRowElements();
            var columnValues = rows.Select(x => x.FindElementsByCssSelector("td")[colIndex].Text);

            return columnValues;
        }

        public ReadOnlyCollection<IWebElement> GetRowElements()
        {
            return this.table.FindElementsByCssSelector("tbody tr");
        }

        public void GoToLastPage()
        {
            var pageButtons = this.driver.FindElementsByCssSelector(".paginate_button");
            var lastPageButton = pageButtons.Last();
            lastPageButton.Click();
            Thread.Sleep(GlobalConstants.GlobalThreadSleep);
        }
    }
}