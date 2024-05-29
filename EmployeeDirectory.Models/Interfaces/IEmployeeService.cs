using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Services
{
    public interface IEmployeeService
    {
        public ServiceResult<EmployeeSummary> GetEmployees();
        public ServiceResult<EmployeeSummary> GetEmployee(string id);

        public ServiceResult<int> Delete(string id);
    }
}