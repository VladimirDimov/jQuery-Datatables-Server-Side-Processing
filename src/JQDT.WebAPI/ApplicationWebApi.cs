namespace JQDT.WebAPI
{
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Web.Http.Filters;
    using JQDT.Application;
    using JQDT.DI;

    internal class ApplicationWebApi<T> : ApplicationBase<T>
    {
        private readonly HttpActionExecutedContext actionExecutedContext;

        public ApplicationWebApi(HttpActionExecutedContext actionExecutedContext, IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.actionExecutedContext = actionExecutedContext;
        }

        protected override NameValueCollection GetAjaxForm()
        {
            var result = actionExecutedContext.Request.Properties as NameValueCollection;

            return result;
        }

        protected override IQueryable<T> GetData()
        {
            throw new System.NotImplementedException();
        }
    }
}