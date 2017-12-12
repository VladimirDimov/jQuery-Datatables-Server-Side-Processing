namespace JQDT.Application
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using JQDT.DataProcessing;
    using JQDT.Delegates;
    using JQDT.DI;
    using JQDT.ModelBinders;
    using JQDT.Models;

    /// <summary>
    /// Application entry point.
    /// The <see cref="ApplicationBase.Execute(System.Collections.Specialized.NameValueCollection, System.Linq.IQueryable{T})"/> should be called
    /// </summary>
    /// <typeparam name="T">Data Collection Generic Type</typeparam>
    public abstract class ApplicationBase<T> : IApplicationBase
    {
        private readonly IDependencyResolver dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase{T}"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public ApplicationBase(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Occurs when [on data processed].
        /// </summary>
        public event DataProcessorEventHandler OnDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs when [on search data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnSearchDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on search data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnSearchDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs when [on custom filters data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnCustomFiltersDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on custom filters data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnCustomFiltersDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs when [on columns filter data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnColumnsFilterDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on columns filter data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnColumnsFilterDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs when [on sort data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnSortDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on sort data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnSortDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs when [on paging data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnPagingDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on paging data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnPagingDataProcessedEvent = delegate { };

        /// <summary>
        /// Application entry point method. Executes all data processors.
        /// </summary>
        /// <returns>
        /// Processed data as <see cref="ResultModel" />
        /// </returns>
        public ResultModel Execute()
        {
            ResultModel result = new ResultModel();
            try
            {
                var modelBinder = new FormModelBinder();
                var ajaxForm = this.GetAjaxForm();
                var data = this.GetData();
                var requestModel = modelBinder.BindModel(ajaxForm, data);

                var dataProcessChain = this.GetDataProcessChain(requestModel.Helpers.DataCollectionType);
                var processedData = dataProcessChain.ProcessData(data, requestModel);

                // Call on data processed event
                this.PerformDataProcessorEventHandler(ref processedData, requestModel);

                result.Data = processedData.ToList().Select(x => (object)x).ToList();
                result.Draw = requestModel.TableParameters.Draw;
                result.RecordsTotal = data.Count();
                result.RecordsFiltered = this.GetRecordsFiltered(dataProcessChain);
            }
            catch (Exception ex)
            {
                result.Error = this.FormatException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the ajax form as <see cref="NameValueCollection"/> from the request context.
        /// </summary>
        /// <returns>form data as <see cref="NameValueCollection"/></returns>
        protected abstract NameValueCollection GetAjaxForm();

        /// <summary>
        /// Gets the data collection as <see cref="IQueryable{T}"/> from the request context.
        /// </summary>
        /// <returns>Data collection as <see cref="IQueryable{T}"/></returns>
        protected abstract IQueryable<T> GetData();

        /// <summary>
        /// Performs the data processor event handler. Boxes the data collection, pass it by reference and then unbox it.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        private void PerformDataProcessorEventHandler(ref IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            var dataAsObj = (object)data;
            this.OnDataProcessedEvent(ref dataAsObj, requestInfoModel);
            data = (IQueryable<T>)dataAsObj;
        }

        private string FormatException(Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine(ex.Message);
            if (ex.HelpLink != null)
            {
                builder.AppendLine($"Help Link: {ex.HelpLink}");
            }

            return builder.ToString();
        }

        private int GetRecordsFiltered(IDataProcess<T> dataProcessChain)
        {
            return
                ((IDataProcessChain<T>)dataProcessChain)
                    .DataProcessors
                    .Last(p => typeof(IDataFilter).IsAssignableFrom(p.GetType()))
                    .ProcessedData.Count();
        }

        private IDataProcess<T> GetDataProcessChain(Type dataCollectionType)
        {
            var dataProcessChain = new DataProcessChain<T>();

            var searchDataProcessor = this.dependencyResolver.GetSearchDataProcessor<T>();
            ((DataProcessBase<T>)searchDataProcessor).OnDataProcessingEvent += this.OnSearchDataProcessingEvent;
            ((DataProcessBase<T>)searchDataProcessor).OnDataProcessedEvent += this.OnSearchDataProcessedEvent;
            dataProcessChain.AddDataProcessor(searchDataProcessor);

            var customFiltersDataProcessor = this.dependencyResolver.GetCustomFiltersDataProcessor<T>();
            ((DataProcessBase<T>)customFiltersDataProcessor).OnDataProcessingEvent += this.OnCustomFiltersDataProcessingEvent;
            ((DataProcessBase<T>)customFiltersDataProcessor).OnDataProcessedEvent += this.OnCustomFiltersDataProcessedEvent;
            dataProcessChain.AddDataProcessor(customFiltersDataProcessor);

            var columnsFilterDataProcessor = this.dependencyResolver.GetColumnsFilterDataProcessor<T>();
            ((DataProcessBase<T>)columnsFilterDataProcessor).OnDataProcessingEvent += this.OnColumnsFilterDataProcessingEvent;
            ((DataProcessBase<T>)columnsFilterDataProcessor).OnDataProcessedEvent += this.OnColumnsFilterDataProcessedEvent;
            dataProcessChain.AddDataProcessor(columnsFilterDataProcessor);

            var sortDataProcessor = this.dependencyResolver.GetSortDataProcessor<T>();
            ((DataProcessBase<T>)sortDataProcessor).OnDataProcessingEvent += this.OnSortDataProcessingEvent;
            ((DataProcessBase<T>)sortDataProcessor).OnDataProcessedEvent += this.OnSortDataProcessedEvent;
            dataProcessChain.AddDataProcessor(sortDataProcessor);

            var pagingDataProcessor = this.dependencyResolver.GetPagingDataProcessor<T>();
            ((DataProcessBase<T>)pagingDataProcessor).OnDataProcessingEvent += this.OnPagingDataProcessingEvent;
            ((DataProcessBase<T>)pagingDataProcessor).OnDataProcessedEvent += this.OnPagingDataProcessedEvent;
            dataProcessChain.AddDataProcessor(pagingDataProcessor);

            return dataProcessChain;
        }
    }
}