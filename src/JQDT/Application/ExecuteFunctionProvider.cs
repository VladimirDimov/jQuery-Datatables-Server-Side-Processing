namespace JQDT.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Creates and caches application execute function.
    /// </summary>
    /// <typeparam name="TContext">The type of the request context.</typeparam>
    public static class ExecuteFunctionProvider<TContext>
    {
        private static ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, object>> executionFunctionsCache = new ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, object>>();
        private static ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, IApplicationBase>> appInitFunctionsCache = new ConcurrentDictionary<Type, Func<TContext, DI.IDependencyResolver, IApplicationBase>>();

        /// <summary>
        /// Gets the execute function.
        /// </summary>
        /// <param name="dataCollectionType">The type of the data collection.</param>
        /// <param name="appType">Type of the application.</param>
        /// <returns>Application execute function.</returns>
        public static Func<TContext, DI.IDependencyResolver, object> GetExecuteFunction(Type dataCollectionType, Type appType)
        {
            Func<TContext, DI.IDependencyResolver, object> executeFunc = null;

            if (!executionFunctionsCache.TryGetValue(dataCollectionType, out executeFunc))
            {
                Type[] typeArgs = { dataCollectionType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(TContext), "context");
                var dependencyResolverExpr = Expression.Parameter(typeof(DI.IDependencyResolver));
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr, dependencyResolverExpr);
                var executeMethodInfo = genericAppType.GetMethod("Execute");
                var executeCallExpr = Expression.Call(newAppExpr, executeMethodInfo);
                var lambda = Expression.Lambda(executeCallExpr, contextExpr, dependencyResolverExpr);

                executeFunc = (Func<TContext, DI.IDependencyResolver, object>)lambda.Compile();
                executionFunctionsCache.TryAdd(dataCollectionType, executeFunc);
            }

            return executeFunc;
        }

        /// <summary>
        /// Gets the application initialization function.
        /// </summary>
        /// <param name="dataCollectionType">Type of the data collection.</param>
        /// <param name="appType">Type of the application.</param>
        /// <returns><see cref="Func{T, TResult}"/> that return new <see cref="IApplicationBase"/> instance</returns>
        public static Func<TContext, DI.IDependencyResolver, IApplicationBase> GetAppInicializationFunc(Type dataCollectionType, Type appType)
        {
            Func<TContext, DI.IDependencyResolver, IApplicationBase> executeFunc = null;

            if (!appInitFunctionsCache.TryGetValue(dataCollectionType, out executeFunc))
            {
                Type[] typeArgs = { dataCollectionType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(TContext), "context");
                var dependencyResolverExpr = Expression.Parameter(typeof(DI.IDependencyResolver));
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr, dependencyResolverExpr);
                var lambda = Expression.Lambda(newAppExpr, contextExpr, dependencyResolverExpr);

                executeFunc = (Func<TContext, DI.IDependencyResolver, IApplicationBase>)lambda.Compile();
                executionFunctionsCache.TryAdd(dataCollectionType, executeFunc);
            }

            return executeFunc;
        }
    }
}