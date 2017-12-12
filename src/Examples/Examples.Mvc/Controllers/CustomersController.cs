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
        public override void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            var list = ((IQueryable<CustomerViewModel>)data).ToList();
            list.ForEach(x => x.Person.FirstName = $"FN {x.Person.FirstName}");
            data = list.AsQueryable();
        }

        public override void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            var queryable = data as IOrderedQueryable<CustomerViewModel>;
            data = queryable.Where(x => x.CustomerID % 2 == 0);
        }
    }
}