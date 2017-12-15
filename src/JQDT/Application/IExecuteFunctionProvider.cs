namespace JQDT.Application
{
    using System;
    using JQDT.ModelBinders;

    /// <summary>
    /// A provider for the application initialization function
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface IExecuteFunctionProvider<TContext>
    {
        /// <summary>
        /// Gets the application initialization function.
        /// </summary>
        /// <param name="dataCollectionType">Type of the data collection.</param>
        /// <param name="appType">Type of the application.</param>
        /// <returns>Application initialization function</returns>
        Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase> GetAppInicializationFunc(Type dataCollectionType, Type appType);
    }
}