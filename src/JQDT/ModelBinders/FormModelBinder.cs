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
        /// <summary>
        /// Binds the model to the ajax form content.
        /// </summary>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data collection.</param>
        /// <returns><see cref="RequestInfoModel"/></returns>
        public RequestInfoModel BindModel(NameValueCollection ajaxForm, object data)
        {
            var lengthStr = ajaxForm["length"];

            // TODO: Throw appropriate exceptions when mandatory value is missing;
            int start = 0;
            int.TryParse(ajaxForm["start"], out start);

            int length = 0;
            int.TryParse(ajaxForm["length"], out length);

            var datatableModel = new DataTableAjaxPostModel
            {
                Start = start,
                Length = length,
                Search = new Search
                {
                    Value = ajaxForm["search[value]"]
                },
                Order = this.GetOrderList(ajaxForm),
                Columns = this.GetColumns(ajaxForm),
                Custom = this.GetCustom(ajaxForm)
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

        private Custom GetCustom(NameValueCollection ajaxForm)
        {
            var custom = new Custom
            {
                Filters = this.GetCustomFilters(ajaxForm)
            };

            return custom;
        }

        private Dictionary<string, IEnumerable<FilterModel>> GetCustomFilters(NameValueCollection ajaxForm)
        {
            const string PATTERN = @"^custom\[filters\]\[(.+)\]\[(gte|gt|lte|lt)\]$";
            var filters = new Dictionary<string, IEnumerable<FilterModel>>();
            foreach (var key in ajaxForm.AllKeys)
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
                        Value = ajaxForm[key]
                    });
                }
            }

            return filters;
        }

        private List<Column> GetColumns(NameValueCollection ajaxForm)
        {
            const string Pattern = @"^columns\[(\d+)\]\[data\]$";
            var columns = new List<Column>();
            var colData = new SortedList<int, string>();
            foreach (var key in ajaxForm.AllKeys)
            {
                var matches = Regex.Match(key, Pattern);

                if (matches.Success)
                {
                    colData.Add(int.Parse(matches.Groups[1].Value), ajaxForm[key]);
                }
            }

            foreach (var item in colData)
            {
                columns.Add(new Column
                {
                    Data = item.Value
                });
            }

            return columns;
        }

        private List<Order> GetOrderList(NameValueCollection form)
        {
            var orders = new List<Order>();
            const string DirectionPattern = @"^order\[(\d+)\]\[dir\]$";
            var colNumbers = new List<int>();
            foreach (var key in form.AllKeys)
            {
                var match = Regex.Match(key, DirectionPattern);
                if (match.Success)
                {
                    var index = int.Parse(match.Groups[1].Value);
                    var columnKey = $"order[{index}][column]";

                    orders.Add(new Order
                    {
                        Column = int.Parse(form[columnKey]),
                        Dir = form[key]
                    });
                }
            }

            var col = form["order[0][column]"];

            return orders;
        }
    }
}