namespace JQDT.DI
{
    using JQDT.DataProcessing;
    using JQDT.ModelBinders;

    /// <summary>
    /// Service Locator Interface
    /// </summary>
    /// <seealso cref="JQDT.DI.IServiceLocator" />
    public interface IServiceLocator
    {
        /// <summary>
        /// Gets the columns filter data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of filter by column data processor</returns>
        IDataProcess<T> GetColumnsFilterDataProcessor<T>();

        /// <summary>
        /// Gets the custom filters data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of custom filters data processor</returns>
        IDataProcess<T> GetCustomFiltersDataProcessor<T>();

        /// <summary>
        /// Gets the paging data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of paging data processor</returns>
        IDataProcess<T> GetPagingDataProcessor<T>();

        /// <summary>
        /// Gets the search data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of search data processor</returns>
        IDataProcess<T> GetSearchDataProcessor<T>();

        /// <summary>
        /// Gets the sort data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of sort data processor</returns>
        IDataProcess<T> GetSortDataProcessor<T>();

        /// <summary>
        /// Gets the form model binder.
        /// </summary>
        /// <returns></returns>
        IFormModelBinder GetFormModelBinder();
    }
}