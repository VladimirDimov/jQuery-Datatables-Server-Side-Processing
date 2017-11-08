namespace JQDT.Extensions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Extension methods for <see cref="Expression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Generates Property Expression for provided property path.
        /// The property path is a path to the nested property delimited by ".".
        /// Example: "MyType.Property1.Property2.Property3"
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>Property select expression as <see cref="MemberExpression"/></returns>
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

        /// <summary>
        /// Generates Property Expression for provided property path as a collection of <see cref="PropertyInfo"/> 
        /// from parent property to target property.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="propertyInfoPath">The property information path.</param>
        /// <returns>Property expression as <see cref="MemberExpression"/></returns>
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