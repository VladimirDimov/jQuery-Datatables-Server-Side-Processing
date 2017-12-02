using System.Collections;
using System.Collections.Generic;
using TestData.Models;

namespace Tests.SeleniumTests.Models
{
    public class MinMaxRandomModel
    {
        public object Min { get; set; }

        public object Max { get; set; }

        IEnumerable<AllTypesModel> Data { get; set; }
    }
}