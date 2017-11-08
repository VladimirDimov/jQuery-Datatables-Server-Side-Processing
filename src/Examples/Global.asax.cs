namespace WebApplication1
{
    using System;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using WebApplication1.Controllers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var random = new Random();

            for (int i = 0; i < 5000; i++)
            {
                HomeController.peopleCollection.Add(new Person
                {
                    Id = i + 1,
                    Name = $"First{i + 1} Last{i + 1}",
                    Age = 10 + random.Next(0, 90),
                    Address = new Address
                    {
                        City = $"City{random.Next(1, 10)}",
                        Country = $"Country{random.Next(1, 3)}",
                        Street = new Street
                        {
                            Name = $"Street_{random.Next(1, 5)}",
                            Number = random.Next(1, 100)
                        }
                    },
                    StartingDate = DateTime.Now.AddDays(random.Next(-5000, -30))
                });
            }
        }
    }
}