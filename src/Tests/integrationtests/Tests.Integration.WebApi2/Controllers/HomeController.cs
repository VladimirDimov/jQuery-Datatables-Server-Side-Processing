namespace Tests.Integration.WebApi2.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using JQDT.WebAPI;
    using TestData.Models;

    [EnableCors]
    public class HomeController : ApiController
    {
        public static IQueryable<AllTypesModel> Data { get; set; }

        [JQDataTable]
        public IHttpActionResult Post()
        {
            return this.Ok(Data);
        }

        public IHttpActionResult GetFullData()
        {
            return this.Ok(Data.ToList());
        }
    }
}