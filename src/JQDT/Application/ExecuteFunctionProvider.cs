namespace JQDT.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Creates and caches application execute function.
    /// </summary>
    public static class ExecuteFunctionProvider<TContext>
    {
        private static ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, object>> executionFunctionsCache = new ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, object>>();

        /// <summary>
        /// Gets the execute function.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>Application execute function.</returns>
        public static Func<TContext, DI.IDependencyResolver, object> GetExecuteFunction(Type modelType, Type appType)
        {
            Func<TContext, DI.IDependencyResolver, object> executeFunc = null;

            if (!executionFunctionsCache.TryGetValue(modelType, out executeFunc))
            {
                Type[] typeArgs = { modelType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(TContext), "context");
                var dependencyResolverExpr = Expression.Parameter(typeof(DI.IDependencyResolver));
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr, dependencyResolverExpr);
                var executeMethodInfo = genericAppType.GetMethod("Execute");
                var executeCallExpr = Expression.Call(newAppExpr, executeMethodInfo);
                var lambda = Expression.Lambda(executeCallExpr, contextExpr, dependencyResolverExpr);

                executeFunc = (Func<TContext, DI.IDependencyResolver, object>)lambda.Compile();
                executionFunctionsCache.TryAdd(modelType, executeFunc);
            }

            return executeFunc;
        }
    }
}