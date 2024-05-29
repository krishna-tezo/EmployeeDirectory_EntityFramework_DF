using EmployeeDirectory.Models;

namespace EmployeeDirectory.Services
{
    public interface ICommonServices
    {
        ServiceResult<int> Add<T>(T obj) where T : new();
        ServiceResult<string> GenerateNewId<T>();
        ServiceResult<T> Get<T>(string id) where T : new();
        ServiceResult<T> GetAll<T>() where T : new();

        public ServiceResult<int> Delete<T>(string id) where T : new();
        ServiceResult<string> GetIdFromName<T>(string Name);
        ServiceResult<int> Update<T>(T newObj) where T : new();
    }
}