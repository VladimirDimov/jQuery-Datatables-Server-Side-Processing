namespace JQDT.MVC
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;
    using JQDT.Application;
    using JQDT.ModelBinders;

    /// <summary>
    /// Entry point for MVC projects
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    /// <seealso cref="JQDT.Application.ApplicationBase{T}" />
    internal class ApplicationMvc<T> : ApplicationBase<T>
    {
        private ActionExecutedContext filterContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationMvc{T}"/> class.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="modelBinder">The model binder.</param>
        public ApplicationMvc(ActionExecutedContext filterContext, DI.IServiceLocator serviceLocator, IFormModelBinder modelBinder)
            : base(serviceLocator, modelBinder)
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
        protected override IQueryable<T> GetData()
        {
            IQueryable<T> data = (IQueryable<T>)this.filterContext.Controller.ViewData.Model;

            return data;
        }
    }
}