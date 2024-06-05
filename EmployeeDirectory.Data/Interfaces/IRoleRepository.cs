using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Data.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        List<RoleSummary> GetRolesSummary();
        RoleSummary GetRoleSummaryById(string id);
    }
}
