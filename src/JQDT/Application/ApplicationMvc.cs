namespace JQDT.Application
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;

    internal class ApplicationMvc : ApplicationBase
    {
        private ActionExecutedContext filterContext;

        public ApplicationMvc(ActionExecutedContext filterContext)
        {
            this.filterContext = filterContext;
        }

        protected override NameValueCollection GetAjaxForm()
        {
            var ajaxForm =
                ((System.Web.HttpRequestWrapper)((System.Web.HttpContextWrapper)this.filterContext
                .RequestContext.HttpContext).Request).Form;

            return ajaxForm;
        }

        protected override IQueryable<object> GetData()
        {
            IQueryable<object> data = (IQueryable<object>)this.filterContext.Controller.ViewData.Model;

            return data;
        }
    }
}