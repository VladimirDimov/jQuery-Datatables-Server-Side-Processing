namespace Tests.SeleniumTests.Common
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;

    public static class ExceptionsHandler
    {
        public static void Hande(Action action, IWebDriver driver)
        {
            try
            {
                action();
            }
            catch (UnhandledAlertException ex)
            {
                IAlert alert = driver.SwitchTo().Alert();
                Console.WriteLine(alert.Text);
                DriverSingletonProvider.Dispose();
                driver = DriverSingletonProvider.GetDriver();
                Assert.Fail(ex.Message + "; " + alert.Text);
            }
        }
    }
}