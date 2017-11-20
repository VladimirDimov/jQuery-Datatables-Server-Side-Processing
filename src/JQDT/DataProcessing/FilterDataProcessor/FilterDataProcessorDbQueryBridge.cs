namespace JQDT.DataProcessing.FilterDataProcessor
{
    using System;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Filter data processor methods for the case when data collection is of type <see cref="DbQuery"/>
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.FilterDataProcessor.IFilterDataProcessorBridge" />
    internal class FilterDataProcessorDbQueryBridge : IFilterDataProcessorBridge
    {
        /// <summary>
        /// Gets the string contains expression. Ex: x =&gt; x.Contains(value)
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// true if contains. False otherwise.
        /// </returns>
        /// <exception cref="NotImplementedException">Thrown when the property is of unsupported type.</exception>
        public Expression GetStringContainsExpression(MemberExpression propertyExpression)
        {
            if (propertyExpression.Type == typeof(string))
            {
                return propertyExpression;
            }
            else if (propertyExpression.Type == typeof(int) || propertyExpression.Type == typeof(long) || propertyExpression.Type == typeof(double) || propertyExpression.Type == typeof(int?))
            {
                // SqlFunctions.StringConvert((decimal)x.Property)
                var stringConvertMethodInfo = typeof(SqlFunctions).GetMethods()
                    .Where(m =>
                    {
                        if (m.Name != "StringConvert")
                        {
                            return false;
                        };

                        var parameters = m.GetParameters();
                        var numberOfParameters = parameters.Count();
                        if (numberOfParameters != 1)
                        {
                            return false;
                        }

                        if (parameters.First().ParameterType != typeof(decimal?))
                        {
                            return false;
                        }

                        return true;
                    }).Single();

                var castToDecimalExpr = Expression.Convert(propertyExpression, typeof(decimal?));
                var stringConvertExpr = Expression.Call(stringConvertMethodInfo, castToDecimalExpr);

                return stringConvertExpr;
            }

            throw new NotImplementedException($"Cannot filter by type: {propertyExpression.Type.FullName}");
        }
    }
}