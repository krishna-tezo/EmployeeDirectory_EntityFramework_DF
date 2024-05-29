
namespace EmployeeDirectory.Data.Services
{
    public interface IDataServiceManager
    {
        int DeleteById<T>(string Id);
        T Get<T>(string Id) where T : new();
        List<T> GetAll<T>() where T : new();
        string GetIdByName<T>(string name);
        string GetLastId<T>();
        int Insert<T>(T obj) where T : new();
        int Update<T>(T obj) where T : new();
    }
}