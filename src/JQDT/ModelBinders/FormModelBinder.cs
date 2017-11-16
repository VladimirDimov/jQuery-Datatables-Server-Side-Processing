namespace JQDT.ModelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text.RegularExpressions;
    using JQDT.Models;

    /// <summary>
    /// Model binder for the ajax form.
    /// </summary>
    internal class FormModelBinder
    {
        private Dictionary<string, string> ajaxFormDictionary;

        /// <summary>
        /// Binds the model to the ajax form content.
        /// </summary>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data collection.</param>
        /// <returns><see cref="RequestInfoModel"/></returns>
        public RequestInfoModel BindModel(NameValueCollection ajaxForm, object data)
        {
            this.InitializeAjaxFormDictionary(ajaxForm);

            var lengthStr = this.ajaxFormDictionary["length"];

            // TODO: Throw appropriate exceptions when mandatory value is missing;
            int start = 0;
            int.TryParse(this.ajaxFormDictionary["start"], out start);

            int length = 0;
            int.TryParse(this.ajaxFormDictionary["length"], out length);

            var datatableModel = new DataTableAjaxPostModel
            {
                Start = start,
                Length = length,
                Search = new Search
                {
                    Value = this.ajaxFormDictionary["search[value]"]
                },
                Order = this.GetOrderList(),
                Columns = this.GetColumns(),
                Custom = this.GetCustom()
            };

            var requestInfoModel = new RequestInfoModel
            {
                TableParameters = datatableModel,
                Helpers = new RequestHelpers
                {
                    ModelType = data.GetType().GenericTypeArguments.First()
                }
            };

            return requestInfoModel;
        }

        private void InitializeAjaxFormDictionary(NameValueCollection ajaxForm)
        {
            this.ajaxFormDictionary = new Dictionary<string, string>();
            foreach (var key in ajaxForm.AllKeys)
            {
                this.ajaxFormDictionary.Add(key, ajaxForm[key]);
            }
        }

        private Custom GetCustom()
        {
            var custom = new Custom
            {
                Filters = this.GetCustomFilters()
            };

            return custom;
        }

        private Dictionary<string, IEnumerable<FilterModel>> GetCustomFilters()
        {
            const string PATTERN = @"^custom\[filters\]\[(.+)\]\[(gte|gt|lte|lt)\]$";
            var filters = new Dictionary<string, IEnumerable<FilterModel>>();
            foreach (var key in this.ajaxFormDictionary.Keys)
            {
                var match = Regex.Match(key, PATTERN);
                if (match.Success)
                {
                    if (!filters.ContainsKey(match.Groups[1].Value))
                    {
                        filters[match.Groups[1].Value] = new List<FilterModel>();
                    }

                    ((ICollection<FilterModel>)filters[match.Groups[1].Value]).Add(new FilterModel
                    {
                        Type = (FilterTypes)Enum.Parse(typeof(FilterTypes), match.Groups[2].Value),
                        Value = this.ajaxFormDictionary[key]
                    });
                }
            }

            return filters;
        }

        private List<Column> GetColumns()
        {
            const string Pattern = @"^columns\[(\d+)\]\[data\]$";
            var columns = new List<Column>();
            var colData = new SortedList<int, string>();
            foreach (var key in this.ajaxFormDictionary.Keys)
            {
                var matches = Regex.Match(key, Pattern);

                if (matches.Success)
                {
                    colData.Add(int.Parse(matches.Groups[1].Value), this.ajaxFormDictionary[key]);
                }
            }

            foreach (var item in colData)
            {
                columns.Add(new Column
                {
                    Data = item.Value,
                    Searchable = this.IsSearchable(item.Key),
                    Orderable = this.IsOrderable(item.Key),
                    Search = this.GetSearch(item.Key)
                });
            }

            return columns;
        }

        private Search GetSearch(int key)
        {
            string searchValue = null;
            if (!this.ajaxFormDictionary.TryGetValue($"columns[{key}][search][value]", out searchValue))
            {
                return null;
            }

            string isRegexStr = string.Empty;
            bool isRegex = false;
            if (this.ajaxFormDictionary.TryGetValue($"columns[{key}][search][regex]", out isRegexStr))
            {
                isRegex = bool.Parse(isRegexStr);
            }

            var search = new Search
            {
                Value = searchValue,
                Regex = isRegex
            };

            return search;
        }

        private bool IsOrderable(int key)
        {
            string isOrderableStr;
            if (!this.ajaxFormDictionary.TryGetValue($"columns[{key}][orderable]", out isOrderableStr))
            {
                return false;
            }

            var isOrderable = bool.Parse(isOrderableStr);

            return isOrderable;
        }

        private bool IsSearchable(int key)
        {
            string isSearchableStr;
            if (!this.ajaxFormDictionary.TryGetValue($"columns[{key}][searchable]", out isSearchableStr))
            {
                return false;
            }

            var isSearchable = bool.Parse(isSearchableStr);

            return isSearchable;
        }

        private List<Order> GetOrderList()
        {
            var orders = new List<Order>();
            const string DirectionPattern = @"^order\[(\d+)\]\[dir\]$";
            var colNumbers = new List<int>();
            foreach (var key in this.ajaxFormDictionary.Keys)
            {
                var match = Regex.Match(key, DirectionPattern);
                if (match.Success)
                {
                    var index = int.Parse(match.Groups[1].Value);
                    var columnKey = $"order[{index}][column]";

                    orders.Add(new Order
                    {
                        Column = int.Parse(this.ajaxFormDictionary[columnKey]),
                        Dir = this.ajaxFormDictionary[key]
                    });
                }
            }

            var col = this.ajaxFormDictionary["order[0][column]"];

            return orders;
        }
    }
}