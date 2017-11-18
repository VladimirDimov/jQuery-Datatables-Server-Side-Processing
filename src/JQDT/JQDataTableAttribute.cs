namespace JQDT
{
    using System;
    using System.Web.Mvc;
    using JQDT.Application;

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
                var app = new ApplicationMvc<object>(filterContext);
                var result = app.Execute();

                filterContext.Result = this.FormatResult(new
                {
                    draw = result.Draw,
                    recordsTotal = result.RecordsTotal,
                    recordsFiltered = result.RecordsFiltered,
                    data = result.Data,
                    error = result.Error
                });
            }
            catch (Exception ex)
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Result = this.FormatResult(new
                {
                    Error = ex.Message
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