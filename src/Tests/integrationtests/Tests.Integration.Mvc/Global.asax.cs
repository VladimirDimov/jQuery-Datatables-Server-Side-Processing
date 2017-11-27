namespace Tests.Integration.Mvc
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using TestData.Data;
    using Tests.Integration.Mvc.Controllers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HomeController.Data = DataGenerator.GenerateSimpleData(5000, 200);
        }
    }
}