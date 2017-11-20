namespace JQDT.DataProcessing.FilterDataProcessor
{
    using System.Linq;
    using System.Linq.Expressions;

    internal class FilterDataProcessorEnumerableQueryBridge : IFilterDataProcessorBridge
    {
        public Expression GetStringContainsExpression(MemberExpression propertyExpression)
        {
            // x.Name.ToString()
            var toStringMethodInfo = propertyExpression.Type.GetMethods().Where(x => x.Name == "ToString" && !x.GetGenericArguments().Any()).First();
            var toStringExpr = Expression.Call(propertyExpression, toStringMethodInfo);

            return toStringExpr;
        }
    }
}