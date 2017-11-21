namespace JQDT.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Extension methods for <see cref="Expression"/>
    /// </summary>
    internal static class ExpressionExtensions
    {
        private const string NullExpressionException = "Cannot apply nested property on null expression. Nested property path: {0}";
        private const string NullEmptyOrWhitespacePropertyPathException = "Property path cannot be null, empty or whitespace.";
        private const string NullPropertyInfoPathException = "Property info path cannot be null.";
        private const string EmptyPropertyInfoPathException = "Property info path cannot be an empty collection.";

        /// <summary>
        /// Generates Property Expression for provided property path.
        /// The property path is a path to the nested property delimited by ".".
        /// Example: "MyType.Property1.Property2.Property3"
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>Property select expression as <see cref="MemberExpression"/></returns>
        internal static MemberExpression NestedProperty(this Expression expression, string propertyPath)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(string.Format(NullExpressionException, propertyPath));
            }

            if (string.IsNullOrEmpty(propertyPath) || string.IsNullOrWhiteSpace(propertyPath))
            {
                throw new ArgumentNullException(NullEmptyOrWhitespacePropertyPathException);
            }

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
        internal static MemberExpression NestedProperty(this Expression expression, IEnumerable<PropertyInfo> propertyInfoPath)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        NullExpressionException,
                        string.Join(".", propertyInfoPath.Select(x => x.Name))));
            }

            if (propertyInfoPath == null)
            {
                throw new ArgumentNullException(NullPropertyInfoPathException);
            }

            if (propertyInfoPath.Count() == 0)
            {
                throw new ArgumentException(EmptyPropertyInfoPathException);
            }

            MemberExpression propertyExpression = null;
            foreach (var propertyInfo in propertyInfoPath)
            {
                propertyExpression = Expression.Property(propertyExpression ?? expression, propertyInfo);
            }

            return propertyExpression;
        }
    }
}