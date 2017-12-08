namespace JQDT.WebAPI
{
    using System;
    using System.Linq;
    using System.Web.Http.Filters;
    using JQDT.Application;
    using JQDT.Models;

    public class JQDataTableAttribute : ActionFilterAttribute
    {
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

            //filterContext.Result = this.FormatResult(new
            //{
            //    draw = result.Draw,
            //    recordsTotal = result.RecordsTotal,
            //    recordsFiltered = result.RecordsFiltered,
            //    data = result.Data,
            //    error = result.Error
            //});
        }
    }
}