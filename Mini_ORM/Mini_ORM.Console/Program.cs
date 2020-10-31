using Mini_ORM.Console.Domain;
using Mini_ORM.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_ORM.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-812DOMC;Database=aspnetb4;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sqlQueryGenerator = new SqlQueryGenerator();
            var sqlDataStore = new SqlDataStore(connectionString);


            // GetById data retrive
            //var dataOpeartion = new DataOperation<House,int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //var data = dataOpeartion.GetById(14);
            //System.Console.WriteLine(data.Id + " " + data.Name + " " );

            //System.Console.WriteLine("Inner object");
            //foreach (var subItem in data.Rooms)
            //{
            //    System.Console.WriteLine(subItem.Name);
            //}

            //  All Data Retrive 
            //var dataOperation = new DataOperation<House, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //var datas = dataOperation.GetAll();
            //foreach (var data in datas)
            //{
            //    System.Console.WriteLine(data.Id + " " + data.Name + " ");
            //    System.Console.WriteLine("Inner object");

            //    foreach (var innerItem in data.Rooms)
            //    {
            //        System.Console.WriteLine(innerItem.Id + " " + innerItem.Name);
            //    }
            //    System.Console.WriteLine();
            //}

            //// update method test
            //var dataOperation = new DataOperation<House, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //dataOperation.Update(new House
            //{
            //    Id = 18,
            //    Name = "Next House",
            //    Rooms = new List<Room>
            //    {
            //        new Room { Id = 10, Name = "Update Next VIP"},
            //        new Room { Id = 11, Name = "Update Next Non-VIP"}
            //    }
            //});

            //var dataOperation = new DataOperation<Emp, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //dataOperation.Update(new Emp
            //{
            //    Id = 50,
            //    Name = "Mr. One",
            //    Salary = new Salary { Id = 4, Amount = 105000 }
            //});


            //var dataOperation = new DataOperation<TestModel, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //dataOperation.Update(new TestModel
            //{
            //    Id = 5,
            //    Name = "DBTest",
            //    Age = 25
            //});

            //// delete method test
            //var dataOperation = new DataOperation<TestModel, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //dataOperation.Delete(new TestModel
            //{
            //    Id = 24,
            //    Name = "Next step"
            //});

            // without relation for insert
            //var dataOperation = new DataOperation<TestModel, int>(connectionString, sqlQueryGenerator, sqlDataStore);
            //var testObject = new TestModel
            //{
            //    Name = "Next step",
            //    Age = 2
            //};
            //dataOperation.Insert(testObject);

            // one to one relation for insert
            //var dataOperation = new DataOperation<Emp, int>(connectionString, sqlQueryGenerator, sqlDataStore);

            //var testObject = new Emp
            //{
            //    Name = "Mr. New",
            //    Salary = new Salary { Amount = 95000 }
            //};
            //dataOperation.Insert(testObject);

            // one to many relation for insert
            //var dataOperation = new DataOperation<House, int>(connectionString, sqlQueryGenerator, sqlDataStore);

            //var house = new House
            //{
            //    Name = "Next House",
            //    Rooms = new List<Room>
            //    {
            //        new Room { Name = "Next VIP"},
            //        new Room { Name = "Next Non-VIP"}
            //    }
            //};
            //dataOperation.Insert(house);


            // # for get value #
            //var dataOperation = new DataOperation<House>(connectionString);
            //var data = dataOperation.GetAll();

            //System.Console.WriteLine("One to Many Relationship Data: ");
            //foreach (var item in data)
            //{
            //    System.Console.WriteLine(item.Id + " " + item.Name + " ");
            //}
        }
    }
}
