namespace Examples.Mvc.ViewModels
{
    public partial class ProductSubcategoryViewModel
    {
        public string Name { get; set; }

        public virtual ProductCategoryViewModel ProductCategory { get; set; }
    }
}