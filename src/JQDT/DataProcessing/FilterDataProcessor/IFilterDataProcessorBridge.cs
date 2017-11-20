namespace JQDT.DataProcessing.FilterDataProcessor
{
    using System.Linq.Expressions;

    internal interface IFilterDataProcessorBridge
    {
        /// <summary>
        /// Gets the string contains expression. Ex: x => x.Contains(value)
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>true if contains. False otherwise.</returns>
        Expression GetStringContainsExpression(MemberExpression propertyExpression);
    }
}