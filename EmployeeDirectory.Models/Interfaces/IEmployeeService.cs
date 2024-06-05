using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;


namespace EmployeeDirectory.Services
{
    public interface IEmployeeService
    {
        ServiceResult<int> AddEmployee(EmployeeModel employee);
        ServiceResult<int> Delete(string id);
        ServiceResult<string> GenerateNewId<T>();
        ServiceResult<List<ProjectModel>> GetAllProjects();
        ServiceResult<List<EmployeeSummary>> GetEmployeeSummaries();
        ServiceResult<EmployeeSummary> GetEmployeeSummary(string id);
        ServiceResult<TTarget> GetMappedObject<TSrc, TTarget>(TSrc source);
        ServiceResult<int> UpdateEmployee(EmployeeModel employee);
    }
}