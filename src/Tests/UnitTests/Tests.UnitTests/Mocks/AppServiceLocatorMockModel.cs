namespace Tests.UnitTests.Mocks
{
    using JQDT.DI;
    using Moq;

    public class AppServiceLocatorMockModel<T>
    {
        public AppMock<T> AppMock { get; set; }

        public Mock<IServiceLocator> ServiceLocatorMock { get; set; }
    }
}