using System.Linq;
using System.Web.Mvc;
using Examples.Data;
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
            var d = this.context.People as IQueryable<object>;
            var f = d.Where(x => ((Person)d).FirstName != null && ((Person)d).FirstName.Contains("z")).ToList();

            var people = this.context.People.Select(x => new PersonViewModel
            {
                Title = x.Title ?? "",
                FirstName = x.FirstName ?? "",
                MiddleName = x.MiddleName ?? "",
                LastName = x.LastName ?? ""
            }).AsQueryable().OrderBy(x => x.Title);

            return this.View(people);
        }
    }

    public class PersonViewModel
    {
        public string Title { get; set; }
        public string FirstName { get; internal set; }
        public string MiddleName { get; internal set; }
        public string LastName { get; internal set; }
    }
}