using EmployeeDirectory.Data.Interfaces;
using DM = EmployeeDirectory.Data.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Models.Models;
using AutoMapper;
using EmployeeDirectory.Models.SummaryModels;


namespace EmployeeDirectory.Data.Repositories
{
    public class EmployeeRepository(DM.AppDBContext context, IMapper mapper) : GenericRepository<DM.Employee>(context), IEmployeeRepository
    {
        private readonly DM.AppDBContext context = context;
        private readonly IMapper mapper = mapper;

        public List<EmployeeSummary> GetEmployeesSummary()
        {
            var employees = context.Employees
                .Where(e => !e.IsDeleted)
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
                EmployeeSummary summary = new ()
                {
                    Employee = mapper.Map<DM.Employee, Employee>(employee),
                    Role = mapper.Map<DM.Role, Role>(employee.Role),
                    Project = mapper.Map<DM.Project, Project>(employee.Project),
                    Manager = mapper.Map<DM.Manager, Manager>(employee.Project.Manager),
                    Department = mapper.Map<DM.Department, Department>(employee.Role.Department),
                    Location = mapper.Map<DM.Location, Location>(employee.Role.Location),
                    ManagerName = $"{employee.Project.Manager.Emp.FirstName} {employee.Project.Manager.Emp.LastName}"
                };

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

            if (employee != null)
            {
                return new EmployeeSummary()
                {
                    Employee = mapper.Map<DM.Employee, Employee>(employee),
                    Role = mapper.Map<DM.Role, Role>(employee.Role),
                    Project = mapper.Map<DM.Project, Project>(employee.Project),
                    Manager = mapper.Map<DM.Manager, Manager>(employee.Project.Manager),
                    Department = mapper.Map<DM.Department, Department>(employee.Role.Department),
                    Location = mapper.Map<DM.Location, Location>(employee.Role.Location),
                    ManagerName = $"{employee.Project.Manager.Emp.FirstName} {employee.Project.Manager.Emp.LastName}"
                };
            }
            return null;
        }

        public int UpdateEmployee(DM.Employee employee)
        {
            DM.Employee trackedEntity = context.Employees.Find(employee.Id);

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
    }
}