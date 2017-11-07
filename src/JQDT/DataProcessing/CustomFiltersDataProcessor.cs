namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal class CustomFiltersDataProcessor : DataProcessBase
    {
        private RequestInfoModel requestInfoModel;

        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
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
            var propertyType = requestInfoModel.Helpers.ModelType.GetProperty(key).PropertyType;
            var xExpr = Expression.Parameter(typeof(object), "x");
            // (Type)x
            var castedXExpr = Expression.Convert(xExpr, this.requestInfoModel.Helpers.ModelType);
            // ((Type)x).Property
            var propertyExpr = Expression.Property(castedXExpr, key);
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
                    rangeExpr = Expression.GreaterThan(propertyExpr, parseExpr);
                    break;

                case FilterTypes.lt:
                    rangeExpr = Expression.LessThan(propertyExpr, parseExpr);
                    break;

                case FilterTypes.lte:
                    rangeExpr = Expression.LessThanOrEqual(propertyExpr, parseExpr);
                    break;

                default:
                    throw new NotImplementedException();
            }

            var tryBlock = Expression.Block(typeof(Boolean), rangeExpr);
            var throwExpr = Expression.Throw(
            Expression.Constant(
                new FormatException($"Unable to parse value for property \"{key}\". Value: {filter.Value}")),
            typeof(Boolean));
            var catchBlock = Expression.Catch(typeof(FormatException), throwExpr);
            var tryCatchExpr = Expression.TryCatch(tryBlock, catchBlock);

            return (Expression<Func<object, bool>>)Expression.Lambda(tryCatchExpr, xExpr);
        }
    }
}