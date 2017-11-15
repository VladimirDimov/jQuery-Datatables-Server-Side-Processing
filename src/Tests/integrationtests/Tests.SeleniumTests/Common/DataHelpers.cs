namespace Tests.SeleniumTests.Common
{
    using System.Collections.Generic;
    using TestData.Data.Models;

    public static class DataHelpers
    {
        public static IEnumerable<SimpleDataModel> GetSimpleDataFull(ISettingsProvider settings)
        {
            var url = settings["serverUrl"] + "home/GetSimpleDataFull";
            var fullData = Http.Get<IEnumerable<SimpleDataModel>>(url);

            return fullData;
        }

        public static IEnumerable<ComplexDataModel> GetComplexDataFull(ISettingsProvider settings)
        {
            var url = settings["serverUrl"] + "home/GetComplexDataFull";
            var fullData = Http.Get<IEnumerable<ComplexDataModel>>(url);

            return fullData;
        }
    }
}