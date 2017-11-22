using System.Linq;
using System.Web.Mvc;
using Examples.Data;
using Examples.Data.ViewModels;
using JQDT;

namespace Examples.Mvc.Controllers
{
    public class AdventureWorksController : Controller
    {
        private readonly AdventureWorks context;

        public AdventureWorksController()
        {
            this.context = new Examples.Data.AdventureWorks();
        }

        // GET: AdventureWorks
        public ActionResult Index()
        {
            return View();
        }

        [JQDataTable]
        public ActionResult GetPeopleData()
        {
            var people = this.context.People.Select(x => new PersonViewModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                Title = x.Title,
                Employee = new EmployeeViewModel
                {
                    BusinessEntityID = x.Employee.BusinessEntityID,
                }
            });

            return this.View(people);
        }

        public ActionResult Products()
        {
            return View();
        }

        [JQDataTable]
        public ActionResult GetProductsData()
        {
            var people = this.context.Products.Select(x => new ProductViewModel
            {
                Id = x.ProductID,
                Name = x.Name,
                ProductModel = new ProductModelViewModel
                {
                    Name = x.ProductModel.Name,
                    ProductSubcategory = x.ProductSubcategory.Name
                },
                ModifiedDate = x.ModifiedDate,
                DiscontinuedDate = x.DiscontinuedDate
            });

            return this.View(people);
        }

        public ActionResult Vendors()
        {
            return this.View();
        }

        [JQDataTable]
        public ActionResult GetVendorsData()
        {
            var data = this.context.Vendors.Select(x => new
            {
                BusinessEntityID = x.BusinessEntityID,
                CreditRating = x.CreditRating,
                ActiveFlag = x.ActiveFlag,
                AccountNumber = x.AccountNumber,
                ModifiedDate = x.ModifiedDate
            });

            return this.View(data);
        }
    }
}