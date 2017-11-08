namespace JQDT.Extensions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionExtensions
    {
        public static MemberExpression NestedProperty(this Expression expression, string propertyPath)
        {
            // TODO: Catch expression == null
            // TODO: Catch propertyPath == null
            var properties = propertyPath.Split('.');
            MemberExpression propertyExpression = null;
            foreach (var property in properties)
            {
                propertyExpression = Expression.Property(propertyExpression ?? expression, property);
            }

            return propertyExpression;
        }

        public static MemberExpression NestedProperty(this Expression expression, IEnumerable<PropertyInfo> propertyInfoPath)
        {
            // TODO: Catch expression == null
            // TODO: Catch propertyInfoPath == null or propertyInfoPath.Count() == 0
            MemberExpression propertyExpression = null;
            foreach (var propertyInfo in propertyInfoPath)
            {
                propertyExpression = Expression.Property(propertyExpression ?? expression, propertyInfo);
            }

            return propertyExpression;
        }
    }
}