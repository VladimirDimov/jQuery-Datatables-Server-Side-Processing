namespace Examples.Mvc.ViewModels
{
    using System;
    public partial class ProductViewModel
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public short ReorderPoint { get; set; }

        public decimal ListPrice { get; set; }

        public int DaysToManufacture { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public virtual UnitMeasureViewModel UnitMeasure { get; set; }

        public virtual ProductModelViewModel ProductModel { get; set; }

        public virtual ProductSubcategoryViewModel ProductSubcategory { get; set; }
    }
}