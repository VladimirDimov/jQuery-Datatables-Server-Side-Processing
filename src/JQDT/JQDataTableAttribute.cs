namespace JQDT
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data.ViewModels;
    using JQDT.Application;
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
            //try
            //{
                var modelType = filterContext.Controller.ViewData.Model.GetType();

                var appType = typeof(ApplicationMvc<>);
                Type[] typeArgs = { modelType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);
                object app = Activator.CreateInstance(genericAppType, filterContext);
                var methodInfo = app.GetType().GetMethod("Execute");
                var result = (ResultModel)methodInfo.Invoke(app, null);

                filterContext.Result = this.FormatResult(new
                {
                    draw = result.Draw,
                    recordsTotal = result.RecordsTotal,
                    recordsFiltered = result.RecordsFiltered,
                    data = result.Data,
                    error = result.Error
                });
            //}
            //catch (Exception ex)
            //{
            //    filterContext.HttpContext.Response.StatusCode = 500;
            //    filterContext.Result = this.FormatResult(new
            //    {
            //        Error = ex.Message
            //    });
            //}

            base.OnActionExecuted(filterContext);
        }

        private ActionResult FormatResult(object resultModel)
        {
            var jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = resultModel;

            return jsonResult;
        }
    }
}