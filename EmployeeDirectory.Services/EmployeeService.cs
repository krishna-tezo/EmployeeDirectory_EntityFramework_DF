
using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Services
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeDataService employeeDataService;
        public EmployeeService(IEmployeeDataService employeeDataService)
        {
            this.employeeDataService = employeeDataService;
        }
        public ServiceResult<EmployeeSummary> GetEmployees()
        {

            List<EmployeeSummary> employees = [];
            try
            {
                employees = employeeDataService.GetEmployeesSummary();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            employees = employees.Where(emp => emp.IsDeleted == false).ToList();
            if (employees != null)
            {
                return ServiceResult<EmployeeSummary>.Success(employees);
            }
            else
            {
                return ServiceResult<EmployeeSummary>.Fail("No Employees");
            }
        }

        public ServiceResult<EmployeeSummary> GetEmployee(string id)
        {

            EmployeeSummary employee  = new EmployeeSummary();
            try
            {
                employee = employeeDataService.GetEmployeeSummaryById(id);
                if (employee != null)
                {
                    return ServiceResult<EmployeeSummary>.Success(employee);
                }
                else
                {
                    return ServiceResult<EmployeeSummary>.Fail("No Employee Found");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<EmployeeSummary>.Fail("Database Issue:"+ex.Message);
            }
            
        }

        public ServiceResult<int> Delete(string id)
        {
            try
            {
                int rowsAffected = employeeDataService.DeleteEmployee(id);
                if(rowsAffected > 0)
                {
                    return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} row affected");
                }
                else
                {
                    return ServiceResult<int>.Fail("Couldn't delete");
                }
            }
            catch(Exception ex)
            {
                return ServiceResult<int>.Fail("DataBase Error"+ex.Message);
            }
            
        }
    }
}
