namespace JQDT.DataProcessing.ColumnsFilterDataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.Common;
    using JQDT.Enumerations;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Applies filtering on separate columns
    /// </summary>
    /// <typeparam name="T">Type of the data model.</typeparam>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase{T}" />
    /// <seealso cref="JQDT.DataProcessing.IDataFilter" />
    internal class ColumnsFilterDataProcessor<T> : DataProcessBase<T>, IDataFilter
    {
        private const string ToStringMethodName = "ToString";
        private const string ToLowerMethodName = "ToLower";
        private const string ContainsMethodName = "Contains";

        private RequestInfoModel rquestInfoModel;
        private CommonSearchProcessor commonProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsFilterDataProcessor{T}"/> class.
        /// </summary>
        /// <param name="commonProcessor">The common processor.</param>
        internal ColumnsFilterDataProcessor(CommonSearchProcessor commonProcessor)
        {
            this.commonProcessor = commonProcessor;
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
            var columnsWithFilter =
                requestInfoModel.TableParameters.Columns
                .Where(col => !string.IsNullOrEmpty(col.Search?.Value));

            if (columnsWithFilter.Count() == 0)
            {
                return data;
            }

            this.rquestInfoModel = requestInfoModel;

            var predicateExpressions = new List<Expression>();
            var modelParam = Expression.Parameter(typeof(T), "m");
            foreach (var column in columnsWithFilter)
            {
                var propExpr = modelParam.NestedProperty(column.Data);
                var propType = propExpr.Type;
                Expression currentPredicateExpr = null;
                if (propType.IsValidForOperation(OperationTypesEnum.Search))
                {
                    currentPredicateExpr = this.commonProcessor.GetSinglePropertyContainsExpression(column.Search.Value, propExpr);
                }
                else if (propType.IsValidForOperation(OperationTypesEnum.Equals))
                {
                    // Add equals expression;
                }

                predicateExpressions.Add(currentPredicateExpr);
            }

            Expression<Func<T, bool>> whereLambdaExpr = this.JoinContainsExpresions(predicateExpressions, modelParam);
            var filteredData = data.Where(whereLambdaExpr);

            return filteredData;
        }

        private Expression<Func<T, bool>> JoinContainsExpresions(List<Expression> containsExpressions, ParameterExpression modelParam)
        {
            Expression joinedExpressions = null;
            foreach (var expr in containsExpressions)
            {
                joinedExpressions = joinedExpressions == null ?
                    expr :
                    Expression.AndAlso(joinedExpressions, expr);
            }

            var lambda = Expression.Lambda(joinedExpressions, modelParam);

            return (Expression<Func<T, bool>>)lambda;
        }
    }
}