# jQuery Datatables Server Side Processing
ASP NET component which adds functionality for automatic server side processing for the famous table plugin for jQuery.

Supports:
- Paging;
- Searching;
- Sorting;
- Custom filters: Less Than, Less than or equal, Greater than, Greater than or equal and Equal;
- Nested objects;

Currently tested with Entity Framework versions 6.0.0 and 6.2.0 and Datatables version 1.10.16.

[How to use with MVC 5](https://github.com/VladimirDimov/jQuery-Datatables-Server-Side-Processing/blob/master/README.Mvc.md)

## How to use on the client side
To use it on the client side the jquery datatables library must be loaded on the browser. Follow the official jQuery Datatables guide for serverside processing [here](https://datatables.net/examples/data_sources/server_side.html).

In order to map the ajax response to the correct columns the columns property must be configure in the configuration object.
### Example:
```js
    var table = $('table').DataTable({
        "proccessing": true,
        "serverSide": true,
        "ajax": {
            url: "path to the data source",
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
```

#### Searching
Read how to use on the client side from [here](https://datatables.net/reference/option/searching)

The searching is performed only on string columns. Therefore the searchable parameter inside the columns property must be set to false. Only the string columns with the searchable parameter set to true will be included in the search.

#### Individual column filtering
Read how to use on the client side from [here](https://datatables.net/examples/api/multi_filter.html).

For all string columns the individual column filter will work as case insensitive search. For other types it will match the exact value. For DateTime types it will match the value with precision to the seconds.

#### Nested objects
Read how to use on the client side from [here](https://datatables.net/examples/ajax/deep.html).

#### Custom filters
To use the predefined custom filters the data property of the configuration object must be configured properly [link](https://datatables.net/reference/option/ajax.data).

The following custom filters are supported on the server side:
- `gt` : greater than;
- `gte` : greater than or equal;
- `lt` : less than;
- `lte` : less than or equal;
- `eq` : eqaul;
##### Example:
```html
<div>
    CreditRating: from <input type="number" id="minCreditRating" value="1" class="reload-table" />
    to <input type="number" id="maxCreditRating" value="3" class="reload-table" />
</div>

<div>
    Business Entity ID = <input type="number" id="businessEntityId" value="" class="reload-table" />
</div>

<div>
    Modified Date = <input type="date" id="modifiedDate" value="" class="reload-table" />
</div>

<div>
    Account Number = <input type="text" id="accountNumber" value="" class="reload-table" />
</div>

<table id="SearchResultTable" class="display" cellspacing="0" width="100">
    <thead>
        <tr>
            <th>BusinessEntityID</th>
            <th>CreditRating</th>
            <th>ActiveFlag</th>
            <th>AccountNumber</th>
            <th>ModifiedDate</th>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <th>BusinessEntityID</th>
            <th>CreditRating</th>
            <th>ActiveFlag</th>
            <th>AccountNumber</th>
            <th>ModifiedDate</th>
        </tr>
    </tfoot>
</table>

@section Scripts {
    <script>


        var table = $('#SearchResultTable').DataTable({
            "proccessing": true,
            "serverSide": true,
            "ajax": {
                url: "@Url.Action("GetVendorsData", "Vendors")",
                type: 'POST',
                "data": function (d) {
                    d.custom = {
                        "filters": {
                            "CreditRating": { "gte": $('#minCreditRating').val(), "lte": $('#maxCreditRating').val() },
                            "BusinessEntityID": { "eq": $('#businessEntityId').val() },
                            "ModifiedDate": { "eq": $('#modifiedDate').val() },
                            "AccountNumber": { "eq": $('#accountNumber').val() },
                        }
                    }
                }
            },
            "language": {
                "search": "",
                "searchPlaceholder": "Search..."
            },
           "columns": [
               { "data": "BusinessEntityID", searchable: false },
               { "data": "CreditRating", searchable: false },
               { "data": "ActiveFlag", "searchable": false },
               { "data": "AccountNumber", searchable: true },
               { "data": "ModifiedDate", "searchable": false},
            ],
           "columnDefs": [
               {
                   "render": function (data, type, row) {
                       date = new Date(parseInt(row.ModifiedDate.replace("/Date(", "").replace(")/", ""), 10));

                       return date.toLocaleDateString();
                   },
                   "targets": 4
               }
           ]

        });

        $('.reload-table').on('change', function () {
            table.ajax.reload();
        });
    </script>
}
```
