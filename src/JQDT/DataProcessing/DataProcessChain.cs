using JQDT.Models;
using System.Collections.Generic;
using System.Linq;

namespace JQDT.DataProcessing
{
    internal class DataProcessChain : DataProcessBase, IDataProcessChain
    {
        private ICollection<IDataProcess> dataProcessors;
        private Dictionary<string, object> intermidiateResults;

        public DataProcessChain()
        {
            this.dataProcessors = new LinkedList<IDataProcess>();
            this.intermidiateResults = new Dictionary<string, object>();
        }

        public IEnumerable<IDataProcess> DataProcessors
        {
            get
            {
                return this.dataProcessors;
            }
        }

        public IDictionary<string, object> IntermidiateResults
        {
            get
            {
                return this.intermidiateResults;
            }
        }

        public void AddDataProcessor(IDataProcess dataProcessor)
        {
            this.dataProcessors.Add(dataProcessor);
        }

        public void AddIntermidiateResult(string key, object value)
        {
            this.intermidiateResults.Add(key, value);
        }

        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var currentDataState = data;

            foreach (var dataProcessor in this.dataProcessors)
            {
                var processedData = dataProcessor.ProcessData(currentDataState, requestInfoModel);
                currentDataState = processedData;
            }

            return currentDataState;
        }
    }
}