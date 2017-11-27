namespace Tests.Integration.Mvc.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        //public static IQueryable<SimpleDataModel> SimpleDataBig { get; set; }
        //public static IQueryable<ComplexDataModel> ComplexDataBig { get; internal set; }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult SimpleDataTestPage(bool isPaged = true, bool searching = true)
        {
            this.ViewBag.IsPaged = isPaged.ToString().ToLower();
            this.ViewBag.Searching = searching.ToString().ToLower();

            return View();
        }

        //[JQDataTable]
        //public ActionResult GetSimpleData(int take = int.MaxValue)
        //{
        //    return this.View(HomeController.SimpleDataBig.Take(take));
        //}

        //public ActionResult GetSimpleDataFull()
        //{
        //    return this.Json(HomeController.SimpleDataBig, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ComplexDataTestPage(bool isPaged = true, bool searching = true)
        {
            this.ViewBag.IsPaged = isPaged.ToString().ToLower();
            this.ViewBag.Searching = searching.ToString().ToLower();

            return View();
        }

        //[JQDataTable]
        //public ActionResult GetComplexData(int take = int.MaxValue)
        //{
        //    return this.View(HomeController.ComplexDataBig.Take(take));
        //}

        //public ActionResult GetComplexDataFull()
        //{
        //    return this.Json(HomeController.ComplexDataBig, JsonRequestBehavior.AllowGet);
        //}
    }
}