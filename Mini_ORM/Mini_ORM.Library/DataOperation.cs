using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mini_ORM.Library
{
    public class DataOperation<TEntity, TKey> : IDataOperation<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly ISqlQueryGenerator _queryGenerator;
        private readonly ISqlDataStore _sqlDataStore;
        private Type _type;

        string namespaceName = string.Empty;
        string assembly = string.Empty;


        public DataOperation(
            string connectionString,
            ISqlQueryGenerator queryGenerator,
            ISqlDataStore sqlDataStore)
        {
            _type = typeof(TEntity);

            namespaceName = _type.Namespace;
            assembly = _type.Assembly.FullName;

            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
            _queryGenerator = queryGenerator;
            _sqlDataStore = sqlDataStore;
        }

        public IList<TEntity> GetAll()
        {
            var data = new List<TEntity>();

            var query = new StringBuilder(_queryGenerator.GetAllQuery(_type.Name));

            var properties = _type.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsClass
                    && property.PropertyType.Assembly.FullName == _type.Assembly.FullName)
                {
                    query.Append(_queryGenerator.InnerJoinQuery(_type.Name, property.PropertyType.Name, "Id"));
                }

            }

            using (var reader = _sqlDataStore.GetAllData(query.ToString()))
            {
                while (reader.Read())
                {
                    var  instance = Activator.CreateInstance(_type);

                    foreach (var property in properties)
                    {
                        if (property.PropertyType.IsClass
                            && property.PropertyType.Assembly.FullName == _type.Assembly.FullName)
                        {
                            var subInstance = Activator.CreateInstance(property.PropertyType);
                            var subProperties = property.PropertyType.GetProperties();

                            foreach (var subProp in subProperties)
                            {
                                subProp.SetValue(subInstance, reader[subProp.Name]);
                            }

                            property.SetValue(instance, subInstance);
                        }

                        else if (property.PropertyType.IsGenericType)
                        {
                            var subInstance = Activator.CreateInstance(property.PropertyType);
                            IList listInstance = (IList)subInstance;

                            var subPropertieTypeName = property.PropertyType.GetGenericArguments()[0].Name;
                            
                            var itemId = reader[0];

                            var subQuery = _queryGenerator.GetAllQuery(subPropertieTypeName) + _queryGenerator.GetByIdQuery(_type.Name, "Id", itemId.ToString());
                            
                            using (var subReader = _sqlDataStore.GetAllData(subQuery))
                            {
                                var ssType = Type.GetType($"{namespaceName}.{subPropertieTypeName},{assembly}");
                                var ssubProps = ssType.GetProperties();

                                while (subReader.Read())
                                {
                                    var ssInstance = Activator.CreateInstance(ssType);

                                    foreach (var ssProp in ssubProps)
                                    {
                                        var value = subReader[ssProp.Name];
                                        ssProp.SetValue(ssInstance, subReader[ssProp.Name]);
                                    }

                                    listInstance.Add(ssInstance);
                                }
                                property.SetValue(instance, listInstance);
                            }
                        }
                        else
                        {
                            property.SetValue(instance, reader[property.Name]);
                        }
                    }

                    data.Add((TEntity) instance);
                }
            }

            return data.ToList();
        }

        public TEntity GetById(TKey id)
        {
            var data = new List<TEntity>();

            var query = new StringBuilder(_queryGenerator.GetAllQuery(_type.Name));

            var properties = _type.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsClass
                    && property.PropertyType.Assembly.FullName == _type.Assembly.FullName)
                {
                    query.Append(_queryGenerator.InnerJoinQuery(_type.Name, property.PropertyType.Name, "Id"));
                }
            }

            query.Append(" ");
            query.Append(_queryGenerator.GetByIdQuery(_type.Name, "Id", id.ToString()));
            

            var instance = Activator.CreateInstance(_type);

            using (var reader = _sqlDataStore.GetById(query.ToString()))
            {
                  reader.Read();

                    foreach (var property in properties)
                    {
                        if (property.PropertyType.IsClass
                            && property.PropertyType.Assembly.FullName == _type.Assembly.FullName)
                        {
                            var subInstance = Activator.CreateInstance(property.PropertyType);
                            var subProperties = property.PropertyType.GetProperties();

                            foreach (var subProp in subProperties)
                            {
                                subProp.SetValue(subInstance, reader[subProp.Name]);
                            }

                            property.SetValue(instance, subInstance);

                        }

                        else if (property.PropertyType.IsGenericType)
                        {
                            var subInstance = Activator.CreateInstance(property.PropertyType);
                            IList listInstance = (IList)subInstance;

                            var subPropertieTypeName = property.PropertyType.GetGenericArguments()[0].Name;

                            var itemId = reader[0];

                            var subQuery = _queryGenerator.GetAllQuery(subPropertieTypeName) + _queryGenerator.GetByIdQuery(_type.Name, $"{_type.Name}Id", itemId.ToString());

                            using (var subReader = _sqlDataStore.GetById(subQuery))
                            {
                                var ssType = Type.GetType($"{namespaceName}.{subPropertieTypeName},{assembly}");
                                var ssubProps = ssType.GetProperties();

                                while (subReader.Read())
                                {
                                    var ssInstance = Activator.CreateInstance(ssType);

                                    foreach (var ssProp in ssubProps)
                                    {
                                        var value = subReader[ssProp.Name];
                                        ssProp.SetValue(ssInstance, subReader[ssProp.Name]);
                                    }

                                    listInstance.Add(ssInstance);
                                }
                                property.SetValue(instance, listInstance);
                            }
                        }
                        else
                        {
                            property.SetValue(instance, reader[property.Name]);
                        }
                    }
            }

            return (TEntity)instance;
        }

        public void Insert(TEntity item)
        {
            var query = string.Empty;
            var columnsValues = new List<string>();

            var properties = _type.GetProperties();

            foreach (var property in properties)
            {
                if (property.Name.ToUpper() != "Id".ToUpper() && 
                    property.PropertyType.Assembly.FullName != _type.Assembly.FullName &&
                    !property.PropertyType.IsGenericType)
                {
                    columnsValues.Add(property.GetValue(item).ToString());
                }
            }

            query = _queryGenerator.InsertQuery(_type.Name, columnsValues);

            _sqlDataStore.Insert(query);

           query = _queryGenerator.LastInsertIdQuery();
           var lastInsertedId = Convert.ToInt32(_sqlDataStore.GetLastInsertedId(query));

            #region working for one to one relationship
            var classTypeProps = _type.GetProperties()
                .Where(p => p.PropertyType.IsClass &&
                p.PropertyType.Assembly.FullName == _type.Assembly.FullName);

            if (classTypeProps != null)
            {
                var subQuery = string.Empty;
                var subColumnsValues = new List<string>();

                foreach (var classProp in classTypeProps)
                {
                    var classTypePropertyName = classProp.Name;
                    var subType = Type.GetType($"{namespaceName}.{classTypePropertyName},{assembly}");

                    var subPropertiesValue = classProp.GetValue(item);

                    var subPropertyProperties = classProp.PropertyType.GetProperties();
                    for (int i = 1; i < subPropertyProperties.Length; i++)
                    {
                        var value = subPropertyProperties[i].GetValue(subPropertiesValue);
                        subColumnsValues.Add(value.ToString());
                    }

                    subColumnsValues.Add(lastInsertedId.ToString());

                    subQuery = _queryGenerator.InsertQuery(classTypePropertyName, subColumnsValues);
                    _sqlDataStore.Insert(subQuery);
                }
            }
            #endregion

            #region working for one to many relationship
            var listOfClassTypeProp = _type.GetProperties().Where(p => p.PropertyType.IsGenericType);

            if (listOfClassTypeProp != null)
            {
                var listOfAllQuery = new StringBuilder();
                foreach (var listOfClassProps in listOfClassTypeProp)
                {
                    var listdata = (IList)listOfClassProps.GetValue(item);

                    foreach (var sListData in listdata)
                    {
                        var typeName = sListData.GetType().Name;
                        var subType = Type.GetType($"{namespaceName}.{typeName},{assembly}");

                        var subTypePropertiesValues = new List<string>();


                        var subProp = subType.GetProperties();
                        for (int i = 1; i < subProp.Length; i++)
                        {
                            var value = subProp[i].GetValue(sListData);
                            subTypePropertiesValues.Add(value.ToString());
                        }

                        subTypePropertiesValues.Add(lastInsertedId.ToString());
                        listOfAllQuery.Append($"{_queryGenerator.InsertQuery(typeName, subTypePropertiesValues)};");
                        
                    }

                    _sqlDataStore.Insert(listOfAllQuery.ToString());
                    listOfAllQuery.Clear();
                   
                }
            }
            #endregion


        }

        public void Update(TEntity item)
        {
            var query = string.Empty;
            var columnsNameAndValues = new List<(string, string)>();

            int itemId = (int)item.GetType().GetProperty("Id").GetValue(item);

            var properties = _type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name.ToUpper() != "Id".ToUpper() &&
                    property.PropertyType.Assembly.FullName != _type.Assembly.FullName &&
                    !property.PropertyType.IsGenericType)
                {
                    columnsNameAndValues.Add((property.Name, property.GetValue(item).ToString()));
                }
            }

            query = _queryGenerator.UpdateQuery(_type.Name, columnsNameAndValues, itemId.ToString());

            _sqlDataStore.Update(query);

            #region working for one to one relationship data update
            var classTypeProps = _type.GetProperties()
                .Where(p => p.PropertyType.IsClass &&
                p.PropertyType.Assembly.FullName == _type.Assembly.FullName);

            if (classTypeProps != null)
            {
                var subQuery = string.Empty;

                foreach (var classProp in classTypeProps)
                {
                    var classTypePropertyName = classProp.Name;
                    var subType = Type.GetType($"{namespaceName}.{classTypePropertyName},{assembly}");

                    var subPropertiesValue = classProp.GetValue(item);

                    var subPropertyProperties = classProp.PropertyType.GetProperties();
                    var subItemId = subPropertyProperties[0].GetValue(subPropertiesValue);

                    var subQueryColumnNameAndValue = new List<(string, string)>();

                    for (int i = 1; i < subPropertyProperties.Length; i++)
                    {
                        var name = subPropertyProperties[i].Name;
                        var value = subPropertyProperties[i].GetValue(subPropertiesValue);

                        subQueryColumnNameAndValue.Add((name, value.ToString()));
                    }

                    subQuery = _queryGenerator.UpdateQuery(classTypePropertyName, subQueryColumnNameAndValue, subItemId.ToString());
                    _sqlDataStore.Update(subQuery);
                }
            }

            #endregion

            #region working for one to many relationship data update
            var listOfClassTypeProp = _type.GetProperties().Where(p => p.PropertyType.IsGenericType);
            if (listOfClassTypeProp != null)
            {
                var listOfAllSubQuery = new StringBuilder();
                foreach (var listOfClassProps in listOfClassTypeProp)
                {
                    var listOfData = (IList)listOfClassProps.GetValue(item);
                    var subQuery = string.Empty;

                    foreach (var sListData in listOfData)
                    {
                        var typeName = sListData.GetType().Name;
                        var subType = Type.GetType($"{namespaceName}.{typeName},{assembly}");
                        var subTypeColumnsNameAndValue = new List<(string, string)>();

                        var subProp = subType.GetProperties();

                        var subItemId = subProp[0].GetValue(sListData);

                        for (int i = 1; i < subProp.Length; i++)
                        {
                            var name = subProp[i].Name;
                            var value = subProp[i].GetValue(sListData);

                            subTypeColumnsNameAndValue.Add((name, value.ToString()));
                        }

                        subQuery = _queryGenerator.UpdateQuery(typeName, subTypeColumnsNameAndValue, subItemId.ToString());

                        listOfAllSubQuery.Append($"{subQuery}; ");
                        subQuery = "";
                    }

                    var test = listOfAllSubQuery.ToString();

                    _sqlDataStore.Update(listOfAllSubQuery.ToString());
                    listOfAllSubQuery.Clear();
                }
            }
                #endregion

            }

        public void Delete(TEntity item)
        {
            var query = _queryGenerator.DeleteQuery(item.GetType().Name, "Id", item.GetType().GetProperty("Id").GetValue(item).ToString());

            _sqlDataStore.Delete(query);
        }

        public void Dispose()
        {
            if(_sqlConnection != null)
            {
                if(_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }

                _sqlConnection.Dispose();
            }
        }
    }
}
