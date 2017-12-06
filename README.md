# jQuery Datatables Server Side Processing
MVC component which adds functionality for automatic server side processing for the famous table plugin for jQuery.

Supports:
- Paging;
- Searching;
- Sorting;
- Custom filters: Less Than, Less than or equal, Greater than, Greater than or equal and Equal;
- Nested objects;

## How to use
Add the `[JQDataTable]` attribute to the ajax controller action. Return 'View(data)' where 'data' is of type IQueryable<>

### Example

#### Server
```cs
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
```

#### Client
```html
        <table id="myTable" class="display" cellspacing="0" width="100">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>FirstName</th>
                    <th>MiddleName</th>
                    <th>LastName</th>
                    <th>BusinessEntityID</th>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <th>Title</th>
                    <th>FirstName</th>
                    <th>MiddleName</th>
                    <th>LastName</th>
                    <th>BusinessEntityID</th>
                </tr>
            </tfoot>
        </table>

        @section Scripts {
            <script>
                var table = $('#myTable').DataTable({
                    "proccessing": true,
                    "serverSide": true,
                    "ajax": {
                        url: "@Url.Action("GetPeopleData", "AdventureWorks")",
                        type: 'POST'
                    },
                    "language": {
                        "search": "",
                        "searchPlaceholder": "Search..."
                    },
                   "columns": [
                       { "data": "Title" },
                       { "data": "FirstName" },
                       { "data": "MiddleName" },
                       { "data": "LastName"},
                       { "data": "Employee.BusinessEntityID"},
                    ]
                });
            </script>
        }
```
