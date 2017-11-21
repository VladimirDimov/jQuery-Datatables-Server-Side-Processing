namespace JQDT.ModelBinders
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using JQDT.Models;

    /// <summary>
    /// Model binder for the ajax form.
    /// </summary>
    internal class FormModelBinder
    {
        private const string NullAjaxFormException = "Ajax form cannot be null.";
        private const string EmptyAjaxForm = "Ajax form cannot be empty.";

        private const string LengthFormKey = "length";
        private const string StartFormKey = "start";
        private const string SearchValueFormKey = "search[value]";
        private const string CustomFiltersPatternFormKey = @"^custom\[filters\]\[(.+)\]\[(gte|gt|lte|lt|eq)\]$";
        private const string ColumnsOrderableFormKey = "columns[{0}][orderable]";
        private const string ColumnsSearchableFormKey = "columns[{0}][searchable]";

        private const string ColumnsFormKeyPattern = @"^columns\[(\d+)\]\[data\]$";
        private const string ColumnsSearchValueFormKeyPattern = "columns[{0}][search][value]";
        private const string ColumnsSearchRegexFormKeyPattern = "columns[{0}][search][regex]";
        private const string OrderDirectionFormKeyPattern = @"^order\[(\d+)\]\[dir\]$";

        private const string MissingMandatoryKeyException = "The form value \"{0}\" is mandatory but is missing in the ajax request content.";
        private const string NullDataException = "The data collection cannot be null.";
        private const string InvalidDataTypeException = "The data must be a valid collection. The provided data is of type {0}";

        private Dictionary<string, string> ajaxFormDictionary;

        /// <summary>
        /// Binds the model to the ajax form content.
        /// </summary>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data collection.</param>
        /// <typeparam name="T">Generic data model type.</typeparam>
        /// <returns><see cref="RequestInfoModel"/></returns>
        internal RequestInfoModel BindModel<T>(NameValueCollection ajaxForm, T data)
        {
            if (ajaxForm == null)
            {
                throw new ArgumentNullException(NullAjaxFormException);
            }

            if (ajaxForm.Count == 0)
            {
                throw new ArgumentException(EmptyAjaxForm);
            }

            if (data == null)
            {
                throw new ArgumentNullException(NullDataException);
            }

            this.InitializeAjaxFormDictionary(ajaxForm);
            this.ValidateMandatoryValues(LengthFormKey, StartFormKey);

            int start = 0;
            int.TryParse(this.ajaxFormDictionary[StartFormKey], out start);

            int length = 0;
            int.TryParse(this.ajaxFormDictionary[LengthFormKey], out length);

            var datatableModel = new DataTableAjaxPostModel
            {
                Start = start,
                Length = length,
                Search = new Search
                {
                    Value = this.GetSearchValue()
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
                    ModelType = this.GetModelType(data),
                    DataCollectionType = data.GetType()
                }
            };

            return requestInfoModel;
        }

        private Type GetModelType(object data)
        {
            var dataType = data.GetType();
            if (((TypeInfo)dataType).ImplementedInterfaces.Any(t => t == typeof(IEnumerable<>)))
            {
                throw new ArgumentException(string.Format(InvalidDataTypeException, dataType.FullName));
            }

            var modelType = dataType.GenericTypeArguments.First();

            return modelType;
        }

        /// <summary>
        /// Safely attempt to get the search value. If the value is missing in the ajax request content
        /// an empty string is returned.
        /// </summary>
        /// <returns>The search value or empty string if the value is missing.</returns>
        private string GetSearchValue()
        {
            string searchValue = string.Empty;
            this.ajaxFormDictionary.TryGetValue(SearchValueFormKey, out searchValue);

            return searchValue;
        }

        private void ValidateMandatoryValues(params string[] mandatoryKeys)
        {
            foreach (var key in mandatoryKeys)
            {
                if (!this.ajaxFormDictionary.ContainsKey(key))
                {
                    throw new ArgumentException(string.Format(MissingMandatoryKeyException, key));
                }
            }
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
            const string PATTERN = CustomFiltersPatternFormKey;
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
            const string Pattern = ColumnsFormKeyPattern;
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
            if (!this.ajaxFormDictionary.TryGetValue(string.Format(ColumnsSearchValueFormKeyPattern, key), out searchValue))
            {
                return null;
            }

            string isRegexStr = string.Empty;
            bool isRegex = false;
            if (this.ajaxFormDictionary.TryGetValue(string.Format(ColumnsSearchRegexFormKeyPattern, key), out isRegexStr))
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
            if (!this.ajaxFormDictionary.TryGetValue(string.Format(ColumnsOrderableFormKey, key), out isOrderableStr))
            {
                return false;
            }

            var isOrderable = bool.Parse(isOrderableStr);

            return isOrderable;
        }

        private bool IsSearchable(int key)
        {
            string isSearchableStr;
            if (!this.ajaxFormDictionary.TryGetValue(string.Format(ColumnsSearchableFormKey, key), out isSearchableStr))
            {
                return false;
            }

            var isSearchable = bool.Parse(isSearchableStr);

            return isSearchable;
        }

        private List<Order> GetOrderList()
        {
            var orders = new List<Order>();
            const string DirectionPattern = OrderDirectionFormKeyPattern;
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

            return orders;
        }
    }
}