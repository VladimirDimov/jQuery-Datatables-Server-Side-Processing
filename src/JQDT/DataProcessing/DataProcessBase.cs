namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Base class for data processors.
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.IDataProcess" />
    public abstract class DataProcessBase : IDataProcess
    {
        private IQueryable<object> processedData;

        /// <summary>
        /// Gets the processed data.
        /// </summary>
        /// <value>
        /// The processed data.
        /// </value>
        public IQueryable<object> ProcessedData
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
        ///   <see cref="IQueryable{object} collection of the processed data." />
        /// </returns>
        public IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            this.processedData = this.OnProcessData(data, requestInfoModel);

            return this.ProcessedData;
        }

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns><see cref="IQueryable{object}"/></returns>
        protected abstract IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel);
    }
}