namespace Tests.UnitTests.Mocks
{
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.Delegates;
    using JQDT.Models;

    internal class DataProcessorMockModel<T> : IDataProcess<T>
    {
        public IQueryable<T> ProcessedData { get; set; }

        public event DataProcessorEventHandler OnDataProcessingEvent = delegate { };

        public event DataProcessorEventHandler OnDataProcessedEvent = delegate { };

        public virtual IQueryable<T> ProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            this.ProcessedData = data;

            return data;
        }
    }
}