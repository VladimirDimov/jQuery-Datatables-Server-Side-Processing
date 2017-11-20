﻿namespace JQDT.Application
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using JQDT.DataProcessing;
    using JQDT.DataProcessing.ColumnsFilter;
    using JQDT.DataProcessing.Common;
    using JQDT.DataProcessing.FilterDataProcessor;
    using JQDT.ModelBinders;
    using JQDT.Models;

    /// <summary>
    /// Application entry point.
    /// The <see cref="ApplicationBase.Execute(System.Collections.Specialized.NameValueCollection, System.Linq.IQueryable{T})"/> should be called
    /// </summary>
    /// <typeparam name="T">Data Collection Generic Type</typeparam>
    internal abstract class ApplicationBase<T>
    {
        /// <summary>
        /// Application entry point method. Should be called from the ActionFilter.
        /// </summary>
        /// <returns><see cref="ResultModel"/></returns>
        public ResultModel Execute()
        {
            ResultModel resultModel = null;
            //try
            //{
            var modelBinder = new FormModelBinder();
            var ajaxForm = this.GetAjaxForm();
            var data = this.GetData();
            var requestModel = modelBinder.BindModel(ajaxForm, data);

            var dataProcessChain = this.GetDataProcessChain(requestModel.Helpers.DataCollectionType);
            var processedData = dataProcessChain.ProcessData(data, requestModel);
            resultModel = new ResultModel
            {
                Draw = requestModel.TableParameters.Draw,
                RecordsTotal = data.Count(),
                RecordsFiltered = this.GetRecordsFiltered(dataProcessChain),
                Data = processedData.ToList().Select(x => (object)x).ToList()
            };
            //}
            //catch (Exception ex)
            //{
            //    resultModel = new ResultModel
            //    {
            //        Error = this.FormatException(ex)
            //    };
            //}

            return resultModel;
        }

        /// <summary>
        /// Gets the ajax form as <see cref="NameValueCollection"/> from the request context.
        /// </summary>
        /// <returns>form data as <see cref="NameValueCollection"/></returns>
        protected abstract NameValueCollection GetAjaxForm();

        /// <summary>
        /// Gets the data collection as <see cref="IQueryable{T}"/> from the request context.
        /// </summary>
        /// <returns>Data collection as <see cref="IQueryable{T}"/></returns>
        protected abstract IQueryable<T> GetData();

        private string FormatException(Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendLine(ex.Message);
            if (ex.HelpLink != null)
            {
                builder.AppendLine($"Help Link: {ex.HelpLink}");
            }

            return builder.ToString();
        }

        private int GetRecordsFiltered(IDataProcess<T> dataProcessChain)
        {
            return
                ((IDataProcessChain<T>)dataProcessChain)
                    .DataProcessors
                    .Last(p => typeof(IDataFilter).IsAssignableFrom(p.GetType()))
                    .ProcessedData.Count();
        }

        private IDataProcess<T> GetDataProcessChain(Type dataCollectionType)
        {
            var dataProcessChain = new DataProcessChain<T>();
            IFilterDataProcessorBridge filterDataProcessorBridge = null;
            if (dataCollectionType.Name == "DbQuery`1")
            {
                filterDataProcessorBridge = new FilterDataProcessorDbQueryBridge();
            }
            else if (dataCollectionType.Name == "EnumerableQuery`1")
            {
                filterDataProcessorBridge = new FilterDataProcessorEnumerableQueryBridge();
            }

            dataProcessChain.AddDataProcessor(
                new FilterDataProcessor<T>(new FiltersCommonProcessor(filterDataProcessorBridge)));
            dataProcessChain.AddDataProcessor(new CustomFiltersDataProcessor<T>());
            dataProcessChain.AddDataProcessor(new ColumnsFilterDataProcessor<T>(
                new FiltersCommonProcessor(filterDataProcessorBridge)));
            dataProcessChain.AddDataProcessor(new SortDataProcessor<T>());
            dataProcessChain.AddDataProcessor(new PagingDataProcessor<T>());

            return dataProcessChain;
        }
    }
}