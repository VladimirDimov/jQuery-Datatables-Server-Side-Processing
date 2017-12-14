namespace Tests.UnitTests.Mocks
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using JQDT.Application;
    using JQDT.DI;
    using JQDT.ModelBinders;

    public class AppMock<T> : ApplicationBase<T>
    {
        public AppMock(IServiceLocator sreviceLocator, IFormModelBinder formModelBinder)
            : base(sreviceLocator, formModelBinder)
        {
        }

        protected override NameValueCollection GetAjaxForm()
        {
            return new NameValueCollection();
        }

        protected override IQueryable<T> GetData()
        {
            return new List<T>().AsQueryable();
        }
    }
}