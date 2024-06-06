using EmployeeDirectory.Model;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using EmployeeDirectory.Models.Interfaces;
namespace EmployeeDirectory.UI.Controllers
{
    public class EmployeeController(IEmployeeService employeeService) : IEmployeeController
    {
        readonly IEmployeeService employeeService = employeeService;

        public ServiceResult<string> GetNewEmployeeId()
        {
            string empId = employeeService.GenerateNewId<Employee>().Data;
            empId ??= "TEZ00001";
            return ServiceResult<string>.Success(empId);
        }

        public ServiceResult<List<EmployeeView>> ViewEmployees()
        {
            var employees = employeeService.GetEmployeeSummaries();

            if (!employees.IsOperationSuccess)
            {
                return ServiceResult<List<EmployeeView>>.Fail($"{employees.Message}");
            }

            List<EmployeeSummary> employeesSummary = employees.Data;
            List<EmployeeView> employeesToView = [];

            foreach (EmployeeSummary employee in employeesSummary)
            {
                employeesToView.Add(MapSummaryToView(employee).Data);
            }

            if (employeesToView.Count > 0)
            {
                return ServiceResult<List<EmployeeView>>.Success(employeesToView);
            }
            else
            {
                return ServiceResult<List<EmployeeView>>.Fail("No Employee To Show");
            }
        }

        public ServiceResult<EmployeeView> ViewEmployee(string empId)
        {

            var result = employeeService.GetEmployeeSummary(empId);
            if (result.IsOperationSuccess)
            {
                EmployeeSummary employeeSummary = result.Data;
                EmployeeView employeeToView = MapSummaryToView(employeeSummary).Data;
                return ServiceResult<EmployeeView>.Success(employeeToView);

            }
            else
            {
                return ServiceResult<EmployeeView>.Fail(result.Message);
            }
        }

        public ServiceResult<Employee> GetEmployeeById(string id)
        {
            var result = employeeService.GetEmployeeSummary(id);
            if (result.IsOperationSuccess)
            {
                Employee employee = result.Data.Employee;
                return ServiceResult<Employee>.Success(employee);
            }
            else
            {
                return ServiceResult<Employee>.Fail(result.Message);
            }
        }

        public ServiceResult<int> AddEmployee(Employee employee)
        {
            
            return employeeService.AddEmployee(employee);
        }

        public ServiceResult<int> EditEmployee(Employee employee)
        {
            return employeeService.UpdateEmployee(employee);
        }

        public ServiceResult<int> DeleteEmployee(string empId)
        {
            return employeeService.DeleteEmployee(empId);
        }

        public ServiceResult<List<Tuple<string, string>>> GetProjectNames()
        {
            try
            {
                List<Project> projects = employeeService.GetAllProjects().Data;
                List<Tuple<string, string>> projectDetails = projects
                    .Select(project => new { project.Id, project.Name })
                    .AsEnumerable()
                    .Select(project => new Tuple<string, string>(project.Id, project.Name))
                    .ToList();

                return ServiceResult<List<Tuple<string, string>>>.Success(projectDetails);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Tuple<string, string>>>.Fail(ex.Message);
            }
        }

        private static ServiceResult<EmployeeView> MapSummaryToView(EmployeeSummary summary)
        {
            try
            {
                EmployeeView employee = new()
                {
                    Id = summary.Employee.Id,
                    Name = $"{summary.Employee.FirstName} {summary.Employee.LastName}",
                    Role = summary.Role.Name,
                    Department = summary.Department.Name,
                    Location = summary.Location.Name,
                    JoinDate = summary.Employee.JoinDate,
                    ManagerName = summary.ManagerName,
                    ProjectName = summary.Project.Name
                };
                return ServiceResult<EmployeeView>.Success(employee);
            }
            catch (Exception ex)
            {
                return ServiceResult<EmployeeView>.Fail("Error Occurred while mapping: "+ex.Message);
            }
        }
    }
}