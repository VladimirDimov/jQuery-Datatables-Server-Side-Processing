namespace Examples.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Examples.Data;
    using Examples.Mvc.ViewModels;
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
}