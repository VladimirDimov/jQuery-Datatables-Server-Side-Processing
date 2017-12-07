namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Enumerations;
    using JQDT.Exceptions;
    using JQDT.Extensions;

    /// <summary>
    /// Common logic for the filter processors
    /// </summary>
    internal class ContainsExpressionBuilder
    {
        private readonly NullCheckExpressionBuilder nullCheckExpressionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsExpressionBuilder"/> class.
        /// </summary>
        /// <param name="nullCheckExpressionBuilder">The null check expression builder.</param>
        public ContainsExpressionBuilder(NullCheckExpressionBuilder nullCheckExpressionBuilder)
        {
            this.nullCheckExpressionBuilder = nullCheckExpressionBuilder;
        }

        /// <summary>
        /// Gets the single property contains expression.
        /// If property type is <see cref="string"/> generates Contains expression.
        /// If the property type is <see cref="char"/> generates Equal expression.
        /// </summary>
        /// <param name="search">The search value.</param>
        /// <param name="propExpr">The property <see cref="MemberExpression"/>.</param>
        /// <returns>Returns the "Contains" expression for a single property</returns>
        /// <exception cref="JQDT.Exceptions.InvalidTypeForOperationException">Thrown if the property type is invalid for search operation.</exception>
        internal Expression BuildExpression(string search, MemberExpression propExpr)
        {
            // Validate that the property type is valid for a search operation
            var propertyType = propExpr.Type;
            if (!propertyType.IsValidForOperation(OperationTypesEnum.Search))
            {
                throw new InvalidTypeForOperationException($"Invalid search operation on type {propertyType}. A search operation can be performed only on string properties.");
            }

            // x.Prop1 != null && x.Prop1.Prop2 != null
            Expression nullCheckExpr = this.BuildNullCheckExpression(propExpr, propertyType);

            // x.Prop1.Prop2.ToLower()
            Expression toLowerExpr = this.BuildToLowerExpression(propExpr);

            // x.Prop1.Prop2.Contains(searchVal)
            Expression containsExpr = this.BuildComparissonExpression(propertyType, toLowerExpr, search);

            // join the null check expressions and the string.Contains() expression with a AND clause
            // If no null check expressions (when the property cannot be null) only the string.Contains() expression is used
            var joinedExpr = nullCheckExpr == null ?
                (Expression)containsExpr :
                Expression.AndAlso(nullCheckExpr, containsExpr);

            return joinedExpr;
        }

        private Expression BuildComparissonExpression(Type propertyType, Expression toLowerExpr, string search)
        {
            if (propertyType == typeof(string))
            {
                // searchVal
                var searchValExpr = Expression.Constant(search.ToLower());

                // x => x.Contains(search)
                var containsMethodInfo = typeof(string).GetMethod("Contains");
                var containsExpr = Expression.Call(toLowerExpr, containsMethodInfo, searchValExpr);

                return containsExpr;
            }
            else
            {
                if (search.Length > 1)
                {
                    return Expression.Constant(false);
                }

                // search value as lower case char type value
                var searchCharValExpr = Expression.Constant(char.ToLower(search.Single()));

                // x => x == search
                var charValue = char.Parse(search);
                var charComparissonExpr = Expression.Equal(toLowerExpr, searchCharValExpr);

                return charComparissonExpr;
            }
        }

        private Expression BuildToLowerExpression(MemberExpression propExpr)
        {
            var propertyType = propExpr.Type;

            if (propExpr.Type == typeof(string))
            {
                var toLowerMethodInfo = propertyType.GetMethods().Where(m => m.Name == "ToLower" && !m.GetParameters().Any()).First();
                var toLowerExpr = Expression.Call(propExpr, toLowerMethodInfo);

                return toLowerExpr;
            }
            else if (propertyType == typeof(char) || propertyType == typeof(char?))
            {
                var toLowerMethodInfo = typeof(char).GetMethods().Where(m => m.Name == "ToLower" && m.GetParameters().Count() == 1).First();
                if (propertyType == typeof(char?))
                {
                    var valueMethodInfo = typeof(Nullable<char>).GetMethods()
                        .Where(x => x.Name == "GetValueOrDefault")
                        .First(x => !x.GetGenericArguments().Any());

                    var valueExpr = Expression.Call(propExpr, valueMethodInfo);

                    return Expression.Call(null, toLowerMethodInfo, valueExpr);
                }

                var toLowerExpr = Expression.Call(null, toLowerMethodInfo, propExpr);

                return toLowerExpr;
            }
            else
            {
                throw new InvalidTypeForOperationException();
            }
        }

        private Expression BuildNullCheckExpression(MemberExpression propExpr, Type propertyType)
        {
            Expression nullCheckExpr = null;

            if (propertyType == typeof(string) || propertyType == typeof(char?))
            {
                nullCheckExpr = Expression.NotEqual(propExpr, Expression.Constant(null));
            }

            return nullCheckExpr;
        }
    }
}