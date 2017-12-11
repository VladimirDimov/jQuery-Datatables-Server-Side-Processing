namespace Tests.Integration.WebApi2
{
    using System.Web.Http;
    using TestData.Data;
    using Tests.Integration.WebApi2.Controllers;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            this.SetUpData();
        }

        private void SetUpData()
        {
            var data = DataGenerator.GenerateSimpleData(1000, 200);
#if TEST_SQL
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, Migrations.Configuration>());

            var context = new EntityFrameworkClasses.AppContext();
            if (!context.AllTypesModels.Any())
            {
                context.Seed(data);
            }

            HomeController.Data = context.AllTypesModels.Where(x => x.NestedModel != null);
#else
            HomeController.Data = data;
#endif
        }
    }
}