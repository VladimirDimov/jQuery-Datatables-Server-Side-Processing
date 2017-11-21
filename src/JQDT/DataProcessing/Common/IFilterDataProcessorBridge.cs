namespace JQDT.DataProcessing.Common
{
    using System.Linq.Expressions;

    /// <summary>
    /// Generic interface for the common filter logic.
    /// </summary>
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