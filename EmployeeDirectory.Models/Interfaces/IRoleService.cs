using EmployeeDirectory.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Services
{
    public interface IRoleService
    {
        ServiceResult<RoleSummary> GetRole(string id);
        ServiceResult<RoleSummary> GetRoles();
        public ServiceResult<int> Add(RoleSummary roleSummary);
        public ServiceResult<bool> DoesRoleExists(string roleName, string locationName);
    }
}