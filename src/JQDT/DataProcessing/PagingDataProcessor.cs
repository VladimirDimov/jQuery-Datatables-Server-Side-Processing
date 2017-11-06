namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System.Linq;

    internal class PagingDataProcessor : DataProcessBase
    {
        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var pagedData = data
                .Skip(requestInfoModel.TableParameters.start)
                .Take(requestInfoModel.TableParameters.length);

            return pagedData;
        }
    }
}