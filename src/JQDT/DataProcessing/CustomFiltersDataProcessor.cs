﻿namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Filters the data using the custom filters.
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class CustomFiltersDataProcessor : DataProcessBase
    {
        private const string InvalidPropertyTypeForRequestedFilterType = "Property {0} of type {1} is invalid for the requested filter of type {2}. It should be any of the supported types: {3}.";

        private static HashSet<Type> comparissonOperatorsSupportedTypes;

        private RequestInfoModel requestInfoModel;

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
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        protected override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
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

        private Expression<Func<object, bool>> GetCustomFilterExpressionProdicate(string key, FilterModel filter)
        {
            switch (filter.Type)
            {
                case FilterTypes.gte:
                case FilterTypes.gt:
                case FilterTypes.lt:
                case FilterTypes.lte:
                    return this.GetRangeExpression(key, filter, filter.Type);

                default:
                    throw new NotImplementedException();
            }
        }

        private Expression<Func<object, bool>> GetRangeExpression(string key, FilterModel filter, FilterTypes filterType)
        {
            // x
            var propertyInfoPath = this.requestInfoModel.Helpers.ModelType.GetPropertyInfoPath(key);
            var propertyType = propertyInfoPath.Last().PropertyType;
            this.ValidatePropertyType(key, propertyType, filterType);
            var xExpr = Expression.Parameter(typeof(object), "x");

            // (Type)x
            var castedXExpr = Expression.Convert(xExpr, this.requestInfoModel.Helpers.ModelType);

            // ((Type)x).Property
            var propertyExpr = castedXExpr.NestedProperty(key);

            // Type.Parse(value)
            var valueExpr = Expression.Constant(filter.Value);
            var gteMethodInfo = propertyType.GetMethods().First(x => x.Name == "Parse");
            var parseExpr = Expression.Call(null, gteMethodInfo, valueExpr);

            BinaryExpression rangeExpr = null;
            switch (filter.Type)
            {
                case FilterTypes.gte:
                    // x >= (Type)value
                    rangeExpr = Expression.GreaterThanOrEqual(propertyExpr, parseExpr);
                    break;

                case FilterTypes.gt:
                    // x > (Type)value
                    rangeExpr = Expression.GreaterThan(propertyExpr, parseExpr);
                    break;

                case FilterTypes.lt:
                    // x < (Type)value
                    rangeExpr = Expression.LessThan(propertyExpr, parseExpr);
                    break;

                case FilterTypes.lte:
                    // x <= (Type)value
                    rangeExpr = Expression.LessThanOrEqual(propertyExpr, parseExpr);
                    break;

                default:
                    throw new NotImplementedException();
            }

            var tryBlock = Expression.Block(typeof(bool), rangeExpr);
            var throwExpr = Expression.Throw(
            Expression.Constant(
                new FormatException($"Unable to parse value for property \"{key}\". Value: {filter.Value}")),
            typeof(bool));
            var catchBlock = Expression.Catch(typeof(FormatException), throwExpr);
            var tryCatchExpr = Expression.TryCatch(tryBlock, catchBlock);

            return (Expression<Func<object, bool>>)Expression.Lambda(tryCatchExpr, xExpr);
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