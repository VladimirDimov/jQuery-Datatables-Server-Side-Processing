namespace Examples.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data;
    using JQDT.MVC;

    public class VendorsController : Controller
    {
        private readonly AdventureWorks context;

        public VendorsController()
        {
            this.context = new Examples.Data.AdventureWorks();
        }

        public ActionResult Index()
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