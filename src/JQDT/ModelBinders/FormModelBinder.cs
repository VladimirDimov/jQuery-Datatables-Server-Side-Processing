namespace JQDT.ModelBinders
{
    using JQDT.Models;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System;

    internal class FormModelBinder
    {
        public RequestInfoModel BindModel(ActionExecutedContext filterContext)
        {
            var controllerContext = filterContext.Controller.ControllerContext;

            var ajaxForm = ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)filterContext.RequestContext.HttpContext).Request).Form;
            var lengthStr = ajaxForm["length"];

            // TODO: Throw appropriate exceptions when mandatory value is missing;
            int start = 0;
            int.TryParse(ajaxForm["start"], out start);

            int length = 0;
            int.TryParse(ajaxForm["length"], out length);

            var datatableModel = new DataTableAjaxPostModel
            {
                start = start,
                length = length,
                search = new Search
                {
                    value = ajaxForm["search[value]"]
                },
                order = this.GetOrderList(ajaxForm),
                columns = this.GetColumns(ajaxForm),
                Custom = this.GetCustom(ajaxForm)
            };

            var requestInfoModel = new RequestInfoModel
            {
                TableParameters = datatableModel,
                Helpers = new RequestHelpers
                {
                    ModelType = ((ViewResultBase)filterContext.Result).Model.GetType().GenericTypeArguments.First()
                }
            };

            return requestInfoModel;
        }

        private Custom GetCustom(NameValueCollection ajaxForm)
        {
            var custom = new Custom
            {
                Ranges = this.GetCustomRanges(ajaxForm)
            };

            return custom;
        }

        private Dictionary<string, IEnumerable<RangeModel>> GetCustomRanges(NameValueCollection ajaxForm)
        {
            const string pattern = @"^custom\[ranges\]\[(.+)\]\[(gte|gt|lte|lt)\]$";
            var ranges = new Dictionary<string, IEnumerable<RangeModel>>();
            foreach (var key in ajaxForm.AllKeys)
            {
                var match = Regex.Match(key, pattern);
                if (match.Success)
                {

                    if (!ranges.ContainsKey(match.Groups[1].Value))
                    {
                        ranges[match.Groups[1].Value] = new List<RangeModel>();
                    }

                    ((ICollection<RangeModel>)ranges[match.Groups[1].Value]).Add(new RangeModel
                    {
                        Type = (RangeTypes)Enum.Parse(typeof(RangeTypes), match.Groups[2].Value),
                        Value = ajaxForm[key]
                    });
                }
            }

            return ranges;
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
                    data = item.Value
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
                        column = int.Parse(form[columnKey]),
                        dir = form[key]
                    });
                }
            }

            var col = form["order[0][column]"];

            return orders;
        }
    }
}