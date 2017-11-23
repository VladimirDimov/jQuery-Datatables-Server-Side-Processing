namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Dynamically parses a string to a given type and returns it as a boxed value.
    /// </summary>
    internal class DynamicParser
    {
        // provides caching for the parsing functions
        private ConcurrentDictionary<Type, Func<string, object>> parseFunctionsCache = new ConcurrentDictionary<Type, Func<string, object>>();

        /// <summary>
        /// Dynamically parses a given string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="toType">To type.</param>
        /// <returns>Boxed value of the parsed result.</returns>
        internal object DynamicParse(string value, Type toType)
        {
            var func = this.GetParseFunction(toType);
            object result = null;
            try
            {
                result = func(value);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Unable to parse value {value} to type {toType.FullName}.");
            }

            return result;
        }

        /// <summary>
        /// Gets the parse function and caches it if not cached.
        /// </summary>
        /// <param name="toType">To type.</param>
        /// <returns>A <see cref="Func{T, TResult}"/> that accepts a string and returns it's parsed value as boxed value.</returns>
        private Func<string, object> GetParseFunction(Type toType)
        {
            Func<string, object> func = null;
            Type innerType = null;
            if (toType.IsGenericType)
            {
                innerType = toType.GetGenericArguments().Single();
            }
            else
            {
                innerType = toType;
            }

            if (!this.parseFunctionsCache.TryGetValue(innerType, out func))
            {
                var xExpr = Expression.Parameter(typeof(string), "x");
                var gteMethodInfo = innerType.GetMethods().First(x => x.Name == "Parse");
                var parseExpr = Expression.Call(null, gteMethodInfo, xExpr);
                var castExpr = Expression.Convert(parseExpr, typeof(object));

                var lambda = Expression.Lambda(castExpr, xExpr);

                this.parseFunctionsCache.TryAdd(innerType, (Func<string, object>)lambda.Compile());
            }

            this.parseFunctionsCache.TryGetValue(innerType, out func);

            return func;
        }
    }
}