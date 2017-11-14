describe("Global filter", function () {

  beforeEach(function () {

  });

  describe('Test simple case', function () {
    var data = null;
    var datatable;
    var onDraw;
    var expectedPageSize = 53;

    beforeEach(function (done) {
      var table = $('#table-simple').DataTable({
        "proccessing": true,
        "serverSide": true,
        "ajax": {
          url: "http://localhost:60487/home/getsimpledata",
          type: 'POST',
          data: function (data) {
            data.length = expectedPageSize;
          },
          "dataSrc": function (json) {
            data = json.data;
            datatable = table;
            done();
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
    });

    it("Page size should be proper", function (done) {
      var actualPageSize = data.length;
      expect(actualPageSize).toBe(expectedPageSize);
      done();
    });
  })
});
