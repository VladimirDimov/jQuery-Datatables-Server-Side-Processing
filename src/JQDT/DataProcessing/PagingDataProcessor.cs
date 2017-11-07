﻿namespace JQDT.DataProcessing
{
    using System.Linq;
    using JQDT.Models;

    /// <summary>
    /// Paging data processor
    /// </summary>
    /// <seealso cref="JQDT.DataProcessing.DataProcessBase" />
    internal class PagingDataProcessor : DataProcessBase
    {
        /// <summary>
        /// Called when [process data].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="requestInfoModel">The request information model.</param>
        /// <returns>
        ///   <see cref="IQueryable{object}" />
        /// </returns>
        public override IQueryable<object> OnProcessData(IQueryable<object> data, RequestInfoModel requestInfoModel)
        {
            var pagedData = data
                .Skip(requestInfoModel.TableParameters.Start)
                .Take(requestInfoModel.TableParameters.Length);

            return pagedData;
        }
    }
}