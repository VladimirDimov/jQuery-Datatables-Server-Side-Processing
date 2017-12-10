namespace JQDT.WebAPI
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
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
            var requestFormStr = this.GetBodyFromRequest(this.actionExecutedContext);
            var formDict = HttpUtility.ParseQueryString(requestFormStr);

            return formDict;
        }

        protected override IQueryable<T> GetData()
        {
            var objectContent = this.actionExecutedContext.Response.Content as ObjectContent;
            var resultConverted = objectContent.Value as IQueryable<T>;

            return resultConverted;
        }

        private string GetBodyFromRequest(HttpActionExecutedContext context)
        {
            string data;
            using (var stream = context.Request.Content.ReadAsStreamAsync().Result)
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }
                data = context.Request.Content.ReadAsStringAsync().Result;
            }
            return data;
        }
    }
}