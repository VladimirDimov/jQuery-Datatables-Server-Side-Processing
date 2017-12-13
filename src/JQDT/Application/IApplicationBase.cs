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
        /// Occurs when [on data processing event].
        /// </summary>
        event DataProcessorEventHandler OnDataProcessingEvent;

        /// <summary>
        /// Occurs when [on data processed].
        /// </summary>
        event DataProcessorEventHandler OnDataProcessedEvent;

        /// <summary>
        /// Occurs when [on search data processing event].
        /// </summary>
        event DataProcessorEventHandler OnSearchDataProcessingEvent;

        /// <summary>
        /// Occurs when [on search data processed event].
        /// </summary>
        event DataProcessorEventHandler OnSearchDataProcessedEvent;

        /// <summary>
        /// Occurs when [on custom filters data processing event].
        /// </summary>
        event DataProcessorEventHandler OnCustomFiltersDataProcessingEvent;

        /// <summary>
        /// Occurs when [on custom filters data processed event].
        /// </summary>
        event DataProcessorEventHandler OnCustomFiltersDataProcessedEvent;

        /// <summary>
        /// Occurs when [on columns filter data processing event].
        /// </summary>
        event DataProcessorEventHandler OnColumnsFilterDataProcessingEvent;

        /// <summary>
        /// Occurs when [on columns filter data processed event].
        /// </summary>
        event DataProcessorEventHandler OnColumnsFilterDataProcessedEvent;

        /// <summary>
        /// Occurs when [on sort data processing event].
        /// </summary>
        event DataProcessorEventHandler OnSortDataProcessingEvent;

        /// <summary>
        /// Occurs when [on sort data processed event].
        /// </summary>
        event DataProcessorEventHandler OnSortDataProcessedEvent;

        /// <summary>
        /// Occurs when [on paging data processing event].
        /// </summary>
        event DataProcessorEventHandler OnPagingDataProcessingEvent;

        /// <summary>
        /// Occurs when [on paging data processed event].
        /// </summary>
        event DataProcessorEventHandler OnPagingDataProcessedEvent;

        /// <summary>
        /// Application entry point method. Executes all data processors.
        /// </summary>
        /// <returns>Processed data as <see cref="ResultModel"/></returns>
        ResultModel Execute();
    }
}