
namespace EmployeeDirectory.Data.Services
{
    public interface IDataServiceManager
    {
        int DeleteById<T>(string Id);
        T Get<T>(string Id) where T : class;
        List<T> GetAll<T>() where T : class;
        string GetIdByName<T>(string name) where T:class;
        string GetLastId<T>() where T: class;
        int Insert<T>(T obj) where T : class;
        int Update<T>(T obj) where T : class;
    }
}