namespace JQDT
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

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
                var ajaxForm = ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)filterContext.RequestContext.HttpContext).Request).Form;
                IQueryable<object> data = (IQueryable<object>)filterContext.Controller.ViewData.Model;

                var app = new Application();
                var result = app.Execute(ajaxForm, data);

                filterContext.Result = this.FormatResult(new
                {
                    draw = result.Draw,
                    recordsTotal = result.RecordsTotal,
                    recordsFiltered = result.RecordsFiltered,
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Result = this.FormatResult(new
                {
                    error = ex.Message
                });
            }

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