namespace TestData.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using JQDT;
    using TestData.Data.Models;

    [AllowCrossSite]
    public class HomeController : Controller
    {
        public static IQueryable<SimpleDataModel> SimpleDataBig { get; set; }
        public static IQueryable<SimpleDataModel> SimpleDataSmall { get; set; }

        [JQDataTable]
        public ActionResult GetSimpleData(int take = int.MaxValue)
        {
            return this.View(HomeController.SimpleDataBig.Take(take));
        }

        [JQDataTable]
        public ActionResult GetSimpleDataSmall()
        {
            return this.View(HomeController.SimpleDataBig);
        }

        [JQDataTable]
        public ActionResult GetSimpleDataEmpty()
        {
            return this.View(new List<SimpleDataModel>().AsQueryable());
        }
    }

    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            base.OnActionExecuting(filterContext);
        }
    }
}