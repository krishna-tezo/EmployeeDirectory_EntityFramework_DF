using EmployeeDirectory.Models;
using EmployeeDirectory.ViewModel;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Services;
using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.UI.Interfaces;
namespace EmployeeDirectory.UI.Controllers
{
    public class EmployeeController : IEmployeeController
    {
        ICommonServices commonServices;
        IEmployeeService employeeService;
        ICommonController commonController;
       

        public EmployeeController(ICommonServices commonServices, IEmployeeService employeeService, ICommonController commonController)
        {
            this.commonServices = commonServices;
            this.employeeService = employeeService;
            this.commonController = commonController;
        }

        public ServiceResult<string> GetNewEmployeeId()
        {
            string empId = commonServices.GenerateNewId<Employee>().Data;
            if (empId == null)
            {
                empId = "TEZ00001";
            }
            return ServiceResult<string>.Success(empId);
        }

        public ServiceResult<EmployeeView> ViewEmployees()
        {
            var employees = employeeService.GetEmployees();

            if (!employees.IsOperationSuccess)
            {
                return ServiceResult<EmployeeView>.Fail($"{employees.Message}");
            }

            List<EmployeeSummary> employeesSummary = employees.DataList;
            List<EmployeeView> employeesToView = new List<EmployeeView>();

            foreach (EmployeeSummary employee in employeesSummary)
            {
                employeesToView.Add(commonController.Map<EmployeeSummary,EmployeeView>(employee).Data);
            }

            if (employeesToView.Count > 0)
            {
                return ServiceResult<EmployeeView>.Success(employeesToView);
            }
            else
            {
                return ServiceResult<EmployeeView>.Fail("No Employee To Show");
            }
        }

        public ServiceResult<EmployeeView> ViewEmployee(string empId)
        {
            EmployeeView employeeToView = new EmployeeView();

            EmployeeSummary employeeSummary = employeeService.GetEmployee(empId).Data;

            if (employeeSummary == null)
            {
                return ServiceResult<EmployeeView>.Fail("Data not found");
            }
            else
            {
                employeeToView = commonController.Map<EmployeeSummary, EmployeeView>(employeeSummary).Data;
            }
            return ServiceResult<EmployeeView>.Success(employeeToView);
        }

        public ServiceResult<Employee> GetEmployeeById(string id)
        {
            return commonServices.Get<Employee>(id);
        }

        public ServiceResult<int> AddEmployee(Employee employee)
        {
            return commonServices.Add<Employee>(employee);
        }

        public ServiceResult<int> EditEmployee(Employee employee)
        {
            return commonServices.Update(employee);
        }

        public ServiceResult<int> DeleteEmployee(string empId)
        {
            return employeeService.Delete(empId);
        }

        public ServiceResult<List<Tuple<string, string>>> GetProjectNames()
        {
            try
            {
                List<Project> projects = commonServices.GetAll<Project>().DataList;
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
    }
}