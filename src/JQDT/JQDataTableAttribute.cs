namespace JQDT
{
    using System;
    using System.Web.Mvc;
    using JQDT.Application;
    using JQDT.Exceptions;
    using JQDT.Models;

    /// <summary>
    /// Used to decorate the action that returns data table response.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false)]
    public class JQDataTableAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                this.PerformOnActionExecuted(filterContext);
            }
            catch (Exception ex)
            {
                throw new JQDataTablesException("Unhandled JQDataTable exception", ex);
            }
        }

        private void PerformOnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelType = filterContext.Controller.ViewData.Model.GetType();
            var applicationExecuteFunction = ExecuteFunctionProvider.GetExecuteFunction(modelType, typeof(ApplicationMvc<>));
            var dependencyResolver = new DI.DependencyResolver();
            var result = (ResultModel)applicationExecuteFunction(filterContext, dependencyResolver);

            filterContext.Result = this.FormatResult(new
            {
                draw = result.Draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data,
                error = result.Error
            });

            base.OnActionExecuted(filterContext);
        }

        private ActionResult FormatResult(object resultModel)
        {
            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = resultModel
            };

            return jsonResult;
        }
    }
}