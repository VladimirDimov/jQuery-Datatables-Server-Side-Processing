namespace Examples.Mvc.ViewModels
{
    using System;
    using Examples.Data;
    public class StoreViewModel
    {
        public int BusinessEntityID { get; set; }

        public string Name { get; set; }

        public int? SalesPersonID { get; set; }

        public string Demographics { get; set; }

        public string rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual BusinessEntity BusinessEntity { get; set; }
    }
}