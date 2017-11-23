namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.Extensions;

    internal class NullCheckExpressionBuilder
    {
        private readonly AndExpressionBuilder andExpressionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullCheckExpressionBuilder"/> class.
        /// </summary>
        /// <param name="andExpressionBuilder">The and expression builder.</param>
        public NullCheckExpressionBuilder(AndExpressionBuilder andExpressionBuilder)
        {
            this.andExpressionBuilder = andExpressionBuilder;
        }

        /// <summary>
        /// Builds the null check expression.
        /// </summary>
        /// <param name="modelParamExpr">The model parameter expr.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression BuildExpression(ParameterExpression modelParamExpr, string propertyPath)
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

            var joinedAndExpr = this.andExpressionBuilder.BuildExpression(nullCheckExprCollection);

            return joinedAndExpr;
        }
    }
}