using EmployeeDirectory.Models.Interfaces;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;
using EmployeeDirectory.UI.Models;

using System.Data;

namespace EmployeeDirectory.Controllers
{
    public class RoleController : IRoleController
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        //View All Roles
        public ServiceResult<List<RoleView>> ViewRoles()
        {

            List<RoleSummary> roles = roleService.GetRolesSummary().Data;
            List<RoleView> rolesToView = new List<RoleView>();
            if (roles != null)
            {
                foreach (RoleSummary role in roles)
                {
                    rolesToView.Add(MapSummaryToView(role).Data);
                }
                return ServiceResult<List<RoleView>>.Success(rolesToView);
            }
            else
            {
                return ServiceResult<List<RoleView>>.Fail("Some Error Occurred");
            }
        }

        //Add a role
        public ServiceResult<int> Add(RoleView viewRole)
        {
            Role role = new Role();
            RoleSummary roleSummary = new RoleSummary
            {
                Role = new Role { Id = viewRole.Id, Name = viewRole.Name },
                Department = new Department { Name = viewRole.Department },
                Location = new Location { Name = viewRole.Location},
                Description = viewRole.Description
            };
            return roleService.AddRole(roleSummary);
            
        }


        //Generate a new Role Id
        public ServiceResult<string> GenerateRoleId()
        {
            return roleService.GenerateNewId<Role>();
        }

        // Get All Role Names along with location name
        public ServiceResult<List<Tuple<string, string, string>>> GetRoleNamesWithLocation()
        {
            // Return RoleId, RoleName and Location
            try
            {
                List<RoleView> roles = ViewRoles().Data;
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
                List<Department> departments = roleService.GetAllDepartments().Data;
                List<string> departmentsName = departments.Select(dept => dept.Name).Distinct().ToList();
                return ServiceResult<List<string>>.Success(departmentsName);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<string>>.Fail(ex.Message);
            }
        }

        //Mapping
        private ServiceResult<RoleView> MapSummaryToView(RoleSummary summary)
        {
            try
            {
                RoleView employee = new()
                {
                    Id = summary.Role.Id,
                    Name = summary.Role.Name,
                    Department = summary.Department.Name,
                    Location = summary.Location.Name,
                    Description = summary.Description
                };
                return ServiceResult<RoleView>.Success(employee);
            }
            catch (Exception ex)
            {
                return ServiceResult<RoleView>.Fail("Error Occurred while mapping: " + ex.Message);
            }
        }
        
    }
}
