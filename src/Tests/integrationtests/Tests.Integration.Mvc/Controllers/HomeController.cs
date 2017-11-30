namespace Tests.Integration.Mvc.Controllers
{
    using System.Linq;
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

            return View();
        }

        [JQDataTable]
        public ActionResult GetData()
        {
            return this.View(Data);
        }

        public ActionResult GetFullData()
        {
            return this.Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}