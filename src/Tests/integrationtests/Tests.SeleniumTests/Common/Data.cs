using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Tests.SeleniumTests.Common
{
    public static class Data
    {
        private static Dictionary<Type, object> cachedData = new Dictionary<Type, object>();

        public static T GetDataFromServer<T>(string url)
        {
            if (cachedData.ContainsKey(typeof(T)))
            {
                return (T)cachedData[typeof(T)];
            }

            var http = new HttpClient();
            var result = http.GetAsync(url).Result;
            var jsonString = result.Content.ReadAsStringAsync().Result;
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            cachedData.Add(typeof(T), json);

            return json;
        }
    }
}