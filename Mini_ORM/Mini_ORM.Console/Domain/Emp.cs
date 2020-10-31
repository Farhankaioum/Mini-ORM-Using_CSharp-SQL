using Mini_ORM.Library;

namespace Mini_ORM.Console.Domain
{
    public class Emp : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Salary Salary { get; set; }
    }

    public class Salary : IEntity<int>
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
