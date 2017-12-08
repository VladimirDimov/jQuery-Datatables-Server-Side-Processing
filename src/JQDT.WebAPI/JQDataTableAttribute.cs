namespace JQDT.WebAPI
{
    using System.Web.Http.Filters;

    public class JQDataTableAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}