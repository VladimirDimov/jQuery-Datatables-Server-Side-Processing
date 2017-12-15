namespace JQDT.Application
{
    using System;
    using JQDT.ModelBinders;

    public interface IExecuteFunctionProvider<TContext>
    {
        Func<TContext, DI.IServiceLocator, IFormModelBinder, IApplicationBase> GetAppInicializationFunc(Type dataCollectionType, Type appType);
    }
}