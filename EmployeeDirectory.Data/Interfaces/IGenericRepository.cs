namespace EmployeeDirectory.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class 
    {
        List<T> GetAll();
        T Get(string id);
        int Insert(T obj);
        int Update(T obj);
        int DeleteById(string id);
        T GetLast();
    }
}