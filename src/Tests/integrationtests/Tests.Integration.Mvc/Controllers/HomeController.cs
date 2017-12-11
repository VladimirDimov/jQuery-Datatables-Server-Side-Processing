namespace Tests.Integration.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Mvc;
    using JQDT.MVC;
    using TestData.Models;

    public class HomeController : Controller
    {
        public static IQueryable<AllTypesModel> Data { get; set; }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult AllTypesData(bool paging = true, bool searching = true, bool showString = true, bool showChar = true)
        {
            this.ViewBag.Paging = paging.ToString().ToLower();
            this.ViewBag.Searching = searching.ToString().ToLower();
            this.ViewBag.ShowString = showString.ToString().ToLower();
            this.ViewBag.ShowChar = showChar.ToString().ToLower();
            this.ViewBag.dataSourceApp = Configuration.SettingsProvider.Get("dataSourceApp");
            this.ViewBag.WebApi2Url = Configuration.SettingsProvider.Get("webApi2Url");

            return View();
        }

        [JQDataTable]
        public ActionResult GetData()
        {
            return this.View(Data);
        }

        public ActionResult GetFullData()
        {
            var dataSourceApp = Configuration.SettingsProvider.Get("dataSourceApp");
            if (dataSourceApp == "mvc")
            {
                return this.Json(Data, JsonRequestBehavior.AllowGet);
            }
            else if (dataSourceApp == "webapi2")
            {
                var http = new HttpClient();
                var url = Configuration.SettingsProvider.Get("webApi2Url") + "/home";
                var result = http.GetAsync(url).Result;
                var jsonString = result.Content.ReadAsStringAsync().Result;
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<AllTypesModel>>(jsonString);
                Console.WriteLine(jsonString);

                return this.Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new ArgumentException("Invalid data source app " + dataSourceApp);
            }
        }
    }
}