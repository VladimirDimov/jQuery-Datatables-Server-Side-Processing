namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Paging data processor
    /// </summary>
    /// <typeparam name="T">Generic data model type.</typeparam>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class PagingDataProcessor<T> : DataProcessBase<T>
    {
        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{T}" />
        /// </returns>
        protected override IQueryable<T> OnProcessData(IQueryable<T> data, RequestInfoModel requestInfoModel)
        {
            if (requestInfoModel.TableParameters.Length < 0)
            {
                return data;
            }

            var pagedData = data
                .Skip(requestInfoModel.TableParameters.Start)
                .Take(requestInfoModel.TableParameters.Length);

            return pagedData;
        }
    }
}