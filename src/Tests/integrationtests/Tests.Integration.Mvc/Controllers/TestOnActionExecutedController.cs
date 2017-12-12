namespace Tests.Integration.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using JQDT.Models;

    public class TestOnActionExecutedController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [CustomOnActionExecuted]
        public ActionResult GetData()
        {
            var data = new List<OnActionExecutedTestModel>();
            for (int i = 1; i <= 5; i++)
            {
                data.Add(new OnActionExecutedTestModel { Number = i });
            }

            return this.View(data.AsQueryable());
        }
    }

    public class OnActionExecutedTestModel
    {
        public int Number { get; set; }
    }

    public class CustomOnActionExecutedAttribute : JQDT.MVC.JQDataTableAttribute
    {
        public override void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            var list = ((IQueryable<OnActionExecutedTestModel>)data).ToList();
            for (int i = 6; i <= 10; i++)
            {
                list.Add(new OnActionExecutedTestModel { Number = i });
            }

            data = list.AsQueryable();
        }
    }
}