namespace JQDT.MVC
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
                if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpNotFoundResult();
                    return;
                }

                this.PerformOnActionExecuted(filterContext);
            }
            catch (Exception ex)
            {
                throw new JQDataTablesException("Unhandled JQDataTable exception", ex);
            }
        }

        private void PerformOnActionExecuted(ActionExecutedContext filterContext)
        {
            var dataCollectionType = filterContext.Controller.ViewData.Model.GetType();
            var dependencyResolver = new DI.DependencyResolver();
            var applicationInitizlizationFunction = ExecuteFunctionProvider<ActionExecutedContext>.GetAppInicializationFunc(dataCollectionType, typeof(ApplicationMvc<>));
            var mvcApplication = applicationInitizlizationFunction(filterContext, dependencyResolver);
            var result = (ResultModel)mvcApplication.Execute();

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