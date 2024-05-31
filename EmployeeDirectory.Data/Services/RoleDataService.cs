using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using Microsoft.EntityFrameworkCore;
namespace EmployeeDirectory.Data.Services
{
    public class RoleDataService : IRoleDataService
    {
        AppDBContext context;

        public RoleDataService(AppDBContext context)
        {
            this.context = context;
        }

        public static class PropertyMapper
        {
            public static void MapProperties<TSource, TDestination>(TSource source, TDestination destination)
            {
                var sourceProperties = typeof(TSource).GetProperties();
                var destinationProperties = typeof(TDestination).GetProperties();

                foreach (var sourceProperty in sourceProperties)
                {
                    var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
                    if (destinationProperty != null)
                    {
                        var value = sourceProperty.GetValue(source);
                        destinationProperty.SetValue(destination, value);
                    }
                }
            }
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
                var summary = new RoleSummary();
                PropertyMapper.MapProperties(role, summary);

                summary.Department = role.Department?.Name;
                summary.Location = role.Location?.Name;

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

            var summary = new RoleSummary();
            PropertyMapper.MapProperties(role, summary);

            summary.Department = role.Department?.Name;
            summary.Location = role.Location?.Name;

            return summary;
        }
    }
}