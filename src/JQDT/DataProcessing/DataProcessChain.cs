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
    internal class DataProcessChain : DataProcessBase, IDataProcessChain
    {
        private ICollection<IDataProcess> dataProcessors;
        private Dictionary<string, object> intermidiateResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessChain"/> class.
        /// </summary>
        public DataProcessChain()
        {
            this.dataProcessors = new LinkedList<IDataProcess>();
            this.intermidiateResults = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets collection of the containing data processors.
        /// </summary>
        /// <value>
        /// The data processors.
        /// </value>
        public IEnumerable<IDataProcess> DataProcessors
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
        public void AddDataProcessor(IDataProcess dataProcessor)
        {
            this.dataProcessors.Add(dataProcessor);
        }

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        protected override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
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