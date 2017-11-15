namespace TestData.Data.Models
{
    using System;

    public class ComplexDataModel
    {
        public string String { get; set; }

        public int Integer { get; set; }

        public double Double { get; set; }

        public DateTime DateTime { get; set; }

        public bool Boolean { get; set; }

        public SimpleDataModel SimpleModel { get; set; }

        public ComplexDataModel ComplexModel { get; set; }
    }
}