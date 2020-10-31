using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_ORM.Library
{
    public interface ISqlQueryGenerator
    {
        string GetAllQuery(string typeName);
        string InnerJoinQuery(string mainTableName,
            string innerJoinTableName,
            string columnName);
        string GetByIdQuery(string typeName, string columnName, string value);
        string InsertQuery(string typeName, List<string> values);
        string UpdateQuery(string typeName, List<(string, string)> ColumnNameAndUpdatedValues, string value);
        string DeleteQuery(string typeName, string columnName, string value);
        string LastInsertIdQuery();
    }
}
