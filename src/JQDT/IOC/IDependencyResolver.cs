using Autofac;

namespace JQDT.IOC
{
    internal static class Container
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MyTestClass>();

            var container = builder.Build();

            return container;
        }

        public static T Resolve<T>()
        {
        }
    }
}