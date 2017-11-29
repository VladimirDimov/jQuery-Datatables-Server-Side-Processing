namespace Tests.SeleniumTests
{
    using global::Tests.SeleniumTests.Common;
    using NUnit.Framework;

    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Do login here.
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            DriverSingletonProvider.Dispose();
        }
    }
}