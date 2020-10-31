using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_ORM.Library
{
    public class TestModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
