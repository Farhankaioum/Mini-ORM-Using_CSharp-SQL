using System;
using System.Data.SqlClient;

namespace Mini_ORM.Library
{
    public interface ISqlDataStore : IDisposable
    {
        SqlDataReader GetAllData(string query);
        SqlDataReader GetById(string query);
        void Insert(string query);
        void Update(string query);
        void Delete(string query);
        string GetLastInsertedId(string query);
    }
}
