namespace JQDT.DataProcessing
{
    using JQDT.Models;
    using System;
    using System.Linq;

    internal class CustomFiltersDataProcessor : DataProcessBase
    {
        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var processedData = data.Where(x => true);

            return processedData;
        }
    }
}