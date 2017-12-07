## How to use
Add the `[JQDataTable]` attribute to the ajax controller action. Return 'View(data)' where 'data' is of type IQueryable<>. On the client side configure the table for server side processing acccording to the jQuery Data Tables documentation https://datatables.net/examples/data_sources/server_side.html.

### Example

#### Server
```cs
    public class CustomersController : Controller
    {
        private AdventureWorks context;

        public CustomersController()
        {
            this.context = new Data.AdventureWorks();
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [JQDataTable]
        public ActionResult GetCustomersData()
        {
            var data = this.context.Customers.Select(x => new CustomerViewModel
            {
                CustomerID = x.CustomerID,
                AccountNumber = x.AccountNumber,
                Person = new PersonViewModel
                {
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                },
                Store = new StoreViewModel
                {
                    Name = x.Store.Name,
                }
            });

            return this.View(data);
        }
    }
```

#### Client
```html
<table class="display" cellspacing="0" width="100%">
    <thead>
        <tr>
            <th>Id</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Store Name</th>
        </tr>
    </thead>

    <tfoot>
        <tr>
            <th>Id</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Store Name</th>
        </tr>
    </tfoot>
</table>

@section Scripts {
    <script>

        var table = $('table').DataTable({
            "proccessing": true,
            "serverSide": true,
            "ajax": {
                url: "@Url.Action("GetCustomersData", "Customers")",
                type: 'POST'
            },
            "language": {
                "search": "",
                "searchPlaceholder": "Search..."
            },
           "columns": [
               { "data": "CustomerID", "searchable": false },
               { "data": "Person.FirstName", "searchable": true },
               { "data": "Person.LastName", "searchable": true },
               { "data": "Store.Name", "searchable": true },
            ]
        });

    </script>
}
```