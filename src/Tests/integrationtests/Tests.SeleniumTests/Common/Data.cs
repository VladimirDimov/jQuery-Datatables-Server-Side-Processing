using System.Net.Http;

namespace Tests.SeleniumTests.Common
{
    public static class Data
    {
        public static T GetDataFromServer<T>(string url)
        {
            var http = new HttpClient();
            var result = http.GetAsync(url).Result;
            var jsonString = result.Content.ReadAsStringAsync().Result;
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);

            return json;
        }
    }
}