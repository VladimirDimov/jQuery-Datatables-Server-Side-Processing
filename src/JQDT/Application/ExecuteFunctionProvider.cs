namespace JQDT.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    /// <summary>
    /// Creates and caches application execute function.
    /// </summary>
    internal static class ExecuteFunctionProvider
    {
        private static ConcurrentDictionary<Type, Func<ActionExecutedContext, DI.IDependencyResolver, object>> mvcExecuteFunctionsCache = new ConcurrentDictionary<Type, Func<ActionExecutedContext, DI.IDependencyResolver, object>>();

        /// <summary>
        /// Gets the execute function.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="appType">Type of the application.</param>
        /// <returns>Application execute function.</returns>
        internal static Func<ActionExecutedContext, DI.IDependencyResolver, object> GetExecuteFunction(Type modelType, Type appType)
        {
            var cache = GetCurrentCache(appType);
            Func<ActionExecutedContext, DI.IDependencyResolver, object> executeFunc = null;

            if (!cache.TryGetValue(modelType, out executeFunc))
            {
                Type[] typeArgs = { modelType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(ActionExecutedContext), "context");
                var dependencyResolverExpr = Expression.Parameter(typeof(DI.IDependencyResolver));
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr, dependencyResolverExpr);
                var executeMethodInfo = genericAppType.GetMethod("Execute");
                var executeCallExpr = Expression.Call(newAppExpr, executeMethodInfo);
                var lambda = Expression.Lambda(executeCallExpr, contextExpr, dependencyResolverExpr);

                executeFunc = (Func<ActionExecutedContext, DI.IDependencyResolver, object>)lambda.Compile();
                cache.TryAdd(modelType, executeFunc);
            }

            return executeFunc;
        }

        private static ConcurrentDictionary<Type, Func<ActionExecutedContext, DI.IDependencyResolver, object>> GetCurrentCache(Type appType)
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