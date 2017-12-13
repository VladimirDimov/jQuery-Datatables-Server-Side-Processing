namespace JQDT.DI
{
    using JQDT.DataProcessing;
    using JQDT.DataProcessing.ColumnsFilterDataProcessing;
    using JQDT.DataProcessing.Common;
    using JQDT.DataProcessing.CustomFiltersDataProcessing;
    using JQDT.DataProcessing.PagingDataProcessing;
    using JQDT.DataProcessing.SearchDataProcessing;
    using JQDT.DataProcessing.SortDataProcessing;

    /// <summary>
    /// Service Locator
    /// </summary>
    /// <seealso cref="JQDT.DI.IServiceLocator" />
    public class ServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Gets the search data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of search data processor</returns>
        public IDataProcess<T> GetSearchDataProcessor<T>()
        {
            return new SearchDataProcessor<T>(
                new ContainsExpressionBuilder(
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder())));
        }

        /// <summary>
        /// Gets the custom filters data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of custom filters data processor</returns>
        public IDataProcess<T> GetCustomFiltersDataProcessor<T>()
        {
            return new CustomFiltersDataProcessor<T>(
                new RangeOrEqualsExpressionBuilder(
                    new OperationTypeValidator(),
                    new ConstantExpressionBuilder(
                        new DynamicParser()),
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder()),
                    new EqualExpressionBuilder(new AndExpressionBuilder())));
        }

        /// <summary>
        /// Gets the columns filter data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of filter by column data processor</returns>
        public IDataProcess<T> GetColumnsFilterDataProcessor<T>()
        {
            return new ColumnsFilterDataProcessor<T>(
                new ContainsExpressionBuilder(
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder())),
                new RangeOrEqualsExpressionBuilder(
                    new OperationTypeValidator(),
                    new ConstantExpressionBuilder(
                        new DynamicParser()),
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder()),
                    new EqualExpressionBuilder(new AndExpressionBuilder())));
        }

        /// <summary>
        /// Gets the sort data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of sort data processor</returns>
        public IDataProcess<T> GetSortDataProcessor<T>()
        {
            return new SortDataProcessor<T>();
        }

        /// <summary>
        /// Gets the paging data processor.
        /// </summary>
        /// <typeparam name="T">Generic data model type</typeparam>
        /// <returns>Instance of paging data processor</returns>
        public IDataProcess<T> GetPagingDataProcessor<T>()
        {
            return new PagingDataProcessor<T>();
        }
    }
}