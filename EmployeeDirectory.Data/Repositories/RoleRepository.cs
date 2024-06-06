using AutoMapper;
using EmployeeDirectory.Data.Interfaces;
using DM = EmployeeDirectory.Data.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using Microsoft.EntityFrameworkCore;


namespace EmployeeDirectory.Data.Repositories
{
    public class RoleRepository(DM.AppDBContext context, IMapper mapper) : GenericRepository<DM.Role>(context), IRoleRepository
    {
        private readonly DM.AppDBContext context = context;
        private readonly IMapper mapper = mapper;

        public List<RoleSummary> GetRolesSummary()
        {
            var roles = context.Roles
                .Include(r => r.Department)
                .Include(r => r.Location)
                .ToList();

            var roleSummaries = new List<RoleSummary>();

            foreach (var role in roles)
            {
                RoleSummary summary = new()
                {
                    Role = mapper.Map<DM.Role, Role>(role),
                    Location = mapper.Map<DM.Location, Location>(role.Location),
                    Department = mapper.Map<DM.Department, Department>(role.Department),
                    Description = role.Description
                };

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
            summary.Role = mapper.Map<DM.Role, Role>(role);
            summary.Location = mapper.Map<DM.Location, Location>(role.Location);
            summary.Department = mapper.Map<DM.Department, Department>(role.Department);
            summary.Description = role.Description;

            return summary;
        }

    }
}