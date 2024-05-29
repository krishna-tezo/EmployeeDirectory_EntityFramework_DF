using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Data.Data.Services
{
    public interface IEmployeeDataService
    {
        EmployeeSummary GetEmployeeSummaryById(string id);
        List<EmployeeSummary> GetEmployeesSummary();
        int DeleteEmployee(string id);
    }
}