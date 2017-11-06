using System.Collections.Generic;

namespace JQDT.Models
{
    public class Custom
    {
        // Dictionary key: ColumnName, value: {type, value}
        public Dictionary<string, IEnumerable<RangeModel>> Ranges { get; set; }
    }
}