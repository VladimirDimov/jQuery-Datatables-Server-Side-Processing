﻿namespace JQDT.DataProcessing.ColumnsFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.Common;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Applies filtering on separate columns
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class ColumnsFilterDataProcessor<T> : DataProcessBase<T>, IDataFilter
    {
        private const string ToStringMethodName = "ToString";
        private const string ToLowerMethodName = "ToLower";
        private const string ContainsMethodName = "Contains";

        private RequestInfoModel rquestInfoModel;
        private FiltersCommonProcessor commonProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsFilterDataProcessor{T}"/> class.
        /// </summary>
        /// <param name="commonProcessor">The common processor.</param>
        public ColumnsFilterDataProcessor(FiltersCommonProcessor commonProcessor)
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

            var containseExpressions = new List<Expression>();
            var modelParam = Expression.Parameter(typeof(T), "m");
            foreach (var column in columnsWithFilter)
            {
                Expression expr = this.commonProcessor
                    .GetSinglePropertyContainsExpression(column.Search.Value, column.Data, modelParam);
                containseExpressions.Add(expr);
            }

            Expression<Func<T, bool>> whereLambdaExpr = this.JoinContainsExpresions(containseExpressions, modelParam);
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

        //private Expression GenerateColumnContainsExpression(Column column, ParameterExpression modelParam)
        //{
        //    // res: m => m.Prop1.Prop2.ToString().ToLower().Contains(substring);

        //    // (ModelType)m.Prop1.Prop2
        //    var propSelectExpr = this.commonProcessor.GetSinglePropertyContainsExpression(column.Search.Value, column.Data, modelParam);

        //    // (ModelType)m.Prop1.Prop2.ToString()
        //    var toStringMethodInfo = typeof(T)
        //        .GetMethods()
        //        .Where(m => m.Name == ToStringMethodName && !m.GetParameters().Any())
        //        .Single();
        //    var toStringExpr = Expression.Call(propSelectExpr, toStringMethodInfo);

        //    // (ModelType)m.Prop1.Prop2.ToString().ToLower()
        //    var toLowerMethodInfo = typeof(string).GetMethods()
        //        .Where(m => m.Name == ToLowerMethodName && !m.GetParameters().Any())
        //        .Single();
        //    var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);

        //    // (ModelType)m.Prop1.Prop2.ToString().ToLower().Contains(substring)
        //    var containsMethodInfo = typeof(string).GetMethods()
        //        .Where(m => m.Name == ContainsMethodName && m.GetParameters().Count() == 1)
        //        .Single();
        //    var searchParamExpr = Expression.Constant(column.Search.Value);
        //    var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchParamExpr);

        //    return containsExpr;
        //}
    }
}