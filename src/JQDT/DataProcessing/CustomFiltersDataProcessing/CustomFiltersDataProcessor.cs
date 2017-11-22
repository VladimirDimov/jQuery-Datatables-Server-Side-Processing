namespace JQDT.DataProcessing.CustomFiltersDataProcessing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.Common;
    using JQDT.Enumerations;
    using JQDT.Exceptions;
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

        private readonly Common.SearchCommonProcessor filterCommonProcessor;
        private readonly DynamicParser dynamicParser;

        private RequestInfoModel requestInfoModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFiltersDataProcessor{T}"/> class.
        /// </summary>
        /// <param name="filterCommonProcessor">The filter common processor.</param>
        /// <param name="dynamicParser">The dynamic parser.</param>
        internal CustomFiltersDataProcessor(SearchCommonProcessor filterCommonProcessor, DynamicParser dynamicParser)
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
                    return this.GetRangeOrEqualsExpression(key, filter);

                default:
                    throw new NotImplementedException(string.Format(InvalidCustomOperatorException, filter.Type));
            }
        }

        // TODO: Check the case when nullable type property is null
        private Expression<Func<T, bool>> GetRangeOrEqualsExpression(string propertyPath, FilterModel filter)
        {
            // x
            var propertyInfoPath = this.requestInfoModel.Helpers.ModelType.GetPropertyInfoPath(propertyPath);
            var propertyType = propertyInfoPath.Last().PropertyType;
            this.ValidatePropertyType(propertyPath, propertyType, filter.Type);
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

                // x == value
                case FilterTypes.eq:
                    rangeExpr = Expression.Equal(propertyExpr, constantExpr);
                    break;
            }

            var nullCheckExpr = this.filterCommonProcessor.BuildNullCheckExpression(xExpr, string.Join(".", propertyPath));

            Expression joinedExpr = nullCheckExpr == null ? (Expression)rangeExpr : Expression.AndAlso(nullCheckExpr, rangeExpr);

            return (Expression<Func<T, bool>>)Expression.Lambda(joinedExpr, xExpr);
        }

        /// <summary>
        /// Dynamically parses the value to the provided type and builds the constant expression.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns><see cref="ConstantExpression"/> of the provided value and type</returns>
        private Expression BuildConstantExpression(string value, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Constant(value, typeof(string));
            }

            var parsedValue = this.dynamicParser.DynamicParse(value, propertyType);
            var constant = Expression.Constant(parsedValue);
            var constantCast = Expression.Convert(constant, propertyType);

            return constantCast;
        }

        /// <summary>
        /// Validates the type of the property for operation type.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <exception cref="JQDT.Exceptions.InvalidTypeForOperationException">Throw when property type is invalid for the requested operation.</exception>
        private void ValidatePropertyType(string propertyPath, Type propertyType, FilterTypes filterType)
        {
            bool isValidForOperation = true;
            var operationType = this.GetOperationType(filterType);

            isValidForOperation = propertyType.IsValidForOperation(operationType);

            if (!isValidForOperation)
            {
                throw new InvalidTypeForOperationException(propertyType, operationType);
            }
        }

        /// <summary>
        /// Gets the type of the operation based on provided <see cref="FilterTypes"/>.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns>Corresponding <see cref="OperationTypesEnum"/></returns>
        private OperationTypesEnum GetOperationType(FilterTypes filterType)
        {
            switch (filterType)
            {
                case FilterTypes.gte:
                case FilterTypes.gt:
                case FilterTypes.lt:
                case FilterTypes.lte:
                    return OperationTypesEnum.Range;

                case FilterTypes.eq:
                    return OperationTypesEnum.Equals;

                default:
                    return OperationTypesEnum.Default;
            }
        }
    }
}