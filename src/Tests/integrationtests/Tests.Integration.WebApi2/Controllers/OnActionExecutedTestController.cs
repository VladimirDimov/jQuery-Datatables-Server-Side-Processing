namespace Tests.Integration.WebApi2.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using JQDT.Models;
    using TestData.Data.Models;

    public class OnActionExecutedTestController : ApiController
    {
        [CustomOnActionExecuted]
        public IHttpActionResult Post()
        {
            var data = new List<OnActionExecutedTestModel>();
            for (int i = 1; i <= 5; i++)
            {
                data.Add(new OnActionExecutedTestModel { Number = i });
            }

            return this.Ok(data.AsQueryable());
        }
    }

    public class CustomOnActionExecutedAttribute : JQDT.WebAPI.JQDataTableAttribute
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