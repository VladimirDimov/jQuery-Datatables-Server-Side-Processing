namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System.Linq;

    internal class PagingDataProcessor : IDataProcess
    {
        public IQueryable<object> ProcessedData { get; set; }

        public IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var pagedData = data
                .Skip(requestInfoModel.TableParameters.start)
                .Take(requestInfoModel.TableParameters.length);

            return pagedData;
        }
    }
}