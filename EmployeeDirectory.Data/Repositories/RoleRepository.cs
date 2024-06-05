using EmployeeDirectory.Data.Interfaces;
using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using Microsoft.EntityFrameworkCore;


namespace EmployeeDirectory.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDBContext context;

        public RoleRepository(AppDBContext context) : base(context)
        {
            this.context = context;
        }

        public List<RoleSummary> GetRolesSummary()
        {
            var roles = context.Roles
                .Include(r => r.Department)
                .Include(r => r.Location)
                .ToList();

            var roleSummaries = new List<RoleSummary>();

            foreach (var role in roles)
            {
                RoleSummary summary = new RoleSummary();
                summary.Role = MapProperties<Role, RoleModel>(role);
                summary.Location = MapProperties<Location, LocationModel>(role.Location);
                summary.Department = MapProperties<Department, DepartmentModel>(role.Department);
                summary.Description = role.Description;

                roleSummaries.Add(summary);
            }

            return roleSummaries;
        }

        public RoleSummary GetRoleSummaryById(string id)
        {
            var role = context.Roles
                .Where(r => r.Id == id)
                .Include(r => r.Department)
                .Include(r => r.Location)
                .FirstOrDefault();

            if (role == null)
            {
                return null;
            }

            RoleSummary summary = new RoleSummary();
            summary.Role = MapProperties<Role, RoleModel>(role);
            summary.Location = MapProperties<Location, LocationModel>(role.Location);
            summary.Department = MapProperties<Department, DepartmentModel>(role.Department);
            summary.Description = role.Description;

            return summary;
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