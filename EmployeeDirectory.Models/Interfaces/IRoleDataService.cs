using EmployeeDirectory.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Data.Data.Services
{
    public interface IRoleDataService
    {
        public List<RoleSummary> GetRolesSummary();
        public RoleSummary GetRoleSummaryById(string id);
    }
}