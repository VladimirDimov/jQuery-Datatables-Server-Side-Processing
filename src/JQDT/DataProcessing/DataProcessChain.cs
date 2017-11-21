namespace JQDT.DataProcessing
{
    using System.Collections.Generic;
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Chain data processor that calls all containing data processors in the added order.
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    /// <seealso cref="JQDT.DataProcessing.IDataProcessChain" />
    internal class DataProcessChain<T> : DataProcessBase<T>, IDataProcessChain<T>
    {
        private ICollection<IDataProcess<T>> dataProcessors;
        private Dictionary<string, T> intermidiateResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessChain"/> class.
        /// </summary>
        internal DataProcessChain()
        {
            this.dataProcessors = new LinkedList<IDataProcess<T>>();
            this.intermidiateResults = new Dictionary<string, T>();
        }

        /// <summary>
        /// Gets collection of the containing data processors.
        /// </summary>
        /// <value>
        /// The data processors.
        /// </value>
        public IEnumerable<IDataProcess<T>> DataProcessors
        {
            get
            {
                return this.dataProcessors;
            }
        }

        /// <summary>
        /// Adds a data processor to the execution chain.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        public void AddDataProcessor(IDataProcess<T> dataProcessor)
        {
            this.dataProcessors.Add(dataProcessor);
        }

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{T}" />
        /// </returns>
        protected override IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            var currentDataState = data;

            foreach (var dataProcessor in this.dataProcessors)
            {
                var processedData = dataProcessor.ProcessData(currentDataState, requestInfoModel);
                currentDataState = processedData;
            }

            return currentDataState;
        }
    }
}