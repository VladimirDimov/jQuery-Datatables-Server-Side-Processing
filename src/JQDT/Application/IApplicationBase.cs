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
        /// Occurs before all data processors execute
        /// </summary>
        event DataProcessorEventHandler OnDataProcessingEvent;

        /// <summary>
        /// Occurs after all data processors execute
        /// </summary>
        event DataProcessorEventHandler OnDataProcessedEvent;

        /// <summary>
        /// Occurs before search data processor executes
        /// </summary>
        event DataProcessorEventHandler OnSearchDataProcessingEvent;

        /// <summary>
        /// Occurs after search data processor executes
        /// </summary>
        event DataProcessorEventHandler OnSearchDataProcessedEvent;

        /// <summary>
        /// Occurs before custom filters data processor executes
        /// </summary>
        event DataProcessorEventHandler OnCustomFiltersDataProcessingEvent;

        /// <summary>
        /// Occurs after custom filters data processor executes
        /// </summary>
        event DataProcessorEventHandler OnCustomFiltersDataProcessedEvent;

        /// <summary>
        /// Occurs before column filters data processor executes
        /// </summary>
        event DataProcessorEventHandler OnColumnsFilterDataProcessingEvent;

        /// <summary>
        /// Occurs after column filters data processor executes
        /// </summary>
        event DataProcessorEventHandler OnColumnsFilterDataProcessedEvent;

        /// <summary>
        /// Occurs before sort data processor executes
        /// </summary>
        event DataProcessorEventHandler OnSortDataProcessingEvent;

        /// <summary>
        /// Occurs after sort data processor executes
        /// </summary>
        event DataProcessorEventHandler OnSortDataProcessedEvent;

        /// <summary>
        /// Occurs before paging data processor executes
        /// </summary>
        event DataProcessorEventHandler OnPagingDataProcessingEvent;

        /// <summary>
        /// Occurs after paging data processor executes
        /// </summary>
        event DataProcessorEventHandler OnPagingDataProcessedEvent;

        /// <summary>
        /// Application entry point method. Executes all data processors.
        /// </summary>
        /// <returns>Processed data as <see cref="ResultModel"/></returns>
        ResultModel Execute();
    }
}