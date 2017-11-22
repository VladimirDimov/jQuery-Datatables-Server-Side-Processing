using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JQDT.Extensions;
using JQDT.Models;

namespace JQDT.DataProcessing.Common
{
    class RangeOrEqualsExpressionBuilder
    {
        private readonly OperationTypeValidator operationTypeValidator;
        private readonly ConstantExpressionBuilder constantExpressionBuilder;
        private readonly NullCheckExpressionBuilder nullCheckExpressionBuilder;

        public RangeOrEqualsExpressionBuilder(
            OperationTypeValidator operationTypeValidator, 
            ConstantExpressionBuilder constantExpressionBuilder,
            NullCheckExpressionBuilder nullCheckExpressionBuilder)
        {
            this.operationTypeValidator = operationTypeValidator;
            this.constantExpressionBuilder = constantExpressionBuilder;
            this.nullCheckExpressionBuilder = nullCheckExpressionBuilder;
        }

        // TODO: Check the case when nullable type property is null
        internal Expression<Func<T, bool>> GetRangeOrEqualsExpression<T>(string propertyPath, FilterModel filter)
        {
            // x
            var propertyInfoPath = typeof(T).GetPropertyInfoPath(propertyPath);
            var propertyType = propertyInfoPath.Last().PropertyType;
            this.operationTypeValidator.ValidatePropertyType(propertyPath, propertyType, filter.Type);
            var xExpr = Expression.Parameter(typeof(T), "x");

            // x.Property1.Property2
            var propertyExpr = xExpr.NestedProperty(propertyPath);

            // Convert(value)
            Expression constantExpr = this.constantExpressionBuilder.BuildConstantExpression(filter.Value, propertyType);

            BinaryExpression rangeExpr = null;
            switch (filter.Type)
            {
                case FilterTypes.gte:
                    // x >= Convert(value)
                    rangeExpr = Expression.GreaterThanOrEqual(propertyExpr, constantExpr);
                    break;

                case FilterTypes.gt:
                    // x > Convert(value)
                    rangeExpr = Expression.GreaterThan(propertyExpr, constantExpr);
                    break;

                case FilterTypes.lt:
                    // x < Convert(value)
                    rangeExpr = Expression.LessThan(propertyExpr, constantExpr);
                    break;

                case FilterTypes.lte:
                    // x <= Convert(value)
                    rangeExpr = Expression.LessThanOrEqual(propertyExpr, constantExpr);
                    break;

                // x == value
                case FilterTypes.eq:
                    rangeExpr = Expression.Equal(propertyExpr, constantExpr);
                    break;
            }

            var nullCheckExpr = this.nullCheckExpressionBuilder.BuildNullCheckExpression(xExpr, string.Join(".", propertyPath));

            Expression joinedExpr = nullCheckExpr == null ? (Expression)rangeExpr : Expression.AndAlso(nullCheckExpr, rangeExpr);

            return (Expression<Func<T, bool>>)Expression.Lambda(joinedExpr, xExpr);
        }
    }
}
