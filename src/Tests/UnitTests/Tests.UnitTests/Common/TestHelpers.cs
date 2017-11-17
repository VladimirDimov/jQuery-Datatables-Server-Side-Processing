namespace Tests.UnitTests.Common
{
    using System.Collections.Generic;
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
    }
}