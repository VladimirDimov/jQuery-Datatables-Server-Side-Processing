namespace JQDT.ActionFilters
{
    using JQDT.Models;

    /// <summary>
    /// jQuery Data Tables Action Filter
    /// </summary>
    public interface IJQDTActionFilter
    {
        /// <summary>
        /// Called before all data processors execute.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after all data processors execute.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnDataProcessed(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called before search data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after search data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnSearchDataProcessed(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called before custom filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnCustomFiltersDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after custom filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnCustomFiltersDataProcessed(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called before columns filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnColumnsFilterDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after columns filters data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnColumnsFilterDataProcessed(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called before sort data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnSortDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after sort data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnSortDataProcessed(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called before paging data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnPagingDataProcessing(ref object data, RequestInfoModel requestInfoModel);

        /// <summary>
        /// Called after paging data processor executes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        void OnPagingDataProcessed(ref object data, RequestInfoModel requestInfoModel);
    }
}