namespace EmployeeDirectory.Data.Services
{
    public class DataServiceManager : IDataServiceManager
    {
        private ICommonDataService commonDataServices;
        public DataServiceManager(ICommonDataService commonDataServices)
        {
            this.commonDataServices = commonDataServices;
        }
        public List<T> GetAll<T>() where T : new()
        {

            List<T> records = new List<T>();
            string entityName = typeof(T).Name;

            string query = $"SELECT * FROM {entityName}";

            if (typeof(T).Name == "Employee")
            {
                query += " WHERE IsDeleted=0";
            }
            return commonDataServices.GetAll<T>(query, commonDataServices.MapObject<T>);

        }

        //Get Query
        public T Get<T>(string Id) where T : new()
        {

            T record = new();
            string entityName = typeof(T).Name;

            string query = $"SELECT * FROM {entityName} WHERE Id= @Id";

            return commonDataServices.Get(query, Id, commonDataServices.MapObject<T>);
        }

        //Insert Into Query
        public int Insert<T>(T obj) where T : new()
        {
            var properties = typeof(T).GetProperties();
            string tableName = typeof(T).Name;

            string columns = string.Join(", ", properties.Select(p => p.Name));


            string values = string.Join(", ", properties.Select(p => "@" + p.Name));

            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            return commonDataServices.InsertOrUpdate(query, obj);
        }

        //Update Query
        public int Update<T>(T obj) where T : new()
        {
            var properties = typeof(T).GetProperties();
            string tableName = typeof(T).Name;

            string setClause = string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"{p.Name} = @{p.Name}"));

            var idProperty = properties.FirstOrDefault(p => p.Name == "Id");
            if (idProperty == null)
            {
                throw new Exception("No primary key property named 'Id' found.");
            }

            string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

            // Call the Update method with the query and the object
            return commonDataServices.InsertOrUpdate(query, obj);
        }

        //Delete Query
        public int DeleteById<T>(string Id)
        {

            string entityName = typeof(T).Name;

            string query = $"DELETE FROM {entityName} WHERE Id= @Id";

            return commonDataServices.DeleteById<T>(query, Id);
        }

        //Get Id by Name
        public string GetIdByName<T>(string name)
        {
            string query = $"SELECT Id FROM {typeof(T).Name} WHERE Name = @Name";
            return commonDataServices.GetIdByName<T>(query, name);

        }

        //Get New Id
        public string GetLastId<T>()
        {
            string query = $"SELECT MAX(Id) FROM {typeof(T).Name}";
            return commonDataServices.GetLastId<T>(query);
        }
    }
}
