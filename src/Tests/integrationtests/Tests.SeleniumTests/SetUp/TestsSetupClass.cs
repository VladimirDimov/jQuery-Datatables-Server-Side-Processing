namespace Tests.SeleniumTests
{
    using NUnit.Framework;
    using Tests.SeleniumTests.Common;

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
            DriverProvider.Dispose();
        }
    }
}