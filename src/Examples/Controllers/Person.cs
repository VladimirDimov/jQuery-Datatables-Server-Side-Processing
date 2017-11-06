using System;

namespace WebApplication1.Controllers
{
    public class Person
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public int Age { get; internal set; }

        public string Town { get; internal set; }

        public DateTime StartingDate { get; internal set; }
    }
}