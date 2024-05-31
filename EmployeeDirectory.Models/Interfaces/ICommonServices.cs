using EmployeeDirectory.Models;

namespace EmployeeDirectory.Services
{
    public interface ICommonServices
    {
        ServiceResult<int> Add<T>(T obj) where T : class;
        public ServiceResult<string> GenerateNewId<T>();
        ServiceResult<T> Get<T>(string id) where T : class;
        ServiceResult<T> GetAll<T>() where T : class;
        ServiceResult<int> Update<T>(T newObj) where T : class;
    }
}