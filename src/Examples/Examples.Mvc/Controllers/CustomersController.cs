namespace Examples.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data;
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

        [JQDataTable]
        public ActionResult GetCustomersData()
        {
            return this.View(this.context.Customers.Select(x => new
            {
                CustomerID = x.CustomerID,
                Person = new PersonModel
                {
                    FirstName = x.Person.FirstName
                },
                Store = new StoreModel
                {
                    Name = x.Store.Name
                }
            }));
        }
    }

    public partial class PersonModel
    {
        public int BusinessEntityID { get; set; }

        public string PersonType { get; set; }

        public bool NameStyle { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public int EmailPromotion { get; set; }

        public string AdditionalContactInfo { get; set; }

        public string Demographics { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual BusinessEntity BusinessEntity { get; set; }
    }

    public partial class StoreModel
    {
        public int BusinessEntityID { get; set; }

        public string Name { get; set; }

        public int? SalesPersonID { get; set; }

        public string Demographics { get; set; }

        public string rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual BusinessEntity BusinessEntity { get; set; }
    }
}