namespace Tests.UnitTests.Mocks
{
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.Delegates;
    using JQDT.Models;

    internal class DataProcessorFilterFake<T> : DataProcessBase<T>, IDataFilter
    {
        public IQueryable<T> ProcessedData { get; set; }

        public event DataProcessorEventHandler OnDataProcessingEvent = delegate { };

        public event DataProcessorEventHandler OnDataProcessedEvent = delegate { };

        public IQueryable<T> ProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            this.ProcessedData = data;

            return data;
        }

        protected override IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            this.ProcessedData = data;

            return data;
        }
    }
}