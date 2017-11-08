﻿namespace JQDT
{
    using System.Collections.Specialized;
    using System.Linq;
    using JQDT.DataProcessing;
    using JQDT.ModelBinders;
    using JQDT.Models;

    /// <summary>
    /// Application entry point.
    /// The <see cref="Application.Execute(System.Collections.Specialized.NameValueCollection, System.Linq.IQueryable{object})"/> should be called
    /// from the ActionFilter.
    /// </summary>
    internal class Application
    {
        /// <summary>
        /// Application entry point method. Should be called from the ActionFilter.
        /// </summary>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data.</param>
        /// <returns><see cref="ResultModel"/></returns>
        public ResultModel Execute(NameValueCollection ajaxForm, IQueryable<object> data)
        {
            var modelBinder = new FormModelBinder();
            var requestModel = modelBinder.BindModel(ajaxForm, data);

            var dataProcessChain = this.GetDataProcessChain();
            var processedData = dataProcessChain.ProcessData(data, requestModel);
            var resultModel = new ResultModel
            {
                Draw = requestModel.TableParameters.Draw,
                RecordsTotal = data.Count(),
                RecordsFiltered = this.GetRecordsFiltered(dataProcessChain),
                Data = processedData.ToList()
            };

            return resultModel;
        }

        private object GetRecordsFiltered(IDataProcess dataProcessChain)
        {
            return
                ((IDataProcessChain)dataProcessChain)
                    .DataProcessors
                    .First(p => p.GetType() == typeof(CustomFiltersDataProcessor))
                    .ProcessedData.Count();
        }

        private IDataProcess GetDataProcessChain()
        {
            var dataProcessChain = new DataProcessChain();
            dataProcessChain.AddDataProcessor(new FilterDataProcessor());
            dataProcessChain.AddDataProcessor(new CustomFiltersDataProcessor());
            dataProcessChain.AddDataProcessor(new SortDataProcessor());
            dataProcessChain.AddDataProcessor(new PagingDataProcessor());

            return dataProcessChain;
        }
    }
}