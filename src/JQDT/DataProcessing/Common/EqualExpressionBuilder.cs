namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Builds equality check expression
    /// </summary>
    internal class EqualExpressionBuilder
    {
        private readonly AndExpressionBuilder andExpressionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualExpressionBuilder"/> class.
        /// </summary>
        /// <param name="andExpressionBuilder">The and expression builder.</param>
        public EqualExpressionBuilder(AndExpressionBuilder andExpressionBuilder)
        {
            this.andExpressionBuilder = andExpressionBuilder;
        }

        /// <summary>
        /// Builds the expression. Ads null checks where needed.
        /// </summary>
        /// <param name="propertyExpr">The property expr.</param>
        /// <param name="constantExpr">The constant expr.</param>
        /// <returns><see cref="Expression"/></returns>
        internal Expression BuildExpression(MemberExpression propertyExpr, Expression constantExpr)
        {
            if (propertyExpr.Type == typeof(DateTime) || propertyExpr.Type == typeof(DateTimeOffset) || propertyExpr.Type == typeof(DateTime?) || propertyExpr.Type == typeof(DateTimeOffset?))
            {
                var propertyExpressionBuilderFunction = this.GetPropertyExpressionFunction(propertyExpr.Type.IsGenericType);

                var propYearEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Year");
                var propMonthEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Month");
                var propDayEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Day");
                var propHourEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Hour");
                var propMinutesEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Minute");
                var propSecondsEpxpr = propertyExpressionBuilderFunction(propertyExpr, "Second");

                var constYearEpxpr = propertyExpressionBuilderFunction(constantExpr, "Year");
                var constMonthEpxpr = propertyExpressionBuilderFunction(constantExpr, "Month");
                var constDayEpxpr = propertyExpressionBuilderFunction(constantExpr, "Day");
                var constHourEpxpr = propertyExpressionBuilderFunction(constantExpr, "Hour");
                var constMinutesEpxpr = propertyExpressionBuilderFunction(constantExpr, "Minute");
                var constSecondsEpxpr = propertyExpressionBuilderFunction(constantExpr, "Second");

                var comparissonExpressions = new List<Expression>();
                if (propertyExpr.Type.IsGenericType)
                {
                    // Add null check expression if needed!
                    comparissonExpressions.Add(Expression.Property(propertyExpr, "HasValue"));
                }

                comparissonExpressions.Add(Expression.Equal(propYearEpxpr, constYearEpxpr));
                comparissonExpressions.Add(Expression.Equal(propMonthEpxpr, constMonthEpxpr));
                comparissonExpressions.Add(Expression.Equal(propDayEpxpr, constDayEpxpr));
                comparissonExpressions.Add(Expression.Equal(propHourEpxpr, constHourEpxpr));
                comparissonExpressions.Add(Expression.Equal(propMinutesEpxpr, constMinutesEpxpr));
                comparissonExpressions.Add(Expression.Equal(propSecondsEpxpr, constSecondsEpxpr));

                var joinedComparissonExpr = this.andExpressionBuilder.BuildExpression(comparissonExpressions);

                return joinedComparissonExpr;
            }
            else
            {
                // prop == const
                return Expression.Equal(propertyExpr, constantExpr);
            }
        }

        private Func<Expression, string, MemberExpression> GetPropertyExpressionFunction(bool isNullable)
        {
            if (isNullable)
            {
                return this.BuildNullablePropertyExpression;
            }

            return this.BuildNonNullablePropertyExpression;
        }

        private MemberExpression BuildNonNullablePropertyExpression(Expression member, string propName)
        {
            if (member.Type == typeof(DateTimeOffset))
            {
                member = Expression.Property(member, "UtcDateTime");
            }

            var propExpr = Expression.Property(member, propName);

            return propExpr;
        }

        private MemberExpression BuildNullablePropertyExpression(Expression member, string propName)
        {
            var valueExpr = Expression.Property(member, "Value");
            if (valueExpr.Type == typeof(DateTimeOffset))
            {
                valueExpr = Expression.Property(valueExpr, "UtcDateTime");
            }

            var propExpr = Expression.Property(valueExpr, propName);

            return propExpr;
        }
    }
}