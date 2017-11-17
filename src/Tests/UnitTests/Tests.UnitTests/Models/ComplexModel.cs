namespace Tests.UnitTests.Models
{
    using System;

    internal class ComplexModel
    {
        public string String { get; set; }

        public int Integer { get; set; }

        public double Double { get; set; }

        public DateTime DateTime { get; set; }

        public bool Boolean { get; set; }

        public char Char { get; set; }

        public SimpleModel SimpleModel { get; set; }

        public ComplexModel NestedComplexModel { get; set; }
    }
}