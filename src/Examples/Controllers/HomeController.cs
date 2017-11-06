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
            return this.SimpleExample();
        }

        public ActionResult SimpleExample()
        {
            return this.View(nameof(HomeController.SimpleExample));
        }

        [JQDataTable]
        public ActionResult GetData()
        {
            return this.View(peopleCollection.AsQueryable());
        }
    }
}