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
        // TODO: Remove the generic parameter        
        /// <summary>
        /// Builds the expression. A null check expression is attached to the primary expression.
        /// </summary>
        /// <param name="modelExpr">The model expr.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        internal Expression BuildExpression(ParameterExpression modelExpr, string propertyPath, FilterModel filter)
        {
            // x
            var modelType = modelExpr.Type;
            var propertyInfoPath = modelType.GetPropertyInfoPath(propertyPath);
            var propertyType = propertyInfoPath.Last().PropertyType;
            this.operationTypeValidator.ValidatePropertyType(propertyType, filter.Type);

            // x.Property1.Property2
            var propertyExpr = modelExpr.NestedProperty(propertyPath);

            // Convert(value)
            Expression constantExpr = this.constantExpressionBuilder.BuildExpression(filter.Value, propertyType);

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

            var nullCheckExpr = this.nullCheckExpressionBuilder.BuildExpression(modelExpr, string.Join(".", propertyPath));

            Expression joinedExpr = nullCheckExpr == null ? (Expression)rangeExpr : Expression.AndAlso(nullCheckExpr, rangeExpr);

            return joinedExpr;
        }
    }
}
