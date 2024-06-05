using EmployeeDirectory.Models;
using EmployeeDirectory.ViewModel;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Services;
using EmployeeDirectory.Data.SummaryModels;
namespace EmployeeDirectory.UI.Controllers
{
    public class EmployeeController : IEmployeeController
    {
        IEmployeeService employeeService;
       
       

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public ServiceResult<string> GetNewEmployeeId()
        {
            string empId = employeeService.GenerateNewId<EmployeeModel>().Data;
            if (empId == null)
            {
                empId = "TEZ00001";
            }
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
            List<EmployeeView> employeesToView = new List<EmployeeView>();

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
            EmployeeView employeeToView = new EmployeeView();

            var result = employeeService.GetEmployeeSummary(empId);
            if (result.IsOperationSuccess)
            {
                EmployeeSummary employeeSummary = result.Data;
                employeeToView = MapSummaryToView(employeeSummary).Data;

            }
            else
            {
                return ServiceResult<EmployeeView>.Fail(result.Message);
            }
            return ServiceResult<EmployeeView>.Success(employeeToView);
        }

        public ServiceResult<EmployeeModel> GetEmployeeById(string id)
        {
            var result = employeeService.GetEmployeeSummary(id);
            if (result.IsOperationSuccess)
            {
                EmployeeModel employee = result.Data.Employee;
                return ServiceResult<EmployeeModel>.Success(employee);
            }
            else
            {
                return ServiceResult<EmployeeModel>.Fail(result.Message);
            }
        }

        public ServiceResult<int> AddEmployee(EmployeeModel employee)
        {
            
            return employeeService.AddEmployee(employee);
        }

        public ServiceResult<int> EditEmployee(EmployeeModel employee)
        {
            return employeeService.UpdateEmployee(employee);
        }

        public ServiceResult<int> DeleteEmployee(string empId)
        {
            return employeeService.Delete(empId);
        }

        public ServiceResult<List<Tuple<string, string>>> GetProjectNames()
        {
            try
            {
                List<ProjectModel> projects = employeeService.GetAllProjects().Data;
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

        private ServiceResult<EmployeeView> MapSummaryToView(EmployeeSummary summary)
        {
            try
            {
                EmployeeView employee = new EmployeeView()
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