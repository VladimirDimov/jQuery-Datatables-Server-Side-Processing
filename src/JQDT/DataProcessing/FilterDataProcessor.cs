namespace JQDT.DataProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Models;

    /// <summary>
    /// Filters the data by a substring value. Looks for the substring in all public properties of the data model.
    /// </summary>
    internal class FilterDataProcessor : DataProcessBase
    {
        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            if (string.IsNullOrWhiteSpace(requestInfoModel.TableParameters.Search.Value))
            {
                return data.Select(x => x);
            }

            var expr = this.BuildExpression(data.GetType().GetGenericArguments().First(), requestInfoModel.TableParameters.Search.Value);
            data = data.Where(expr);

            return data;
        }

        private Expression<Func<dynamic, bool>> BuildExpression(Type modelType, string search)
        {
            // x
            var modelParamExpr = Expression.Parameter(typeof(object), "model");
            var properties = ((System.Reflection.TypeInfo)modelType).DeclaredProperties;
            var containExpressionCollection = new List<MethodCallExpression>();
            foreach (var property in properties)
            {
                var propName = property.Name;
                var currentPropertyContainsExpression = this.GetSinglePropertyExpression(modelType, search, propName, modelParamExpr);
                containExpressionCollection.Add(currentPropertyContainsExpression);
            }

            Expression orExpr = this.GetOrExpr(containExpressionCollection);

            var lambda = Expression.Lambda(orExpr, modelParamExpr);

            return (Expression<Func<dynamic, bool>>)lambda;
        }

        private Expression GetOrExpr(List<MethodCallExpression> containExpressionCollection)
        {
            var numberOfExpressions = containExpressionCollection.Count;
            var counter = 0;
            Expression orExpr = null;
            do
            {
                orExpr = Expression.Or(orExpr ?? containExpressionCollection[counter], containExpressionCollection[counter + 1]);

                counter++;
            }
            while (counter < numberOfExpressions - 1);

            return orExpr;
        }

        // Returns the "Contains" expression for a single property
        private MethodCallExpression GetSinglePropertyExpression(Type modelType, string search, string propName, ParameterExpression modelParamExpr)
        {
            // searchVal
            var searchValExpr = Expression.Constant(search.ToLower());

            // (TypeOfX)x
            var convertExpr = Expression.Convert(modelParamExpr, modelType);

            // x.Name
            var propExpr = Expression.Property(convertExpr, propName);

            // x.Name.ToString()
            var toStringMethodInfo = typeof(object).GetMethod("ToString");
            var toStringExpr = Expression.Call(propExpr, toStringMethodInfo);

            // x.Name.ToString().ToLower()
            var toLowerMethodInfo = typeof(string).GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
            var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);

            // x.Name.ToString().Contains()
            var containsMethodInfo = typeof(string).GetMethod("Contains");
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

            return containsExpr;
        }
    }
}