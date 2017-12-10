namespace JQDT.WebAPI
{
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http.Filters;
    using JQDT.Application;
    using JQDT.Models;

    /// <summary>
    /// Used to decorate the action that returns data table response
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    public class JQDataTableAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            this.PerformOnActionExecuted(actionExecutedContext);

            base.OnActionExecuted(actionExecutedContext);
        }

        private void PerformOnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var modelType = ((System.Net.Http.ObjectContent)actionExecutedContext.Response.Content).ObjectType;
            var applicationExecuteFunction = ExecuteFunctionProvider<HttpActionExecutedContext>.GetExecuteFunction(modelType, typeof(ApplicationWebApi<>));
            var dependencyResolver = new DI.DependencyResolver();
            var result = (ResultModel)applicationExecuteFunction(actionExecutedContext, dependencyResolver);
            var formattedObjectResult = this.GetObjectResult(result);
            actionExecutedContext.Response.Content = new ObjectContent(typeof(object), formattedObjectResult, new JsonMediaTypeFormatter());
        }

        private object GetObjectResult(ResultModel result)
        {
            return new
            {
                draw = result.Draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data,
                error = result.Error
            };
        }
    }
}