namespace EmployeeDirectory.Data.Services
{
    public interface ICommonDataService
    {
        int DeleteById<T>(string id) where T : class;
        T Get<T>(string id) where T : class;
        List<T> GetAll<T>() where T : class;
        int Insert<T>(T obj);
        int Update<T>(T obj);
        T GetLast<T>() where T : class;
    }
}