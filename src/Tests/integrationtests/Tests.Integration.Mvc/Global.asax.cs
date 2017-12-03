namespace Tests.Integration.Mvc
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using TestData.Data;
    using Tests.Integration.Mvc.Configuration;
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
            var dataSource = SettingsProvider.Get("dataSource");
            var data = DataGenerator.GenerateSimpleData(1000, 200);
            switch (dataSource)
            {
                case "sql":
                    var context = new EntityFrameworkClasses.AppContext();
                    if (!context.AllTypesModels.Any())
                    {
                        context.Seed(data);
                    }

                    HomeController.Data = context.AllTypesModels;
                    break;

                case "mem":
                    HomeController.Data = data;
                    break;

                default:
                    break;
            }
        }
    }
}