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
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class CustomFiltersDataProcessor<T> : DataProcessBase<T>, IDataFilter
    {
        private const string InvalidPropertyTypeForRequestedFilterType = "Property {0} of type {1} is invalid for the requested filter of type {2}. It should be any of the supported types: {3}.";
        private const string InvalidCustomOperatorException = "Invalid custom operator: {0}";

        private static HashSet<Type> comparissonOperatorsSupportedTypes;

        private RequestInfoModel requestInfoModel;
        private readonly Common.FiltersCommonProcessor filterCommonProcessor;

        public CustomFiltersDataProcessor(FiltersCommonProcessor filterCommonProcessor)
        {
            this.filterCommonProcessor = filterCommonProcessor;
        }

        static CustomFiltersDataProcessor()
        {
            comparissonOperatorsSupportedTypes = new HashSet<Type>()
            {
                typeof(int), typeof(double), typeof(byte), typeof(long), typeof(DateTime), typeof(DateTimeOffset), typeof(char)
            };
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

            // ((Type)x).Property
            var propertyExpr = xExpr.NestedProperty(propertyPath);

            // Type.Parse(value)
            var valueExpr = Expression.Constant(filter.Value);
            var gteMethodInfo = propertyType.GetMethods().First(x => x.Name == "Parse");
            var parseExpr = Expression.Call(null, gteMethodInfo, valueExpr);
            var parsedValue = new DynamicParser().DynamicParse(filter.Value, propertyType);
            var constant = Expression.Constant(parsedValue);
            var constantCast = Expression.Convert(constant, propertyType);

            BinaryExpression rangeExpr = null;
            switch (filter.Type)
            {
                case FilterTypes.gte:
                    // x >= (Type)value
                    rangeExpr = Expression.GreaterThanOrEqual(propertyExpr, /*parseExpr*/constantCast);
                    break;

                case FilterTypes.gt:
                    // x > (Type)value
                    rangeExpr = Expression.GreaterThan(propertyExpr, /*parseExpr*/constantCast);
                    break;

                case FilterTypes.lt:
                    // x < (Type)value
                    rangeExpr = Expression.LessThan(propertyExpr, /*parseExpr*/constantCast);
                    break;

                case FilterTypes.lte:
                    // x <= (Type)value
                    rangeExpr = Expression.LessThanOrEqual(propertyExpr, /*parseExpr*/constantCast);
                    break;
            }

            var nullCheckExpr = this.filterCommonProcessor.BuildNullCheckExpression(xExpr, string.Join(".", propertyPath));

            Expression joinedExpr = nullCheckExpr == null ? (Expression)rangeExpr : Expression.AndAlso(nullCheckExpr, rangeExpr);

            return (Expression<Func<T, bool>>)Expression.Lambda(joinedExpr, xExpr);
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