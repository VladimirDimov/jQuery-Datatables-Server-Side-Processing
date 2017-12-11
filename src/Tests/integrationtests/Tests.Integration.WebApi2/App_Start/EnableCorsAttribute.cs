using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Tests.Integration.WebApi2
{
    public class EnableCorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Content.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Response.Content.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuting(actionContext);
        }
    }
}