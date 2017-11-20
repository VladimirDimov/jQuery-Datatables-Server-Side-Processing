namespace JQDT.DataProcessing.FilterDataProcessor
{
    using System.Linq.Expressions;

    internal interface IFilterDataProcessorBridge
    {
        Expression GetStringContainsExpression(MemberExpression propertyExpression);
    }
}