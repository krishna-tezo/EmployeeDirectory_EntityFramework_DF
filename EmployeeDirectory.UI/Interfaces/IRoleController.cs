using EmployeeDirectory.Models;
using EmployeeDirectory.UI.ViewModels;

namespace EmployeeDirectory.Controllers
{
    public interface IRoleController
    {
        ServiceResult<int> Add(RoleView role);
        ServiceResult<string> GenerateRoleId();
        public ServiceResult<bool> DoesRoleExists(string roleName, string locationName);
        ServiceResult<List<string>> GetAllDepartments();
        public ServiceResult<List<Tuple<string, string, string>>> GetRoleNamesWithLocation();
        ServiceResult<List<RoleView>> ViewRoles();
    }
}