using Microsoft.Data.SqlClient;
using System.Globalization;

namespace EmployeeDirectory.Data.Services
{
    public class CommonDataService : ICommonDataService
    {
        private IDbConnection dbConnection;
        public CommonDataService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        //Get All
        public List<T> GetAll<T>(string query, Func<SqlDataReader, T> mapFunction)
        {
            List<T> collection = new List<T>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        T obj = mapFunction(reader);
                        collection.Add(obj);
                    }
                }
            }
            return collection;
        }

        //Get By Id
        public T Get<T>(string query, string id, Func<SqlDataReader, T> mapFunction)
        {
            T? entity = default;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        T item = mapFunction(reader);
                        entity = item;
                    }
                }
            }
            return entity;
        }

        //Insert
        public int InsertOrUpdate<T>(string query, T obj)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = GetCommand<T>(query, conn, obj))
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rowsAffected;

                }
            }
        }

        //Delete
        public int DeleteById<T>(string query, string Id)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
        }

        

        //Add Parameters to command
        private SqlCommand GetCommand<T>(string query, SqlConnection conn, T obj)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                cmd.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
            }
            return cmd;
        }

        //Map
        public T MapObject<T>(SqlDataReader reader) where T : new()
        {
            T obj = new();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (reader[prop.Name] == DBNull.Value)
                    continue;

                object value = reader[prop.Name];
                var propType = prop.PropertyType;

                if (propType == typeof(DateTime) && DateTime.TryParseExact(value.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                {
                    prop.SetValue(obj, dateValue);
                }
                else if (propType == typeof(bool))
                {
                    if (value.ToString() == "1")
                    {
                        prop.SetValue(obj, true);
                    }
                    else if (value.ToString() == "0")
                    {
                        prop.SetValue(obj, false);
                    }
                }
                else
                {
                    prop.SetValue(obj, Convert.ChangeType(value, propType));
                }
            }
            return obj;
        }

        //GetIdByName
        public string GetIdByName<T>(string query, string name)
        {
            string? result = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    result = cmd.ExecuteScalar().ToString();
                }
                conn.Close();
                return result;
            }
        }

        //Get Last Id
        public string GetLastId<T>(string query)
        {
            string? result = null;
            
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                
                using (SqlCommand cmd = new(query, conn))
                {
                    result = cmd.ExecuteScalar().ToString();
                }
                conn.Close();
                return result;
            }
        }
    }
}
