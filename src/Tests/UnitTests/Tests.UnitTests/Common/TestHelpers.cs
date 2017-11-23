namespace Tests.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.Models;
    using Tests.UnitTests.Models;

    internal static class TestHelpers
    {
        public static RequestInfoModel GetSimpleRequestInfoModel()
        {
            return new RequestInfoModel()
            {
                Helpers = new RequestHelpers
                {
                    ModelType = typeof(SimpleModel)
                },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = ""
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "String",
                            Orderable = true
                        },
                        new Column
                        {
                            Data = "Integer",
                            Orderable = true
                        }
                    }
                }
            };
        }

        public static RequestInfoModel GetComplexRequestInfoModel()
        {
            return new RequestInfoModel()
            {
                Helpers = new RequestHelpers
                {
                    ModelType = typeof(ComplexModel)
                },
                TableParameters = new DataTableAjaxPostModel
                {
                    Search = new Search
                    {
                        Value = ""
                    },
                    Columns = new List<Column>
                    {
                        new Column{
                            Data = "NestedComplexModel.NestedComplexModel.SimpleModel.String",
                            Orderable = true
                        }
                    },
                    Custom = new Custom()
                }
            };
        }

        public static object GetRandomPropertyValue<T>(IQueryable<T> data, string property)
        {
            var random = new Random();
            var startIndex = random.Next(0, data.Count() / 2);
            var partData = data.Skip(startIndex).Take(data.Count() - startIndex);

            var propInfo = typeof(T).GetProperty(property);
            var isNullable = propInfo.PropertyType.IsGenericType;
            var item = partData.First(x => !isNullable || propInfo.GetValue(x) != null);

            return propInfo.GetValue(item);
        }
    }
}