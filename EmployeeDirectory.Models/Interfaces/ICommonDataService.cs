using Microsoft.Data.SqlClient;

namespace EmployeeDirectory.Data.Services
{
    public interface ICommonDataService
    {
        int DeleteById<T>(string query, string Id);
        T Get<T>(string query, string id, Func<SqlDataReader, T> mapFunction);
        List<T> GetAll<T>(string query, Func<SqlDataReader, T> mapFunction);
        int InsertOrUpdate<T>(string query, T obj);
        T MapObject<T>(SqlDataReader reader) where T : new();
        string GetLastId<T>(string query);
        string GetIdByName<T>(string query, string name);
    }
}