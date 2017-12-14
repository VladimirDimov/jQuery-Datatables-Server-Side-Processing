namespace JQDT.ModelBinders
{
    using System.Collections.Specialized;
    using JQDT.Models;

    /// <summary>
    /// Form data model binder
    /// </summary>
    public interface IFormModelBinder
    {
        /// <summary>
        /// Binds the model.
        /// </summary>
        /// <typeparam name="T">The type of the data collection.</typeparam>
        /// <param name="ajaxForm">The ajax form.</param>
        /// <param name="data">The data.</param>
        /// <returns><see cref="RequestInfoModel"/></returns>
        RequestInfoModel BindModel<T>(NameValueCollection ajaxForm, T data);
    }
}