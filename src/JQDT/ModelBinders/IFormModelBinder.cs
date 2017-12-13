using System.Collections.Specialized;
using JQDT.Models;

namespace JQDT.ModelBinders
{
    public interface IFormModelBinder
    {
        RequestInfoModel BindModel<T>(NameValueCollection ajaxForm, T data);
    }
}