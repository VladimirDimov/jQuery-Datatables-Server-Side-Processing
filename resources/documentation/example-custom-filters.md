### Custom filters example
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
