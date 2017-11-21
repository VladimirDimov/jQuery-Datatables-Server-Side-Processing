namespace JQDT.DataProcessing.Common
{
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A common filter logic specific for the case when the data collection is <see cref="EnumerableQuery"/>
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.Common.IFilterDataProcessorBridge" />
    internal class FilterDataProcessorEnumerableQueryBridge : IFilterDataProcessorBridge
    {
        /// <summary>
        /// Gets the string contains expression. Ex: x =&gt; x.Contains(value)
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// true if contains. False otherwise.
        /// </returns>
        public Expression GetConvertToStringExpression(MemberExpression propertyExpression)
        {
            // x.Name.ToString()
            var toStringMethodInfo = propertyExpression.Type.GetMethods().Where(x => x.Name == "ToString" && !x.GetGenericArguments().Any()).First();
            var toStringExpr = Expression.Call(propertyExpression, toStringMethodInfo);

            return toStringExpr;
        }
    }
}