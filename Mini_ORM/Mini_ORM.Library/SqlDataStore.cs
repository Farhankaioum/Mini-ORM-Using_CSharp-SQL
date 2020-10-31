using System.Data.SqlClient;

namespace Mini_ORM.Library
{
    public class SqlDataStore: ISqlDataStore
    {
        private readonly SqlConnection _sqlConnection;

        public SqlDataStore(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        public SqlDataReader GetAllData(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            var data = command.ExecuteReader();

            return data;
        }

        public SqlDataReader GetById(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            var data = command.ExecuteReader();

            return data;
        }

        public void Insert(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void Update(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public void Delete(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public string GetLastInsertedId(string query)
        {
            var command = new SqlCommand(query, _sqlConnection);
            return command.ExecuteScalar().ToString();
        }

        public void Dispose()
        {
            if (_sqlConnection != null)
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }

                _sqlConnection.Dispose();
            }
            
        }
    }
}
