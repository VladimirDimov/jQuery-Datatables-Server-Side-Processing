# jQuery Datatables Server Side Processing
MVC plugin for server side processing for jQuery datatables.

Supports:
- Searching.
- Filter by range.
- Filter by fixed value

## How to use
Add the `[JQDataTable]` attribute to the ajax controller action. Return 'View(data)' where 'data' is of type IQueryable<>

### Example
`
        private ApplicationDbContext context;
        
        [JQDataTable]
        public ActionResult GetVendorsData()
        {
            var data = this.context.Employees.Select(x => new
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = new AddressViewModel
                {
                    Country = x.Address.Country,
                    City = x.Address.City
                }
            });

            return this.View(data);
        }
`
