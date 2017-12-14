namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Delegates;
    using JQDT.Models;

    /// <summary>
    /// Data processor interface
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    public interface IDataProcess<T>
    {
        /// <summary>
        /// Occurs when [on data processing event].
        /// </summary>
        event DataProcessorEventHandler OnDataProcessingEvent;

        /// <summary>
        /// Occurs when [on data processed event].
        /// </summary>
        event DataProcessorEventHandler OnDataProcessedEvent;

        /// <summary>
        /// Gets the processed data.
        /// </summary>
        /// <value>
        /// The processed data.
        /// </value>
        IQueryable<T> ProcessedData { get; }

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns><see cref="IQueryable{object} collection of the processed data."/></returns>
        IQueryable<T> ProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel);
    }
}