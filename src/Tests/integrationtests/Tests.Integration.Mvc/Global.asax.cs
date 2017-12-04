namespace Tests.Integration.Mvc
{
#if TEST_SQL

    using System.Linq;

#endif

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

            this.SetUpData();
        }

        private void SetUpData()
        {
            var data = DataGenerator.GenerateSimpleData(1000, 200);
#if TEST_SQL
            var context = new EntityFrameworkClasses.AppContext();
            if (!context.AllTypesModels.Any())
            {
                context.Seed(data);
            }

            HomeController.Data = context.AllTypesModels;
#else
            HomeController.Data = data;
#endif
        }
    }
}