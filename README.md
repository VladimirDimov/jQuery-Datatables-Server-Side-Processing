# jQuery Datatables Server Side Processing
ASP NET component which adds functionality for automatic server side processing for the famous table plugin for jQuery. It uses IQueryable<T> interface to construct query expressions to your data collection which can be processed by an ORM like Entity Framework.

Supports:
- Paging;
- Searching;
- Sorting;
- Custom filters: Less Than, Less than or equal, Greater than, Greater than or equal and Equal;
- Nested objects;
- Option to add custom server side logic using the provided extensibility points;

Currently tested with Entity Framework versions 6.0.0 and 6.2.0 and Datatables version 1.10.16.

## Install
#### MVC 5
[Install nuget package](https://www.nuget.org/packages/jQDataTables.ServerSide.MVC5/)

`Install-Package jQDataTables.ServerSide.MVC5`

## How to use on the server side

[How to use with MVC 5](/resources/documentation/README.Mvc.md)

[How to use with Web Api 2](/resources/documentation/README.WebAPI2.md)

## How to use on the client side
For how to install the jQuery Datatables plugin refer to the official documentation [here](https://datatables.net/manual/installation) 

[Here](https://datatables.net/examples/data_sources/server_side.html) you can find how to configure for server side processing.

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
        "columns": [
            { "data": "CustomerID" },
            { "data": "Person.FirstName" },
            { "data": "Person.LastName" },
            { "data": "Store.Name" },
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

[Custom filters example](/resources/documentation/example-custom-filters.md)

#### Extensibility Points
You can insert custom logic at any point of the data processing pipe line. The following extensibility points are provided (in order of execution):
- OnDataProcessing - Called before all data processors execute.
- OnSearchDataProcessing - Called before search data processor executes.
- OnSearchDataProcessed - Called after search data processor executes.
- OnCustomFiltersDataProcessing - Called before custom filters data processor executes.
- OnCustomFiltersDataProcessed - Called after custom filters data processor executes.
- OnColumnsFilterDataProcessing - Called before columns filters data processor executes.
- OnColumnsFilterDataProcessed - Called after columns filters data processor executes.
- OnSortDataProcessing - Called before sort data processor executes.
- OnSortDataProcessed - Called after sort data processor executes.
- OnPagingDataProcessing - Called before paging data processor executes.
- OnPagingDataProcessed - Called after paging data processor executes.
- OnDataProcessed - Called after all data processors execute.

##### How to use the extensibility points?
Create a new class which inherrits from `JQDataTableAttribute` class. The extensibility points are implemented as virtual methods which can be overriden.
###### Example
```cs
MyCustomDataTableAttribute : JQDataTableAttribute
{
    // This method will modify the collection and only the customers 
    // with even ID number will be included in the result
    public override void OnSearchDataProcessing(ref object data, RequestInfoModel requestInfoModel)
    {
        var dataAsQueryable = data as IQueryable<CustomerViewModel>;
        data = dataAsQueryable.Where(x => x.CustomerID % 2 == 0);
    }
}
```

Apply the new attribute on the controller action:
```cs

CustomersController : Controller
{
    [MyCustomDataTableAttribute]
    public ActionResult GetCustomersData()
    {
        ...
        return this.View(data);
    }
}
```