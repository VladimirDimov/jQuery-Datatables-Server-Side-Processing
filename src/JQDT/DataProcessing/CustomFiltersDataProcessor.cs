namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.Common;
    using JQDT.Extensions;
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

        private static HashSet<Type> comparissonOperatorsSupportedTypes;

        private readonly Common.FiltersCommonProcessor filterCommonProcessor;
        private readonly DynamicParser dynamicParser;

        private RequestInfoModel requestInfoModel;

        /// <summary>
        /// Initializes static members of the <see cref="CustomFiltersDataProcessor{T}"/> class.
        /// </summary>
        static CustomFiltersDataProcessor()
        {
            comparissonOperatorsSupportedTypes = new HashSet<Type>()
            {
                typeof(int), typeof(double), typeof(byte), typeof(long), typeof(DateTime), typeof(DateTimeOffset), typeof(char)
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFiltersDataProcessor{T}"/> class.
        /// </summary>
        /// <param name="filterCommonProcessor">The filter common processor.</param>
        /// <param name="dynamicParser">The dynamic parser.</param>
        internal CustomFiltersDataProcessor(FiltersCommonProcessor filterCommonProcessor, DynamicParser dynamicParser)
        {
            this.filterCommonProcessor = filterCommonProcessor;
            this.dynamicParser = dynamicParser;
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

                    var expressionPredicate = this.GetCustomFilterExpressionProdicate(columnFilters.Key, filter);
                    processedData = processedData.Where(expressionPredicate);
                }
            }

            return processedData;
        }

        private Expression<Func<T, bool>> GetCustomFilterExpressionProdicate(string key, FilterModel filter)
        {
            switch (filter.Type)
            {
                case FilterTypes.gte:
                case FilterTypes.gt:
                case FilterTypes.lt:
                case FilterTypes.lte:
                case FilterTypes.eq:
                    return this.GetRangeExpression(key, filter, filter.Type);

                default:
                    throw new NotImplementedException(string.Format(InvalidCustomOperatorException, filter.Type));
            }
        }

        private Expression<Func<T, bool>> GetRangeExpression(string propertyPath, FilterModel filter, FilterTypes filterType)
        {
            // x
            var propertyInfoPath = this.requestInfoModel.Helpers.ModelType.GetPropertyInfoPath(propertyPath);
            var propertyType = propertyInfoPath.Last().PropertyType;
            this.ValidatePropertyType(propertyPath, propertyType, filterType);
            var xExpr = Expression.Parameter(typeof(T), "x");

            // x.Property1.Property2
            var propertyExpr = xExpr.NestedProperty(propertyPath);

            // Convert(value)
            Expression constantExpr = this.BuildConstantExpression(filter.Value, propertyType);

            BinaryExpression rangeExpr = null;
            switch (filter.Type)
            {
                case FilterTypes.gte:
                    // x >= Convert(value)
                    rangeExpr = Expression.GreaterThanOrEqual(propertyExpr, constantExpr);
                    break;

                case FilterTypes.gt:
                    // x > Convert(value)
                    rangeExpr = Expression.GreaterThan(propertyExpr, constantExpr);
                    break;

                case FilterTypes.lt:
                    // x < Convert(value)
                    rangeExpr = Expression.LessThan(propertyExpr, constantExpr);
                    break;

                case FilterTypes.lte:
                    // x <= Convert(value)
                    rangeExpr = Expression.LessThanOrEqual(propertyExpr, constantExpr);
                    break;

                case FilterTypes.eq:
                    rangeExpr = Expression.Equal(propertyExpr, constantExpr);
                    break;
            }

            var nullCheckExpr = this.filterCommonProcessor.BuildNullCheckExpression(xExpr, string.Join(".", propertyPath));

            Expression joinedExpr = nullCheckExpr == null ? (Expression)rangeExpr : Expression.AndAlso(nullCheckExpr, rangeExpr);

            return (Expression<Func<T, bool>>)Expression.Lambda(joinedExpr, xExpr);
        }

        private Expression BuildConstantExpression(string value, Type propertyType)
        {
            var parsedValue = this.dynamicParser.DynamicParse(value, propertyType);
            var constant = Expression.Constant(parsedValue);
            var constantCast = Expression.Convert(constant, propertyType);

            return constantCast;
        }

        private void ValidatePropertyType(string propertyPath, Type propertyType, FilterTypes filterType)
        {
            if (!comparissonOperatorsSupportedTypes.Contains(propertyType))
            {
                throw new ArgumentException(string.Format(InvalidPropertyTypeForRequestedFilterType, propertyPath, propertyType.Name, filterType.ToString(), string.Join(", ", comparissonOperatorsSupportedTypes.Select(x => x.Name))));
            }
        }
    }
}