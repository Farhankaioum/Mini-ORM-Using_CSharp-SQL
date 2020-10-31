using System.Collections.Generic;
using System.Text;

namespace Mini_ORM.Library
{
    public class SqlQueryGenerator : ISqlQueryGenerator
    {

        public string GetAllQuery(string typeName)
        {
            var query = new StringBuilder($"select * from {typeName} ");

            return query.ToString();
        }

        public string InnerJoinQuery(string mainTableName,
            string innerJoinTableName,
            string columnName)
        {
            var query = new StringBuilder($"inner join {innerJoinTableName} on {mainTableName}.{columnName}={innerJoinTableName}.{mainTableName}{columnName}");

            return query.ToString();
        }

        public string GetByIdQuery(string typeName, string columnName, string value)
        {
            return $"where {columnName}={value}";
        }

        public string InsertQuery(string typeName, List<string> values)
        {
            var query = new StringBuilder($"insert into {typeName} values(");

            foreach (var value in values)
            {
                query.Append($"'{value}'");
                query.Append(",");
            }

            query = query.Remove(query.Length - 1, 1);
            query.Append(");");

            return query.ToString();
        }

        public string UpdateQuery(string typeName, List<(string, string)> ColumnNameAndUpdatedValues, string value)
        {
            var query = new StringBuilder($"update {typeName} set ");

            foreach (var columnNameAndValue in ColumnNameAndUpdatedValues)
            {
                query.Append($"{columnNameAndValue.Item1} = '{columnNameAndValue.Item2}', ");
            }

            query = query.Remove(query.Length - 2, 2);
            query.Append($" where Id = '{value}'");

            return query.ToString();
        }

        public string DeleteQuery(string typeName, string columnName, string value)
        {
            var query = new StringBuilder($"delete from ")
                .Append($"{typeName} ")
                .Append($"where {columnName} = ")
                .Append($"'{value}'");

            return query.ToString();
        }

        public string LastInsertIdQuery()
        {
            var query = "select scope_identity()";
            return query;
        }
    }
}
