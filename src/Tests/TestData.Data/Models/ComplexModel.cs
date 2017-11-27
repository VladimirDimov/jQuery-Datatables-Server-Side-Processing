namespace Tests.UnitTests.Models
{
    using System;

    public class ComplexModel
    {
        public string String { get; set; }

        public int Integer { get; set; }

        public double Double { get; set; }

        public DateTime DateTime { get; set; }

        public bool Boolean { get; set; }

        public char Char { get; set; }

        public AllTypesModel SimpleModel { get; set; }

        public ComplexModel NestedComplexModel { get; set; }
    }
}