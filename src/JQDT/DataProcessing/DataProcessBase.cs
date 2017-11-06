namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System.Linq;

    public abstract class DataProcessBase : IDataProcess
    {
        private IQueryable<object> processedData;

        public IQueryable<object> ProcessedData
        {
            get
            {
                return this.processedData;
            }
        }

        public IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            this.processedData = this.OnProcessData(data, requestInfoModel);

            return this.ProcessedData;
        }

        public abstract IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel);
    }
}