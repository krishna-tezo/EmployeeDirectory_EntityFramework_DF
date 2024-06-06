using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;


namespace EmployeeDirectory.Models.Interfaces
{
    public interface IEmployeeService
    {
        ServiceResult<int> AddEmployee(Employee employee);
        ServiceResult<int> DeleteEmployee(string id);
        ServiceResult<string> GenerateNewId<T>();
        ServiceResult<List<Project>> GetAllProjects();
        ServiceResult<List<EmployeeSummary>> GetEmployeeSummaries();
        ServiceResult<EmployeeSummary> GetEmployeeSummary(string id);
        ServiceResult<int> UpdateEmployee(Employee employee);
    }
}