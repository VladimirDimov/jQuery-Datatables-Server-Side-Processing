namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class FilterDataProcessor : IDataProcess
    {
        public IQueryable<object> ProcessedData { get; set; }

        public IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            if (string.IsNullOrWhiteSpace(requestInfoModel.TableParameters.search.value))
            {
                return data.Select(x => x);
            }

            var expr = BuildExpression(data.GetType().GetGenericArguments().First(), requestInfoModel.TableParameters.search.value);
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
                var currentPropertyContainsExpression = GetSinglePropertyExpression(modelType, search, propName, modelParamExpr);
                containExpressionCollection.Add(currentPropertyContainsExpression);
            }

            Expression orExpr = GetOrExpr(containExpressionCollection);

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
            } while (counter < numberOfExpressions - 1);

            return orExpr;
        }

        // Returns the "Contains" expression for a single property
        private static MethodCallExpression GetSinglePropertyExpression(Type modelType, string search, string propName, ParameterExpression modelParamExpr)
        {
            // searchVal
            var searchValExpr = Expression.Constant(search.ToLower());
            // (TypeOfX)x
            var convertExpr = Expression.Convert(modelParamExpr, modelType);
            // x.Name
            var propExpr = Expression.Property(convertExpr, propName);
            // x.Name.ToString()
            var toStringMethodInfo = typeof(Object).GetMethod("ToString");
            var toStringExpr = Expression.Call(propExpr, toStringMethodInfo);
            // x.Name.ToString().ToLower()
            var toLowerMethodInfo = typeof(String).GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
            var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);
            // x.Name.ToString().Contains()
            var containsMethodInfo = typeof(String).GetMethod("Contains");
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

            return containsExpr;
        }
    }
}