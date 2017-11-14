namespace Tests.SeleniumTests.Common
{
    using System.Collections.Generic;
    using System.Net.Http;
    using TestData.Data.Models;

    public static class DataHelpers
    {
        public static IEnumerable<SimpleDataModel> GetSimpleDataFull(ISettingsProvider settings)
        {
            var http = new HttpClient();
            var res = http.GetAsync(settings["serverUrl"] + "home/GetSimpleDataFull").Result;
            string responseBody = res.Content.ReadAsStringAsync().Result;
            var fullData = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SimpleDataModel>>(responseBody);

            return fullData;
        }
    }
}