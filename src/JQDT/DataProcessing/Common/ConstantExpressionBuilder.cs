namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Builds constant expression
    /// </summary>
    internal class ConstantExpressionBuilder
    {
        private readonly DynamicParser dynamicParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantExpressionBuilder"/> class.
        /// </summary>
        /// <param name="dynamicParser">The dynamic parser.</param>
        public ConstantExpressionBuilder(DynamicParser dynamicParser)
        {
            this.dynamicParser = dynamicParser;
        }

        /// <summary>
        /// Dynamically parses the value to the provided type and builds the constant expression.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns><see cref="ConstantExpression"/> of the provided value and type</returns>
        internal Expression BuildExpression(string value, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Expression.Constant(value, typeof(string));
            }

            var parsedValue = this.dynamicParser.DynamicParse(value, propertyType);
            var constant = Expression.Constant(parsedValue);
            var constantCast = Expression.Convert(constant, propertyType);

            return constantCast;
        }
    }
}