using JQDT.Models;
using System.Linq;

namespace JQDT.DataProcessing
{
    internal interface IDataProcess
    {
        IQueryable<object> ProcessedData { get; set; }

        IQueryable<object> ProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel);
    }
}