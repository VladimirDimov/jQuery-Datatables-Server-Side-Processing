namespace Examples.WebApi2.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Examples.Data;
    using Examples.Mvc.ViewModels;
    using JQDT.WebAPI;

    [EnableCors]
    public class ProductsController : ApiController
    {
        private AdventureWorks context;

        public ProductsController()
        {
            this.context = new Data.AdventureWorks();
        }

        [HttpPost]
        [JQDataTable]
        public IHttpActionResult Post()
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

            return this.Ok(data);
        }
    }
}