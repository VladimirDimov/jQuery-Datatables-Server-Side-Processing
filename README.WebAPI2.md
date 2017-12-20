## How to use
Add the `[JQDataTable]` attribute to the controller action which provides the data. Return from the action `OkNegotiatedContentResult` containing IQueryable collection of a strongly typed view model. On the client side configure the table for server side processing according to the jQuery Datatables documentation https://datatables.net/examples/data_sources/server_side.html.

### Example

#### Server
```cs
    public class CustomersController : ApiController
    {
        [JQDataTable]
        public Post GetCustomersData()
        {
            var context = new Data.AdventureWorks();
            var data = context.Customers;

            return this.Ok(data);
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public Person Person { get; set; }
        public Store Store { get; set; }
        ...
        ...
    }

    public class Person
    {
        ...
        public string FirstName { get; set; }
        public string LastName { get; set; }
        ...
        ...
    }

    public class Store
    {
        ...
        public string Name { get; set; }
        ...
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
                url: "api/customers",
                type: 'POST'
            },
            "language": {
                "search": "",
            },
           "columns": [
               { "data": "CustomerID" },
               { "data": "Person.FirstName" },
               { "data": "Person.LastName" },
               { "data": "Store.Name" },
            ]
        });

    </script>
}
```
