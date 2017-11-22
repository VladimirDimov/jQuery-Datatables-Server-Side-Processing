namespace JQDT.DataProcessing.CustomFiltersDataProcessing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.Common;
    using JQDT.Models;

    /// <summary>
    /// Filters the data using the custom filters.
    /// </summary>
    /// <typeparam name="T">Generic type of the data model.</typeparam>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase{T}" />
    /// <seealso cref="JQDT.DataProcessing.IDataFilter" />
    internal class CustomFiltersDataProcessor<T> : DataProcessBase<T>, IDataFilter
    {
        private const string InvalidPropertyTypeForRequestedFilterType = "Property {0} of type {1} is invalid for the requested filter of type {2}. It should be any of the supported types: {3}.";
        private const string InvalidCustomOperatorException = "Invalid custom operator: {0}";

        private readonly RangeOrEqualsExpressionBuilder rangeOrEqualsExpressionBuilder;
        private RequestInfoModel requestInfoModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFiltersDataProcessor{T}"/> class.
        /// </summary>
        /// <param name="filterCommonProcessor">The filter common processor.</param>
        /// <param name="dynamicParser">The dynamic parser.</param>
        internal CustomFiltersDataProcessor(RangeOrEqualsExpressionBuilder rangeOrEqualsExpressionBuilder)
        {
            this.rangeOrEqualsExpressionBuilder = rangeOrEqualsExpressionBuilder;
        }

        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{T}" />
        /// </returns>
        protected override IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            this.requestInfoModel = requestInfoModel;

            var processedData = data.Select(x => x);
            var customFilters = requestInfoModel.TableParameters.Custom.Filters;
            foreach (var columnFilters in customFilters)
            {
                foreach (var filter in columnFilters.Value)
                {
                    if (string.IsNullOrEmpty(filter.Value))
                    {
                        continue;
                    }

                    var expressionPredicate = this.GetCustomFilterExpressionPredicate(columnFilters.Key, filter);
                    processedData = processedData.Where(expressionPredicate);
                }
            }

            return processedData;
        }

        private Expression<Func<T, bool>> GetCustomFilterExpressionPredicate(string key, FilterModel filter)
        {
            switch (filter.Type)
            {
                case FilterTypes.gte:
                case FilterTypes.gt:
                case FilterTypes.lt:
                case FilterTypes.lte:
                case FilterTypes.eq:
                    return this.rangeOrEqualsExpressionBuilder.GetRangeOrEqualsExpression<T>(key, filter);

                default:
                    throw new NotImplementedException(string.Format(InvalidCustomOperatorException, filter.Type));
            }
        }
    }
}