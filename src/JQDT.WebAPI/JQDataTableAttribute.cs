namespace JQDT.WebAPI
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http.Filters;
    using JQDT.ActionFilters;
    using JQDT.Application;
    using JQDT.Exceptions;
    using JQDT.Models;

    /// <summary>
    /// Used to decorate the action that returns data table response
    /// </summary>
    /// <seealso cref="JQDT.ActionFilters.IJQDTActionFilter" />
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    public class JQDataTableAttribute : ActionFilterAttribute, IJQDTActionFilter
    {
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        /// <exception cref="JQDataTablesException">Unhandled JQDataTable exception</exception>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                this.PerformOnActionExecuted(actionExecutedContext);
            }
            catch (Exception ex)
            {
                throw new JQDataTablesException("Unhandled JQDataTable exception", ex);
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// Called when [data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called before all data processors execute.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called before search data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called after search data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnSearchDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called before custom filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnCustomFiltersDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called after custom filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnCustomFiltersDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called before columns filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnColumnsFilterDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called after columns filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnColumnsFilterDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called before sort data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnSortDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called after sort data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnSortDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called before paging data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnPagingDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        /// <summary>
        /// Called after paging data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public void OnPagingDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
        }

        private void PerformOnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var modelType = ((System.Net.Http.ObjectContent)actionExecutedContext.Response.Content).ObjectType;
            var applicationInitizlizationFunction = ExecuteFunctionProvider<HttpActionExecutedContext>.GetAppInicializationFunc(modelType, typeof(ApplicationWebApi<>));
            var serviceLocator = new DI.ServiceLocator();
            var formModelBinder = serviceLocator.GetFormModelBinder();
            var webApiApplication = applicationInitizlizationFunction(actionExecutedContext, serviceLocator, formModelBinder);
            this.SubscribeToEvents(webApiApplication);
            var result = (ResultModel)webApiApplication.Execute();
            var formattedObjectResult = this.GetObjectResult(result);
            actionExecutedContext.Response.Content = new ObjectContent(typeof(object), formattedObjectResult, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        /// <param name="application">The application.</param>
        private void SubscribeToEvents(IApplicationBase application)
        {
            application.OnDataProcessingEvent += this.OnDataProcessing;
            application.OnDataProcessedEvent += this.OnDataProcessed;

            application.OnSearchDataProcessingEvent += this.OnSearchDataProcessing;
            application.OnSearchDataProcessedEvent += this.OnSearchDataProcessed;

            application.OnCustomFiltersDataProcessingEvent += this.OnCustomFiltersDataProcessing;
            application.OnCustomFiltersDataProcessedEvent += this.OnCustomFiltersDataProcessed;

            application.OnColumnsFilterDataProcessingEvent += this.OnColumnsFilterDataProcessing;
            application.OnColumnsFilterDataProcessedEvent += this.OnColumnsFilterDataProcessed;

            application.OnSortDataProcessingEvent += this.OnSortDataProcessing;
            application.OnSortDataProcessedEvent += this.OnSortDataProcessed;

            application.OnPagingDataProcessingEvent += this.OnPagingDataProcessing;
            application.OnPagingDataProcessedEvent += this.OnPagingDataProcessed;
        }

        private object GetObjectResult(ResultModel result)
        {
            return new
            {
                draw = result.Draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data,
                error = result.Error
            };
        }
    }
}