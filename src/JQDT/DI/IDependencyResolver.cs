namespace JQDT.DI
{
    using JQDT.DataProcessing;

    /// <summary>
    /// Dependency Resolver
    /// </summary>
    /// <seealso cref="JQDT.DI.IDependencyResolver" />
    internal interface IDependencyResolver
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
    }
}