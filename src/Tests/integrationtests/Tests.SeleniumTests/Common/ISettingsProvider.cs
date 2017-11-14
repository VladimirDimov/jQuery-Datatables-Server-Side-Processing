namespace Tests.SeleniumTests.Common
{
    public interface ISettingsProvider
    {
        string this[string index] { get; }
    }
}