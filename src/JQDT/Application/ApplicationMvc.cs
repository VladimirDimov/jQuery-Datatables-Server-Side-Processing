namespace JQDT.Application
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Entry point for MVC projects
    /// </summary>
    /// <seealso cref="JQDT.Application.ApplicationBase" />
    internal class ApplicationMvc : ApplicationBase
    {
        private ActionExecutedContext filterContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMvc"/> class.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public ApplicationMvc(ActionExecutedContext filterContext)
        {
            this.filterContext = filterContext;
        }

        /// <summary>
        /// Gets the ajax form as <see cref="NameValueCollection" /> from the request context.
        /// </summary>
        /// <returns>
        /// form data as <see cref="NameValueCollection" />
        /// </returns>
        protected override NameValueCollection GetAjaxForm()
        {
            var ajaxForm =
                ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)this.filterContext
                .RequestContext.HttpContext).Request).Form;

            return ajaxForm;
        }

        /// <summary>
        /// Gets the data collection as <see cref="IQueryable{object}" /> from the request context.
        /// </summary>
        /// <returns>
        /// Data collection as <see cref="IQueryable{object}" />
        /// </returns>
        protected override IQueryable<object> GetData()
        {
            IQueryable<object> data = (IQueryable<object>)this.filterContext.Controller.ViewData.Model;

            return data;
        }
    }
}