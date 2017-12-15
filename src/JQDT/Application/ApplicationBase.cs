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
        private readonly IServiceLocator serviceLocator;
        private readonly IFormModelBinder modelBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase{T}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="modelBinder">The model binder.</param>
        public ApplicationBase(IServiceLocator serviceLocator, IFormModelBinder modelBinder)
        {
            this.serviceLocator = serviceLocator;
            this.modelBinder = modelBinder;
        }

        /// <summary>
        /// Occurs before all data processors execute
        /// </summary>
        public event DataProcessorEventHandler OnDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after all data processors execute
        /// </summary>
        public event DataProcessorEventHandler OnDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs before search data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnSearchDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after search data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnSearchDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs before custom filters data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnCustomFiltersDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after custom filters data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnCustomFiltersDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs before column filters data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnColumnsFilterDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after column filters data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnColumnsFilterDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs before sort data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnSortDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after sort data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnSortDataProcessedEvent = delegate { };

        /// <summary>
        /// Occurs before paging data processor executes
        /// </summary>
        public event DataProcessorEventHandler OnPagingDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs after paging data processor executes
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
                var ajaxForm = this.GetAjaxForm();
                var data = this.GetData();
                var requestModel = this.modelBinder.BindModel(ajaxForm, data);

                var dataProcessChain = this.GetDataProcessChain(typeof(T));

                // Call events before the data is processed
                this.PerformDataProcessorEventHandler(ref data, requestModel, this.OnDataProcessingEvent);

                var processedData = dataProcessChain.ProcessData(data, requestModel);

                // Call events after the data is processed
                this.PerformDataProcessorEventHandler(ref processedData, requestModel, this.OnDataProcessedEvent);

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
        /// <param name="eventHandler">The event handler.</param>
        private void PerformDataProcessorEventHandler(ref IQueryable<T> data, RequestInfoModel requestInfoModel, DataProcessorEventHandler eventHandler)
        {
            var dataAsObj = (object)data;
            eventHandler(ref dataAsObj, requestInfoModel);

            // Assert that the data type remains proper inside the event subscriber function.
            if (!typeof(IQueryable<T>).IsAssignableFrom(dataAsObj.GetType()))
            {
                throw new ArgumentException($"Inappropriate data collection type inside event subscriber function. The data collection type must be IQueryable<>.");
            }

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

            var searchDataProcessor = this.serviceLocator.GetSearchDataProcessor<T>();
            searchDataProcessor.OnDataProcessingEvent += this.OnSearchDataProcessingEvent;
            searchDataProcessor.OnDataProcessedEvent += this.OnSearchDataProcessedEvent;
            dataProcessChain.AddDataProcessor(searchDataProcessor);

            var customFiltersDataProcessor = this.serviceLocator.GetCustomFiltersDataProcessor<T>();
            customFiltersDataProcessor.OnDataProcessingEvent += this.OnCustomFiltersDataProcessingEvent;
            customFiltersDataProcessor.OnDataProcessedEvent += this.OnCustomFiltersDataProcessedEvent;
            dataProcessChain.AddDataProcessor(customFiltersDataProcessor);

            var columnsFilterDataProcessor = this.serviceLocator.GetColumnsFilterDataProcessor<T>();
            columnsFilterDataProcessor.OnDataProcessingEvent += this.OnColumnsFilterDataProcessingEvent;
            columnsFilterDataProcessor.OnDataProcessedEvent += this.OnColumnsFilterDataProcessedEvent;
            dataProcessChain.AddDataProcessor(columnsFilterDataProcessor);

            var sortDataProcessor = this.serviceLocator.GetSortDataProcessor<T>();
            sortDataProcessor.OnDataProcessingEvent += this.OnSortDataProcessingEvent;
            sortDataProcessor.OnDataProcessedEvent += this.OnSortDataProcessedEvent;
            dataProcessChain.AddDataProcessor(sortDataProcessor);

            var pagingDataProcessor = this.serviceLocator.GetPagingDataProcessor<T>();
            pagingDataProcessor.OnDataProcessingEvent += this.OnPagingDataProcessingEvent;
            pagingDataProcessor.OnDataProcessedEvent += this.OnPagingDataProcessedEvent;
            dataProcessChain.AddDataProcessor(pagingDataProcessor);

            return dataProcessChain;
        }
    }
}