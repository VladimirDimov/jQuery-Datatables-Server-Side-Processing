namespace JQDT.WebAPI
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http.Filters;
    using JQDT.Application;
    using JQDT.Exceptions;
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
            try
            {
                this.PerformOnActionExecuted(actionExecutedContext);
            }
            catch (Exception ex)
            {
                throw new JQDataTablesException("Unhandled JQDataTable exception", ex);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// Called when [data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        private void PerformOnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var modelType = ((System.Net.Http.ObjectContent)actionExecutedContext.Response.Content).ObjectType;
            var applicationInitizlizationFunction = ExecuteFunctionProvider<HttpActionExecutedContext>.GetAppInicializationFunc(modelType, typeof(ApplicationWebApi<>));
            var dependencyResolver = new DI.DependencyResolver();
            var webApiApplication = applicationInitizlizationFunction(actionExecutedContext, dependencyResolver);
            this.SubscribeToEvents(webApiApplication);
            var result = (ResultModel)webApiApplication.Execute();
            var formattedObjectResult = this.GetObjectResult(result);
            actionExecutedContext.Response.Content = new ObjectContent(typeof(object), formattedObjectResult, new JsonMediaTypeFormatter());
        }

        private void SubscribeToEvents(IApplicationBase application)
        {
            application.OnDataProcessed += this.OnDataProcessed;
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