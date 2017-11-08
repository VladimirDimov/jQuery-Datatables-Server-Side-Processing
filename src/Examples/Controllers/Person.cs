using System;

namespace WebApplication1.Controllers
{
    public class Person
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public int Age { get; internal set; }

        public Address Address { get; set; }

        public DateTime StartingDate { get; internal set; }
    }

    public class Address
    {
        public string City { get; set; }

        public string Country { get; set; }

        public Street Street { get; set; }
    }

    public class Street
    {
        public string Name { get; set; }

        public int Number { get; set; }
    }
}