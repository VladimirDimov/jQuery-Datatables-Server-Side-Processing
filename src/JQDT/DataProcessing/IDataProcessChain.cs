namespace JQDT.DataProcessing
{
    using System.Collections.Generic;

    internal interface IDataProcessChain
    {
        IEnumerable<IDataProcess> DataProcessors { get; }

        void AddDataProcessor(IDataProcess dataProcessor);
    }
}