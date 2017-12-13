namespace JQDT.MVC
{
    using System;
    using System.Web.Mvc;
    using JQDT.Application;
    using JQDT.Exceptions;
    using JQDT.Models;

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
                if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpNotFoundResult();
                    return;
                }

                this.PerformOnActionExecuted(filterContext);
            }
            catch (Exception ex)
            {
                throw new JQDataTablesException("Unhandled JQDataTable exception", ex);
            }
        }

        /// <summary>
        /// Called after all data processors execute.
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
        public virtual void OnDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [search data processing].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [search data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnSearchDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [custom filters data processing].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnCustomFiltersDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [custom filters data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnCustomFiltersDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [columns filter data processing].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnColumnsFilterDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [columns filter data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnColumnsFilterDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [sort data processing].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnSortDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [sort data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnSortDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [paging data processing].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnPagingDataProcessing(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        /// <summary>
        /// Called when [paging data processed].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        public virtual void OnPagingDataProcessed(ref object data, RequestInfoModel requestInfoModel)
        {
            // No data processing logic by default;
        }

        private void PerformOnActionExecuted(ActionExecutedContext filterContext)
        {
            var dataCollectionType = filterContext.Controller.ViewData.Model.GetType();
            var applicationInitizlizationFunction = ExecuteFunctionProvider<ActionExecutedContext>.GetAppInicializationFunc(dataCollectionType, typeof(ApplicationMvc<>));
            var serviceLocator = new DI.ServiceLocator();
            var formModelBinder = serviceLocator.GetFormModelBinder();
            var mvcApplication = applicationInitizlizationFunction(filterContext, serviceLocator, formModelBinder);
            this.SubscribeToEvents(mvcApplication);
            var result = (ResultModel)mvcApplication.Execute();

            filterContext.Result = this.FormatResult(new
            {
                draw = result.Draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data,
                error = result.Error
            });

            base.OnActionExecuted(filterContext);
        }

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

        private ActionResult FormatResult(object resultModel)
        {
            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = resultModel
            };

            return jsonResult;
        }
    }
}