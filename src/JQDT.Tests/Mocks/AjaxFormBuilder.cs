namespace JQDT.Tests.Mocks
{
    using System;
    using System.Collections.Specialized;

    internal class AjaxFormBuilder
    {
        public NameValueCollection BuildForm(Type model)
        {
            var properties = model.GetProperties();
            var form = new NameValueCollection();
            for (int i = 0; i < properties.Length; i++)
            {
                var propName = properties[i].Name;
                form.Add($"columns[{i}][data]:{propName}", propName);
            }

            return form;
        }
    }
}