using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JQDT.DataProcessing.Common
{
    internal class EqualExpressionBuilder
    {
        private readonly AndExpressionBuilder andExpressionBuilder;

        public EqualExpressionBuilder(AndExpressionBuilder andExpressionBuilder)
        {
            this.andExpressionBuilder = andExpressionBuilder;
        }

        internal Expression BuildExpression(MemberExpression propertyExpr, Expression constantExpr)
        {
            // TODO: Add validation for operation type
            if (propertyExpr.Type == typeof(DateTime) || propertyExpr.Type == typeof(DateTimeOffset) || propertyExpr.Type == typeof(DateTime?) || propertyExpr.Type == typeof(DateTimeOffset?))
            {
                var propYearEpxpr = Expression.Property(propertyExpr, "Year");
                var propMonthEpxpr = Expression.Property(propertyExpr, "Month");
                var propDayEpxpr = Expression.Property(propertyExpr, "Day");
                var propHourEpxpr = Expression.Property(propertyExpr, "Hour");
                var propMinutesEpxpr = Expression.Property(propertyExpr, "Minute");
                var propSecondsEpxpr = Expression.Property(propertyExpr, "Second");

                var constYearEpxpr = Expression.Property(constantExpr, "Year");
                var constMonthEpxpr = Expression.Property(constantExpr, "Month");
                var constDayEpxpr = Expression.Property(constantExpr, "Day");
                var constHourEpxpr = Expression.Property(constantExpr, "Hour");
                var constMinutesEpxpr = Expression.Property(constantExpr, "Minute");
                var constSecondsEpxpr = Expression.Property(constantExpr, "Second");

                var comparissonExpressions = new List<Expression>
                {
                    Expression.Equal(propYearEpxpr, constYearEpxpr),
                    Expression.Equal(propMonthEpxpr, constMonthEpxpr),
                    Expression.Equal(propDayEpxpr, constDayEpxpr),
                    Expression.Equal(propHourEpxpr, constHourEpxpr),
                    Expression.Equal(propMinutesEpxpr, constMinutesEpxpr),
                    Expression.Equal(propSecondsEpxpr, constSecondsEpxpr),
                };

                var joinedComparissonExpr = this.andExpressionBuilder.BuildExpression(comparissonExpressions);

                return joinedComparissonExpr;
            }
            else
            {
                // prop == const
                return Expression.Equal(propertyExpr, constantExpr);
            }
        }
    }
}