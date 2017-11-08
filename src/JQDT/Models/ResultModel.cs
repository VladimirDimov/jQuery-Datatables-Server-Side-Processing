using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JQDT.Models
{
    class ResultModel
    {
        public int Draw { get; internal set; }
        public int RecordsTotal { get; internal set; }
        public object RecordsFiltered { get; internal set; }
        public List<object> Data { get; internal set; }
    }
}
