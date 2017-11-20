namespace JQDT.DataProcessing.FilterDataProcessor
{
    using System;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Linq.Expressions;

    internal class FilterDataProcessorDbQueryBridge : IFilterDataProcessorBridge
    {
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
                        if (m.Name != "StringConvert") return false;
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