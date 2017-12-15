namespace JQDT.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using JQDT.ModelBinders;

    /// <summary>
    /// Creates and caches application execute function.
    /// </summary>
    /// <typeparam name="TContext">The type of the request context.</typeparam>
    public class ExecuteFunctionProvider<TContext> : IExecuteFunctionProvider<TContext>
    {
        private static ConcurrentDictionary<Type, Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase>> appInitFunctionsCache = new ConcurrentDictionary<Type, Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase>>();

        /// <summary>
        /// Gets the application initialization function.
        /// </summary>
        /// <param name="dataCollectionType">Type of the data collection.</param>
        /// <param name="appType">Type of the application.</param>
        /// <returns><see cref="Func{T, TResult}"/> that return new <see cref="IApplicationBaseIFormModelBinder"/> instance</returns>
        public Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase> GetAppInicializationFunc(Type dataCollectionType, Type appType)
        {
            Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase> executeFunc = null;

            if (!appInitFunctionsCache.TryGetValue(dataCollectionType, out executeFunc))
            {
                Type[] typeArgs = { dataCollectionType.GenericTypeArguments.First() };
                var genericAppType = appType.MakeGenericType(typeArgs);

                var contextExpr = Expression.Parameter(typeof(TContext), "context");
                var srviceLocatorExpr = Expression.Parameter(typeof(DI.IServiceLocator));
                var formModelBinderExpr = Expression.Parameter(typeof(IFormModelBinder));
                var appConstructorInfo = genericAppType.GetConstructors().First();
                var newAppExpr = Expression.New(appConstructorInfo, contextExpr, srviceLocatorExpr, formModelBinderExpr);
                var lambda = Expression.Lambda(newAppExpr, contextExpr, srviceLocatorExpr, formModelBinderExpr);

                executeFunc = (Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase>)lambda.Compile();
                appInitFunctionsCache.TryAdd(dataCollectionType, executeFunc);
            }

            return executeFunc;
        }
    }
}