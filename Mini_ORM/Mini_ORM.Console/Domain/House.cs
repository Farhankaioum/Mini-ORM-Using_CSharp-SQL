using Mini_ORM.Library;
using System.Collections.Generic;

namespace Mini_ORM.Console.Domain
{
    public class House : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }

    public class Room : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
