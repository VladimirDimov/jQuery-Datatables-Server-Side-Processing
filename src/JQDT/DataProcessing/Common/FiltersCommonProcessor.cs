namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.DataProcessing.FilterDataProcessor;
    using JQDT.Extensions;

    internal class FiltersCommonProcessor
    {
        private IFilterDataProcessorBridge filterDataProcessorBridge;

        public FiltersCommonProcessor(IFilterDataProcessorBridge filterDataProcessorBridge)
        {
            this.filterDataProcessorBridge = filterDataProcessorBridge;
        }

        // Returns the "Contains" expression for a single property
        public Expression GetSinglePropertyContainsExpression(string search, string propertyPath, ParameterExpression modelParamExpr)
        {
            // searchVal
            var searchValExpr = Expression.Constant(search.ToLower());
            // (TypeOfX)x
            // var convertExpr = Expression.Convert(modelParamExpr, modelType);

            // x.Name
            var propExpr = modelParamExpr.NestedProperty(propertyPath);

            // x.Name != null
            Expression nullCheckExpr = this.BuildNullCheckExpression(modelParamExpr, propertyPath);

            // x.Name.ToString()
            var toStringExpr = this.filterDataProcessorBridge.GetStringContainsExpression(propExpr);

            // x.Name.ToString().ToLower()
            var toLowerMethodInfo = typeof(string).GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
            var toLowerExpr = Expression.Call(toStringExpr, toLowerMethodInfo);

            // x.Name.ToString().Contains()
            var containsMethodInfo = typeof(string).GetMethod("Contains");
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

            var joinedExpr = nullCheckExpr == null ?
                (Expression)containsExpr :
                Expression.AndAlso(nullCheckExpr, containsExpr);

            return joinedExpr;
        }

        internal Expression BuildNullCheckExpression(ParameterExpression modelParamExpr, string propertyPath)
        {
            var nullCheckExprCollection = new List<Expression>();
            var propPathCollection = propertyPath.Split('.');
            for (int i = 1; i < propPathCollection.Length + 1; i++)
            {
                var propSelectExpr = modelParamExpr.NestedProperty(string.Join(".", propPathCollection.Take(i)));
                var propertyType = propSelectExpr.Type;
                if (!propertyType.IsClass || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    // Do not add a null check if the type is class or nullable struct;
                    continue;
                }

                var nullCheckExpr = Expression.NotEqual(propSelectExpr, Expression.Constant(null));

                nullCheckExprCollection.Add(nullCheckExpr);
            }

            var joinedAndExpr = this.GetAndExpression(nullCheckExprCollection);

            return joinedAndExpr;
        }

        public Expression GetAndExpression(List<Expression> containExpressionCollection)
        {
            if (containExpressionCollection.Count == 0)
            {
                return null;
            }

            if (containExpressionCollection.Count == 1)
            {
                return containExpressionCollection.First();
            }

            var numberOfExpressions = containExpressionCollection.Count;
            var counter = 0;
            Expression andExpr = null;
            do
            {
                andExpr = Expression.AndAlso(andExpr ?? containExpressionCollection[counter], containExpressionCollection[counter + 1]);

                counter++;
            }
            while (counter < numberOfExpressions - 1);

            return andExpr;
        }
    }
}