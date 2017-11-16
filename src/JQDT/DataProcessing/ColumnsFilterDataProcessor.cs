namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using JQDT.Extensions;
    using JQDT.Models;

    /// <summary>
    /// Applies filtering on separate columns
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class ColumnsFilterDataProcessor : DataProcessBase
    {
        private RequestInfoModel rquestInfoModel;

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
            var columnsWithFilter = requestInfoModel.TableParameters.Columns
                .Where(col => !string.IsNullOrEmpty(col.Search?.Value));

            if (columnsWithFilter.Count() == 0)
            {
                return data;
            }

            this.rquestInfoModel = requestInfoModel;

            var containseExpressions = new List<Expression>();
            var modelParam = Expression.Parameter(typeof(object), "m");
            foreach (var column in columnsWithFilter)
            {
                Expression expr = this.GenerateColumnContainsExpression(column, modelParam);
                containseExpressions.Add(expr);
            }

            Expression<Func<object, bool>> whereLambdaExpr = this.JoinContainsExpresions(containseExpressions, modelParam);
            var filteredData = data.Where(whereLambdaExpr);

            return filteredData;
        }

        private Expression<Func<object, bool>> JoinContainsExpresions(List<Expression> containsExpressions, ParameterExpression modelParam)
        {
            Expression joinedExpressions = null;
            foreach (var expr in containsExpressions)
            {
                joinedExpressions = joinedExpressions == null ?
                    expr :
                    Expression.And(joinedExpressions, expr);
            }

            var lambda = Expression.Lambda(joinedExpressions, modelParam);

            return (Expression<Func<object, bool>>)lambda;
        }

        private Expression GenerateColumnContainsExpression(Column column, ParameterExpression modelParam)
        {
            // res: m => (ModelType)m.Prop1.Prop2.ToString().ToLower().Contains(substring);

            // (ModelType)m
            var convertExpr = Expression.Convert((Expression)modelParam, this.rquestInfoModel.Helpers.ModelType);

            // (ModelType)m.Prop1.Prop2
            var propSelectExpr = convertExpr.NestedProperty(column.Data);

            // (ModelType)m.Prop1.Prop2.ToString()
            var toStringMethodInfo = typeof(object)
                .GetMethods()
                .Where(m => m.Name == "ToString" && !m.GetParameters().Any())
                .Single();
            var toStringExpr = Expression.Call(propSelectExpr, toStringMethodInfo);

            // (ModelType)m.Prop1.Prop2.ToString().ToLower()
            var toLowerMethodInfo = typeof(string).GetMethods()
                .Where(m => m.Name == "ToLower" && !m.GetParameters().Any())
                .Single();
            var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);

            // (ModelType)m.Prop1.Prop2.ToString().ToLower().Contains(substring)
            var containsMethodInfo = typeof(string).GetMethods()
                .Where(m => m.Name == "Contains" && m.GetParameters().Count() == 1)
                .Single();
            var searchParamExpr = Expression.Constant(column.Search.Value);
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchParamExpr);

            return containsExpr;
        }
    }
}