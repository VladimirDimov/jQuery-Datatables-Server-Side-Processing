namespace JQDT.Application
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using JQDT.DataProcessing;
    using JQDT.DI;
    using JQDT.ModelBinders;
    using JQDT.Models;

    /// <summary>
    /// Application entry point.
    /// The <see cref="ApplicationBase.Execute(System.Collections.Specialized.NameValueCollection, System.Linq.IQueryable{T})"/> should be called
    /// </summary>
    /// <typeparam name="T">Data Collection Generic Type</typeparam>
    public abstract class ApplicationBase<T>
    {
        private readonly IDependencyResolver dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBase{T}"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public ApplicationBase(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Application entry point method. Should be called from the ActionFilter.
        /// </summary>
        /// <returns><see cref="ResultModel"/></returns>
        public ResultModel Execute()
        {
            ResultModel result = new ResultModel();
            try
            {
                var modelBinder = new FormModelBinder();
                var ajaxForm = this.GetAjaxForm();
                var data = this.GetData();
                var requestModel = modelBinder.BindModel(ajaxForm, data);

                var dataProcessChain = this.GetDataProcessChain(requestModel.Helpers.DataCollectionType);
                var processedData = dataProcessChain.ProcessData(data, requestModel);

                result.Data = processedData.ToList().Select(x => (object)x).ToList();
                result.Draw = requestModel.TableParameters.Draw;
                result.RecordsTotal = data.Count();
                result.RecordsFiltered = this.GetRecordsFiltered(dataProcessChain);
            }
            catch (Exception ex)
            {
                result.Error = this.FormatException(ex);
            }

            return result;
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

            dataProcessChain.AddDataProcessor(this.dependencyResolver.GetSearchDataProcessor<T>());
            dataProcessChain.AddDataProcessor(this.dependencyResolver.GetCustomFiltersDataProcessor<T>());
            dataProcessChain.AddDataProcessor(this.dependencyResolver.GetColumnsFilterDataProcessor<T>());
            dataProcessChain.AddDataProcessor(this.dependencyResolver.GetSortDataProcessor<T>());
            dataProcessChain.AddDataProcessor(this.dependencyResolver.GetPagingDataProcessor<T>());

            return dataProcessChain;
        }
    }
}