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

        public ActionResult AllTypesData(bool paging = true, bool searching = true)
        {
            this.ViewBag.Paging = paging.ToString().ToLower();
            this.ViewBag.Searching = searching.ToString().ToLower();

            return View();
        }

        [JQDataTable]
        public ActionResult GetData()
        {
            return this.View(Data);
        }
    }
}