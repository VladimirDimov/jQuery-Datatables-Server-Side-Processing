namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Data processor interface
    /// </summary>
    internal interface IDataProcess
    {
        /// <summary>
        /// Gets the processed data.
        /// </summary>
        /// <value>
        /// The processed data.
        /// </value>
        IQueryable<object> ProcessedData { get; }

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns><see cref="IQueryable{object} collection of the processed data."/></returns>
        IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel);
    }
}