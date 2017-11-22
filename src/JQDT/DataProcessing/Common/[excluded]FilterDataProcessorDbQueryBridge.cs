namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Filter data processor methods for the case when data collection is of type <see cref="DbQuery"/>
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.FilterDataProcessor.IFilterDataProcessorBridge" />
    internal class FilterDataProcessorDbQueryBridge : IFilterDataProcessorBridge
    {
        private static HashSet<Type> supportedNumericTypes;

        static FilterDataProcessorDbQueryBridge()
        {
            supportedNumericTypes = new HashSet<Type>
            {
                typeof(byte),
                typeof(sbyte),
                typeof(decimal),
                typeof(double),
                typeof(float),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(short),
                typeof(ushort),
            };
        }

        /// <summary>
        /// Gets the string contains expression. Ex: x =&gt; x.Contains(value)
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// true if contains. False otherwise.
        /// </returns>
        /// <exception cref="NotImplementedException">Thrown when the property is of unsupported type.</exception>
        public Expression GetConvertToStringExpression(MemberExpression propertyExpression)
        {
            if (propertyExpression.Type == typeof(string) || propertyExpression.Type == typeof(char))
            {
                return propertyExpression;
            }
            else if (supportedNumericTypes.Contains(propertyExpression.Type))
            {
                // SqlFunctions.StringConvert((decimal)x.Property)
                //var stringConvertMethodInfo = typeof(SqlFunctions).GetMethods()
                //    .Where(m =>
                //    {
                //        if (m.Name != "StringConvert")
                //        {
                //            return false;
                //        };

                //        var parameters = m.GetParameters();
                //        var numberOfParameters = parameters.Count();
                //        if (numberOfParameters != 1)
                //        {
                //            return false;
                //        }

                //        if (parameters.First().ParameterType != typeof(decimal?))
                //        {
                //            return false;
                //        }

                //        return true;
                //    }).Single();

                //var castToDecimalExpr = Expression.Convert(propertyExpression, typeof(decimal?));
                //var stringConvertExpr = Expression.Call(stringConvertMethodInfo, castToDecimalExpr);

                //return stringConvertExpr;
            }

            throw new NotImplementedException($"Unsupported searchable type: {propertyExpression.Type.FullName}. Supported types: {string.Join(", ", supportedNumericTypes)}");
        }
    }
}