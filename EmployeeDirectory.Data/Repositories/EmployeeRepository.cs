using EmployeeDirectory.Data.Interfaces;
using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Data.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Models.Models;


namespace EmployeeDirectory.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDBContext context;
        public EmployeeRepository(AppDBContext context) : base(context)
        {
            this.context = context;
        }

        public List<EmployeeSummary> GetEmployeesSummary()
        {
            var employees = context.Employees
                .Where(e => e.IsDeleted != true)
                .Include(e => e.Project)
                    .ThenInclude(p => p.Manager)
                        .ThenInclude(m => m.Emp)
                .Include(e => e.Role)
                    .ThenInclude(r => r.Location)
                .Include(e => e.Role)
                    .ThenInclude(r => r.Department)
                .ToList();

            var employeeSummaries = new List<EmployeeSummary>();

            foreach (var employee in employees)
            {
                var summary = new EmployeeSummary();

                summary.Employee = MapProperties<Employee, EmployeeModel>(employee);
                summary.Role = MapProperties<Role, RoleModel>(employee.Role);
                summary.Project = MapProperties<Project, ProjectModel>(employee.Project);
                summary.Manager = MapProperties<Manager, ManagerModel>(employee.Project.Manager);
                summary.Department = MapProperties<Department, DepartmentModel>(employee.Role.Department);
                summary.Location = MapProperties<Location, LocationModel>(employee.Role.Location);
                summary.ManagerName = $"{employee.Project.Manager.Emp.FirstName} {employee.Project.Manager.Emp.LastName}";

                employeeSummaries.Add(summary);
            }

            return employeeSummaries;
        }

        public EmployeeSummary GetEmployeeSummaryById(string id)
        {
            var employee = context.Employees
                 .Where(e => e.IsDeleted != true)
                 .Where(e => e.Id == id)
                 .Include(e => e.Project)
                     .ThenInclude(p => p.Manager)
                         .ThenInclude(m => m.Emp)
                 .Include(e => e.Role)
                     .ThenInclude(r => r.Location)
                 .Include(e => e.Role)
                     .ThenInclude(r => r.Department)
                  .FirstOrDefault();

            if (employee == null)
            {
                return null;
            }

            EmployeeSummary summary = new EmployeeSummary();
            summary.Employee = MapProperties<Employee, EmployeeModel>(employee);
            summary.Role = MapProperties<Role, RoleModel>(employee.Role);
            summary.Project = MapProperties<Project, ProjectModel>(employee.Project);
            summary.Manager = MapProperties<Manager, ManagerModel>(employee.Project.Manager);
            summary.Department = MapProperties<Department, DepartmentModel>(employee.Role.Department);
            summary.Location = MapProperties<Location, LocationModel>(employee.Role.Location);
            summary.ManagerName = $"{employee.Project.Manager.Emp.FirstName} {employee.Project.Manager.Emp.LastName}";

            return summary;
        }

        public int UpdateEmployee(Employee employee)
        {
            Employee trackedEntity = context.Employees.Find(employee.Id);

            employee.CreatedBy = trackedEntity.CreatedBy;
            employee.CreatedDate = trackedEntity.CreatedDate;
            context.Entry(trackedEntity).CurrentValues.SetValues(employee);

            return context.SaveChanges();
        }
        public int DeleteEmployee(string id)
        {
            var employee = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                employee.IsDeleted = true;
                return context.SaveChanges();
            }
            return 0;
        }

        private TTarget MapProperties<TSource, TTarget>(TSource source) where TTarget : class, new()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TTarget).GetProperties();

            
            TTarget target = new TTarget();

            foreach (var sourceProperty in sourceProperties)
            {
                var targetProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
                if (targetProperty != null)
                {
                    var value = sourceProperty.GetValue(source);
                    if (value != null)
                    {
                        targetProperty.SetValue(target, value);
                    }
                }
            }
            return target;
        }

    }
}