namespace JQDT.Application
{
    using JQDT.Delegates;
    using JQDT.Models;

    /// <summary>
    /// Base interface for <see cref="ApplicationBase{T}"/>
    /// </summary>
    public interface IApplicationBase
    {
        /// <summary>
        /// Occurs when [on data processed].
        /// </summary>
        event DataProcessorEventHandler OnDataProcessedEvent;

        /// <summary>
        /// Occurs when [on search data processing event].
        /// </summary>
        event DataProcessorEventHandler OnSearchDataProcessingEvent;

        /// <summary>
        /// Application entry point method. Executes all data processors.
        /// </summary>
        /// <returns>Processed data as <see cref="ResultModel"/></returns>
        ResultModel Execute();
    }
}