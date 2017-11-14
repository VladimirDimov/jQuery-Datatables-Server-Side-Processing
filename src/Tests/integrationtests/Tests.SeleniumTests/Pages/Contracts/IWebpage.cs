namespace Tests.SeleniumTests.Pages.Contracts
{
    internal interface IWebPage<T>
    {
        T NavigateTo();
    }
}