namespace Tests.SeleniumTests.Common
{
    using System.Net.Http;

    public static class Http
    {
        public static T Get<T>(string url)
        {
            var http = new HttpClient();
            var res = http.GetAsync(url).Result;
            string responseBody = res.Content.ReadAsStringAsync().Result;
            var fullData = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);

            return fullData;
        }
    }
}