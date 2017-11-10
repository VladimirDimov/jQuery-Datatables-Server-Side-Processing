using JQDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public static List<Person> peopleCollection = new List<Person>();

        public ActionResult Index()
        {
            return this.RedirectToAction(nameof(this.SimpleExample));
        }

        public ActionResult SimpleExample()
        {
            return this.View();
        }

        public ActionResult RangeExample()
        {
            return this.View();
        }

        public ActionResult FeatureDisableExample()
        {
            return this.View();
        }

        public ActionResult EachColumnSearchExample()
        {
            return this.View();
        }

        [JQDataTable]
        public ActionResult GetData()
        {
            return this.View(peopleCollection.AsQueryable());
        }
    }
}