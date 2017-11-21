namespace JQDT.DataProcessing.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class DynamicParser
    {
        private Dictionary<Type, Func<string, object>> parseFunctionsCache = new Dictionary<Type, Func<string, object>>();

        public object DynamicParse(string value, Type toType)
        {
            var func = this.GetParseFunction(toType);
            object result = null;
            try
            {
                result = func(value);
            }
            catch (Exception)
            {
                new ArgumentException($"Unable to parse value {value} to type {toType.FullName}.");
            }

            return result;
        }

        public Func<string, object> GetParseFunction(Type toType)
        {
            Func<string, object> func = null;

            if (!parseFunctionsCache.TryGetValue(toType, out func))
            {
                var xExpr = Expression.Parameter(typeof(string), "x");
                var gteMethodInfo = toType.GetMethods().First(x => x.Name == "Parse");
                var parseExpr = Expression.Call(null, gteMethodInfo, xExpr);
                var castExpr = Expression.Convert(parseExpr, typeof(object));

                var lambda = Expression.Lambda(castExpr, xExpr);
                func = (Func<string, object>)lambda.Compile();

                parseFunctionsCache.Add(toType, func);
            }

            return func;
        }
    }
}