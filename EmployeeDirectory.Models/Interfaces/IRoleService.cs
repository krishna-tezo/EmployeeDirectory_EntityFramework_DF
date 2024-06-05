using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Services
{
    public interface IRoleService
    {
        ServiceResult<int> Add(RoleSummary roleSummary);
        ServiceResult<bool> DoesRoleExists(string roleName, string locationName);
        ServiceResult<string> GenerateNewId<T>();
        ServiceResult<string> GetDepartmentIdByName(string name);
        ServiceResult<string> GetLocationIdByName(string name);
        ServiceResult<RoleSummary> GetRoleSummary(string id);
        ServiceResult<List<RoleSummary>> GetRolesSummary();
        ServiceResult<List<DepartmentModel>> GetAllDepartments();
        ServiceResult<List<LocationModel>> GetAllLocations();
        ServiceResult<T> Get<T>(string id) where T : class;
    }
}