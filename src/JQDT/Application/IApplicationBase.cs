namespace JQDT.Application
{
    using JQDT.Models;

    /// <summary>
    /// Base interface for <see cref="ApplicationBase{T}"/>
    /// </summary>
    public interface IApplicationBase
    {
        /// <summary>
        /// Application entry point method. Executes all data processors.
        /// </summary>
        /// <returns>Processed data as <see cref="ResultModel"/></returns>
        ResultModel Execute();
    }
}