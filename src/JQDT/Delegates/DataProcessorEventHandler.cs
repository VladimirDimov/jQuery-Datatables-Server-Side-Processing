namespace JQDT.Delegates
{
    using JQDT.Models;

    /// <summary>
    /// Occurs on data process event
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="requestInfoModel">The request information model.</param>
    public delegate void DataProcessorEventHandler(ref object data, RequestInfoModel requestInfoModel);
}