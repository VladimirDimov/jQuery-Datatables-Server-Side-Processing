namespace JQDT.Application
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using JQDT.DataProcessing;
    using JQDT.ModelBinders;
    using JQDT.Models;

    /// <summary>
    /// Application entry point.
    /// The <see cref="ApplicationBase.Execute(System.Collections.Specialized.NameValueCollection, System.Linq.IQueryable{object})"/> should be called
    /// from the ActionFilter.
    /// </summary>
    internal abstract class ApplicationBase
    {
        /// <summary>
        /// Application entry point method. Should be called from the ActionFilter.
        /// </summary>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data.</param>
        /// <returns><see cref="ResultModel"/></returns>
        public ResultModel Execute()
        {
            ResultModel resultModel = null;
            try
            {
                var modelBinder = new FormModelBinder();
                var ajaxForm = this.GetAjaxForm();
                var data = this.GetData();
                var requestModel = modelBinder.BindModel(ajaxForm, data);

                var dataProcessChain = this.GetDataProcessChain();
                var processedData = dataProcessChain.ProcessData(data, requestModel);
                resultModel = new ResultModel
                {
                    Draw = requestModel.TableParameters.Draw,
                    RecordsTotal = data.Count(),
                    RecordsFiltered = this.GetRecordsFiltered(dataProcessChain),
                    Data = processedData.ToList()
                };
            }
            catch (Exception ex)
            {
                resultModel = new ResultModel
                {
                    Error = this.FormatException(ex)
                };
            }

            return resultModel;
        }

        protected abstract NameValueCollection GetAjaxForm();

        protected abstract IQueryable<object> GetData();

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
            dataProcessChain.AddDataProcessor(new ColumnsFilterDataProcessor());
            dataProcessChain.AddDataProcessor(new SortDataProcessor());
            dataProcessChain.AddDataProcessor(new PagingDataProcessor());

            return dataProcessChain;
        }
    }
}