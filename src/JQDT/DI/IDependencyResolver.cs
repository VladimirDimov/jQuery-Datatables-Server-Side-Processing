namespace JQDT.DI
{
    using JQDT.DataProcessing;

    internal interface IDependencyResolver
    {
        IDataProcess<T> GetColumnsFilterDataProcessor<T>();

        IDataProcess<T> GetCustomFiltersDataProcessor<T>();

        IDataProcess<T> GetPagingDataProcessor<T>();

        IDataProcess<T> GetSearchDataProcessor<T>();

        IDataProcess<T> GetSortDataProcessor<T>();
    }
}