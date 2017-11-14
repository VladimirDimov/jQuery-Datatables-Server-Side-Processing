namespace Tests.Integration.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using JQDT;
    using TestData.Data.Models;

    public class HomeController : Controller
    {
        public static IQueryable<SimpleDataModel> SimpleDataBig { get; set; }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult SimpleDataTestPage()
        {
            return View();
        }

        public ActionResult SimpleDataNoPagingTestPage()
        {
            return this.View();
        }

        [JQDataTable]
        public ActionResult GetSimpleData(int take = int.MaxValue)
        {
            return this.View(HomeController.SimpleDataBig.Take(take));
        }

        public ActionResult GetSimpleDataFull()
        {
            return this.Json(SimpleDataBig, JsonRequestBehavior.AllowGet);
        }
    }
}