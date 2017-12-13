namespace JQDT.DataProcessing
{
    using System;
    using System.Linq;
    using JQDT.Delegates;
    using JQDT.Models;

    /// <summary>
    /// Base class for data processors.
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    /// <seealso cref="JQDT.DataProcessing.IDataProcess" />
    internal abstract class DataProcessBase<T> : IDataProcess<T>
    {
        private const string NullDataExceptionMessage = "Invalid null value for data argument in data processor";
        private const string NullRequestInfoModelExceptionMessage = "Invalid null value for request info model argument in data processor.";

        private IQueryable<T> processedData;

        /// <summary>
        /// Occurs when [on data processing event].
        /// </summary>
        public event DataProcessorEventHandler OnDataProcessingEvent = delegate { };

        /// <summary>
        /// Occurs when [on data processed event].
        /// </summary>
        public event DataProcessorEventHandler OnDataProcessedEvent = delegate { };

        /// <summary>
        /// Gets the processed data.
        /// </summary>
        /// <value>
        /// The processed data.
        /// </value>
        public IQueryable<T> ProcessedData
        {
            get
            {
                return this.processedData;
            }
        }

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{T} collection of the processed data." />
        /// </returns>
        public IQueryable<T> ProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            if (data == null)
            {
                throw new ArgumentNullException(NullDataExceptionMessage);
            }

            if (requestInfoModel == null)
            {
                throw new ArgumentNullException(NullRequestInfoModelExceptionMessage);
            }

            // Execute events prior data processing
            this.ExecuteOnDataprocessingEvents(ref data, requestInfoModel);

            this.processedData = this.OnProcessData(data, requestInfoModel);

            // Execute events after the data has been processed
            this.ExecuteOnDataprocessedEvents(ref data, requestInfoModel);

            return this.ProcessedData;
        }

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns><see cref="IQueryable{T}"/></returns>
        protected abstract IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel);

        private void ExecuteOnDataprocessingEvents(ref IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            var dataAsObj = (object)data;
            this.OnDataProcessingEvent(ref dataAsObj, requestInfoModel);
            data = (IQueryable<T>)dataAsObj;
        }

        private void ExecuteOnDataprocessedEvents(ref IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            var dataAsObj = (object)data;
            this.OnDataProcessedEvent(ref dataAsObj, requestInfoModel);
            data = (IQueryable<T>)dataAsObj;
        }
    }
}