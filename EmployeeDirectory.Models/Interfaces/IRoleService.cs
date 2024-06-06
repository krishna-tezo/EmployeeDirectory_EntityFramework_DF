using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Models.Interfaces
{
    public interface IRoleService
    {
        ServiceResult<int> AddRole(RoleSummary roleSummary);
        ServiceResult<bool> DoesRoleExists(string roleName, string locationName);
        ServiceResult<string> GenerateNewId<T>();
        ServiceResult<string> GetDepartmentIdByName(string name);
        ServiceResult<string> GetLocationIdByName(string name);
        ServiceResult<RoleSummary> GetRoleSummary(string id);
        ServiceResult<List<RoleSummary>> GetRolesSummary();
        ServiceResult<List<Department>> GetAllDepartments();
        ServiceResult<List<Location>> GetAllLocations();
    }
}