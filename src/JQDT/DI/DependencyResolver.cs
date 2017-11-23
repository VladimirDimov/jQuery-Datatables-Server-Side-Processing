namespace JQDT.DI
{
    using JQDT.DataProcessing;
    using JQDT.DataProcessing.ColumnsFilterDataProcessing;
    using JQDT.DataProcessing.Common;
    using JQDT.DataProcessing.CustomFiltersDataProcessing;
    using JQDT.DataProcessing.PagingDataProcessing;
    using JQDT.DataProcessing.SearchDataProcessing;

    internal class DependencyResolver : IDependencyResolver
    {
        public IDataProcess<T> GetSearchDataProcessor<T>()
        {
            return new SearchDataProcessor<T>(
                new ContainsExpressionBuilder(
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder())));
        }

        public IDataProcess<T> GetCustomFiltersDataProcessor<T>()
        {
            return new CustomFiltersDataProcessor<T>(
                new RangeOrEqualsExpressionBuilder(
                    new OperationTypeValidator(),
                    new ConstantExpressionBuilder(
                        new DynamicParser()),
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder())));
        }

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
                        new AndExpressionBuilder())));
        }

        public IDataProcess<T> GetSortDataProcessor<T>()
        {
            return new CustomFiltersDataProcessor<T>(
                new RangeOrEqualsExpressionBuilder(
                    new OperationTypeValidator(),
                    new ConstantExpressionBuilder(
                        new DynamicParser()),
                    new NullCheckExpressionBuilder(
                        new AndExpressionBuilder())));
        }

        public IDataProcess<T> GetPagingDataProcessor<T>()
        {
            return new PagingDataProcessor<T>();
        }
    }
}