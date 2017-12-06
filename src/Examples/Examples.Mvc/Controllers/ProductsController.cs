namespace Examples.Mvc.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data;
    using JQDT.MVC;

    public class ProductsController : Controller
    {
        private AdventureWorks context;

        public ProductsController()
        {
            this.context = new Data.AdventureWorks();
        }

        public ActionResult Index()
        {
            return View();
        }

        [JQDataTable]
        public ActionResult GetProductsData()
        {
            var data = this.context.Products.Select(x => new ProductViewModel
            {
                Name = x.Name,
                Color = x.Color,
                ListPrice = x.ListPrice,
                DaysToManufacture = x.DaysToManufacture,
                DiscontinuedDate = x.DiscontinuedDate,
                SellStartDate = x.SellStartDate,
                ReorderPoint = x.ReorderPoint,
                ProductModel = new ProductModelViewModel
                {
                    Name = x.ProductModel.Name,
                    ModifiedDate = x.ProductModel.ModifiedDate
                },
                ProductSubcategory = new ProductSubcategoryViewModel
                {
                    Name = x.ProductSubcategory.Name,
                    ProductCategory = new ProductCategoryViewModel
                    {
                        Name = x.ProductSubcategory.ProductCategory.Name
                    }
                },
                UnitMeasure = new UnitMeasureViewModel
                {
                    Name = x.UnitMeasure.Name
                }
            });

            return this.View(data);
        }
    }

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

    public partial class UnitMeasureViewModel
    {
        public string Name { get; set; }
    }

    public partial class ProductModelViewModel
    {
        public string Name { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    public partial class ProductSubcategoryViewModel
    {
        public string Name { get; set; }

        public virtual ProductCategoryViewModel ProductCategory { get; set; }
    }

    public partial class ProductCategoryViewModel
    {
        public int ProductCategoryID { get; set; }

        public string Name { get; set; }
    }
}