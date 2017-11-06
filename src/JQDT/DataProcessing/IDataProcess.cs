using JQDT.Models;
using System.Linq;

namespace JQDT.DataProcessing
{
    internal interface IDataProcess
    {
        IQueryable<object> ProcessedData { get; }

        IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel);
    }
}