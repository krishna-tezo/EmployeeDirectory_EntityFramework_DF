using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using EmployeeDirectory.Services;
using EmployeeDirectory.UI.Interfaces;
using EmployeeDirectory.UI.ViewModels;

using System.Data;

namespace EmployeeDirectory.Controllers
{
    public class RoleController : IRoleController
    {
        private readonly ICommonServices commonServices;
        private readonly IRoleService roleService;
        private readonly ICommonController commonController;

        public RoleController(ICommonServices commonServices, IRoleService roleService, ICommonController commonController)
        {
            this.commonServices = commonServices;
            this.roleService = roleService;
            this.commonController = commonController;
        }

        //View All Roles
        public ServiceResult<RoleView> ViewRoles()
        {

            List<RoleSummary> roles = roleService.GetRoles().DataList;
            List<RoleView> rolesToView = new List<RoleView>();
            if (roles != null)
            {
                foreach (RoleSummary role in roles)
                {
                    rolesToView.Add(commonController.Map<RoleSummary, RoleView>(role).Data);
                }
                return ServiceResult<RoleView>.Success(rolesToView);
            }
            else
            {
                return ServiceResult<RoleView>.Fail("Some Error Occurred");
            }
        }

        //Add a role
        public ServiceResult<int> Add(RoleView viewRole)
        {
            Role role = new Role();
            RoleSummary roleSummary = new RoleSummary
            {
                Id = viewRole.Id,
                Name = viewRole.Name,
                Description = viewRole.Description,
                Department = viewRole.Department,
                Location = viewRole.Location,

            };
            return roleService.Add(roleSummary);
            
        }


        //Generate a new Role Id
        public ServiceResult<string> GenerateRoleId()
        {
            return commonServices.GenerateNewId<Role>();
        }

        // Get All Role Names along with location name
        public ServiceResult<List<Tuple<string, string, string>>> GetRoleNamesWithLocation()
        {
            // Return RoleId, RoleName and Location
            try
            {
                List<RoleView> roles = ViewRoles().DataList;
                List<Tuple<string, string, string>> roleDetails = roles
                    .Select(role => new { role.Id, role.Name, role.Location })
                    .OrderBy(role => role.Name)
                    .Select(role => new Tuple<string, string, string>(role.Id, role.Name, role.Location))
                    .ToList();

                return ServiceResult<List<Tuple<string, string, string>>>.Success(roleDetails);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Tuple<string, string, string>>>.Fail(ex.Message);
            }
        }

        //Does a role exist with the same name and location
        public ServiceResult<bool> DoesRoleExists(string roleName, string locationName)
        {
            bool roleExists = roleService.DoesRoleExists(roleName, locationName).Data;

            return ServiceResult<bool>.Success(roleExists);

        }

        public ServiceResult<List<string>> GetAllDepartments()
        {
            try
            {
                List<Department> departments = commonServices.GetAll<Department>().DataList;
                List<string> departmentsName = departments.Select(dept => dept.Name).Distinct().ToList();
                return ServiceResult<List<string>>.Success(departmentsName);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<string>>.Fail(ex.Message);
            }
        }
    }
}
