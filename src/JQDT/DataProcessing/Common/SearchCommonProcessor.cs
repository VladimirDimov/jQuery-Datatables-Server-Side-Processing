namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Enumerations;
    using JQDT.Extensions;

    /// <summary>
    /// Common logic for the filter processors
    /// </summary>
    internal class SearchCommonProcessor
    {
        /// <summary>
        /// Gets the single property contains expression.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="modelParamExpr">The model parameter expr.</param>
        /// <returns>Returns the "Contains" expression for a single property</returns>
        internal Expression GetSinglePropertyContainsExpression(string search, string propertyPath, ParameterExpression modelParamExpr)
        {
            // searchVal
            var searchValExpr = Expression.Constant(search.ToLower());

            // x.Prop1.Prop2
            var propExpr = modelParamExpr.NestedProperty(propertyPath);

            // Validate that the property type is valid for a search operation
            var propertyType = propExpr.Type;
            if (!propertyType.IsValidForOperation(OperationTypesEnum.Search))
            {
                throw new ArgumentException($"Invalid search operation on type {propertyType}. A search operation can be performed only on string properties.");
            }

            // x.Prop1 != null && x.Prop1.Prop2 != null
            Expression nullCheckExpr = Expression.NotEqual(propExpr, Expression.Constant(null));

            // x.Prop1.Prop2.ToLower()
            var toLowerMethodInfo = typeof(string).GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
            var toLowerExpr = Expression.Call(propExpr, toLowerMethodInfo);

            // x.Prop1.Prop2.Contains(searchVal)
            var containsMethodInfo = typeof(string).GetMethod("Contains");
            var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

            // join the null check expressions and the string.Contains() expression with a AND clause
            // If no null check expressions (when the property cannot be null) only the string.Contains() expression is used
            var joinedExpr = nullCheckExpr == null ?
                (Expression)containsExpr :
                Expression.AndAlso(nullCheckExpr, containsExpr);

            return joinedExpr;
        }

        /// <summary>
        /// Builds the null check expression.
        /// </summary>
        /// <param name="modelParamExpr">The model parameter expr.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns><see cref="Expression"/></returns>
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

        /// <summary>
        /// Joins the expressions inside a collection of <see cref="Expression"/> with AND clause.
        /// </summary>
        /// <param name="containExpressionCollection">The contain expression collection.</param>
        /// <returns>Joined <see cref="Expression"/></returns>
        internal Expression GetAndExpression(List<Expression> containExpressionCollection)
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