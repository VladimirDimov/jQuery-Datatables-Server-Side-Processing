namespace JQDT.DataProcessing
{
    using System.Collections.Generic;

    /// <summary>
    /// Chain data processor that calls all containing data processors in the added order.
    /// </summary>
    internal interface IDataProcessChain<T>
    {
        /// <summary>
        /// Gets collection of the containing data processors.
        /// </summary>
        /// <value>
        /// The data processors.
        /// </value>
        IEnumerable<IDataProcess<T>> DataProcessors { get; }

        /// <summary>
        /// Adds a data processor to the execution chain.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        void AddDataProcessor(IDataProcess<T> dataProcessor);
    }
}