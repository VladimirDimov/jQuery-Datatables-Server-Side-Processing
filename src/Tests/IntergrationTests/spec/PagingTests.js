describe("Paging", function () {
  var data = null;
  var datatable;

  beforeEach(function () {
    if (datatable) {
      datatable.destroy();
    }
  });

  function setTable(done, pageSize, url) {
    var table = $('#table-simple').DataTable({
      "fnDrawCallback": function (oSettings) {
        done();
      },
      "proccessing": true,
      "serverSide": true,
      "ajax": {
        url: url || globalConstants.serverUrl + "/home/getsimpledata",
        type: 'POST',
        data: function (data) {
          data.length = pageSize || data.length;
        },
        "dataSrc": function (json) {
          data = json.data;
          datatable = table;
          // done();

          return json.data;
        }
      },
      "columns": [
        { "data": "String" },
        { "data": "Integer" },
        { "data": "Double" },
        { "data": "DateTime" },
        { "data": "Boolean" }
      ]
    });
  }

  describe('Should work properly with explicitly given page size', function () {
    var expectedPageSize = 53;

    beforeEach(function (done) {
      setTable(done, expectedPageSize);
    });

    it("Page size should be proper", function (done) {
      var rows = datatable.data();
      var actualPageSize = rows.length;
      expect(actualPageSize).toBe(expectedPageSize);
      done();
    });
  });

  describe('Should work properly with default page size', function () {
    beforeEach(function (done) {
      setTable(done);
    });

    it("Page size should be proper", function (done) {
      var defaultLength = datatable.ajax.params().length;
      var rows = datatable.data();
      var actualPageSize = rows.length;
      expect(actualPageSize).toBe(defaultLength);
      done();
    });
  });

  describe('Should work properly if the page size is bigger than the number of rows in the result', function () {
    var rowsNumber = 5

    beforeEach(function (done) {
      setTable(done, 10, globalConstants.serverUrl + "/home/getsimpledata?take=" + rowsNumber);
    });

    it("Page size should be proper", function (done) {
      var defaultLength = datatable.ajax.params().length;
      var rows = datatable.data();
      var actualPageSize = rows.length;
      expect(actualPageSize).toBe(rowsNumber);
      done();
    });
  });
});
