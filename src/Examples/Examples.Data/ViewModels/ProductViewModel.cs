using System;

namespace Examples.Data.ViewModels
{
    public class ProductViewModel
    {
        public ProductModelViewModel ProductModel { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }
}