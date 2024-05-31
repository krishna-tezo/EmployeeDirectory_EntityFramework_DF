using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace EmployeeDirectory.Data.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {

        private AppDBContext context;
        public EmployeeDataService(AppDBContext context)
        {
            this.context = context;

        }
        public void MapProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
                if (destinationProperty != null)
                {
                    var value = sourceProperty.GetValue(source);
                    if (value != null)
                    {
                        destinationProperty.SetValue(destination, value);
                    }
                }
            }
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
                MapProperties(employee, summary);


                summary.ProjectId = employee.ProjectId;
                summary.ProjectName = employee.Project?.Name;

                summary.Role = employee.Role?.Name;

                summary.DepartmentId = employee.Role.DepartmentId;
                summary.Department = employee.Role?.Department?.Name;

                summary.LocationId = employee.Role.LocationId;
                summary.Location = employee.Role?.Location?.Name;

                summary.ManagerId = employee.Project.ManagerId;
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

            if(employee == null)
            {
                return null;
            }

            EmployeeSummary summary = new EmployeeSummary();
            MapProperties(employee, summary);

            summary.ProjectId = employee.ProjectId;
            summary.ProjectName = employee.Project?.Name;

            summary.Role = employee.Role?.Name;

            summary.DepartmentId = employee.Role.DepartmentId;
            summary.Department = employee.Role?.Department?.Name;

            summary.LocationId = employee.Role.LocationId;
            summary.Location = employee.Role?.Location?.Name;

            summary.ManagerId = employee.Project.ManagerId;
            summary.ManagerName = $"{employee.Project.Manager.Emp.FirstName} {employee.Project.Manager.Emp.LastName}";

            return summary;

        }
        public int DeleteEmployee(string id)
        {
            int rowsAffected = 0;

            var employee = context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                employee.IsDeleted = true;
                rowsAffected = context.SaveChanges();
            }

            return rowsAffected;
        }

    }
}