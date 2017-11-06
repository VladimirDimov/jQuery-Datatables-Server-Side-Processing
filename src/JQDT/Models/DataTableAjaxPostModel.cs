namespace JQDT.Models
{
    using System.Collections.Generic;

    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }

        public int start { get; set; }

        public int length { get; set; }

        public List<Column> columns { get; set; }

        public Search search { get; set; }

        public List<Order> order { get; set; }

        public Custom Custom { get; set; }
    }
}