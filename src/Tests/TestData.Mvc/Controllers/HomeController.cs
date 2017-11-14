namespace TestData.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using JQDT;
    using TestData.Data.Models;

    [AllowCrossSite]
    public class HomeController : Controller
    {
        public static IQueryable<SimpleDataModel> SimpleData { get; set; }

        [JQDataTable]
        public ActionResult GetSimpleData()
        {
            return this.View(HomeController.SimpleData);
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