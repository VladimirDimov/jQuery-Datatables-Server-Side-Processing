namespace JQDT.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    internal static class ExecuteFunctionProvider
    {
        private static ConcurrentDictionary<Type, Func<ActionExecutedContext, object>> mvcExecuteFunctionsCache = new ConcurrentDictionary<Type, Func<ActionExecutedContext, object>>();

        internal static Func<ActionExecutedContext, object> GetExecuteFunction(Type modelType, Type appType)
        {
            var cache = GetCurrentCache(appType);
            Func<ActionExecutedContext, object> executeFunc = null;

            if (!cache.TryGetValue(modelType, out executeFunc))
            {
                Type[] typeArgs = { modelType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(ActionExecutedContext), "context");
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr);
                var executeMethodInfo = genericAppType.GetMethod("Execute");
                var executeCallExpr = Expression.Call(newAppExpr, executeMethodInfo);
                var lambda = Expression.Lambda(executeCallExpr, contextExpr);

                executeFunc = (Func<ActionExecutedContext, object>)lambda.Compile();
                cache.TryAdd(modelType, executeFunc);
            }

            return executeFunc;
        }

        private static ConcurrentDictionary<Type, Func<ActionExecutedContext, object>> GetCurrentCache(Type appType)
        {
            if (appType == typeof(ApplicationMvc<>))
            {
                return mvcExecuteFunctionsCache;
            }
            else
            {
                throw new ArgumentException($"Unsupported application type: {appType.FullName}");
            }
        }
    }
}