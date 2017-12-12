namespace Examples.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data;
    using Examples.Mvc.ViewModels;
    using JQDT.Models;
    using JQDT.MVC;

    public class CustomersController : Controller
    {
        private AdventureWorks context;

        public CustomersController()
        {
            this.context = new Data.AdventureWorks();
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [CustomJQDataTable]
        public ActionResult GetCustomersData()
        {
            var data = this.context.Customers.Select(x => new CustomerViewModel
            {
                CustomerID = x.CustomerID,
                AccountNumber = x.AccountNumber,
                Person = new PersonViewModel
                {
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                },
                Store = new StoreViewModel
                {
                    Name = x.Store.Name,
                }
            });

            return this.View(data);
        }
    }

    public class CustomJQDataTableAttribute : JQDataTableAttribute
    {
        public override void OnDataProcessed(object data, RequestInfoModel requestInfoModel)
        {
            var processedData = ((IQueryable<CustomerViewModel>)data).Where(x => x.CustomerID > 10 && x.CustomerID < 20);
            var list = processedData.ToList();
            data = list;
        }
    }
}