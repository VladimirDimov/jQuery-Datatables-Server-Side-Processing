namespace Tests.UnitTests.Models
{
    using System;

    internal class SimpleModel
    {
        public string String { get; set; }

        public int Integer { get; set; }

        public double Double { get; set; }

        public DateTime DateTime { get; set; }

        public bool Boolean { get; set; }

        public char Char { get; set; }

        public char? CharNullable { get; set; }
    }
}