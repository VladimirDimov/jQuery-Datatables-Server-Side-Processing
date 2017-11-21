namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Data processor interface
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    internal interface IDataProcess<T>
    {
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