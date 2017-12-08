namespace JQDT.WebAPI
{
    using System.Collections.Specialized;
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
            throw new System.NotImplementedException();
        }

        protected override IQueryable<T> GetData()
        {
            throw new System.NotImplementedException();
        }
    }
}