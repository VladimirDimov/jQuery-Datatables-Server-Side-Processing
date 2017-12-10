namespace JQDT.WebAPI
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Filters;
    using JQDT.Application;
    using JQDT.DI;

    /// <summary>
    /// Entry point for Web API 2 projects
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    /// <seealso cref="JQDT.Application.ApplicationBase{T}" />
    internal class ApplicationWebApi<T> : ApplicationBase<T>
    {
        private readonly HttpActionExecutedContext actionExecutedContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationWebApi{T}"/> class.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public ApplicationWebApi(HttpActionExecutedContext actionExecutedContext, IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.actionExecutedContext = actionExecutedContext;
        }

        /// <summary>
        /// Gets the ajax form as <see cref="T:System.Collections.Specialized.NameValueCollection" /> from the request context.
        /// </summary>
        /// <returns>
        /// form data as <see cref="T:System.Collections.Specialized.NameValueCollection" />
        /// </returns>
        protected override NameValueCollection GetAjaxForm()
        {
            var requestFormStr = this.GetBodyFromRequest(this.actionExecutedContext);
            var formDict = HttpUtility.ParseQueryString(requestFormStr);

            return formDict;
        }

        /// <summary>
        /// Gets the data collection as <see cref="T:System.Linq.IQueryable`1" /> from the request context.
        /// </summary>
        /// <returns>
        /// Data collection as <see cref="T:System.Linq.IQueryable`1" />
        /// </returns>
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