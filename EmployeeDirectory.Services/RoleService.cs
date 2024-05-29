using EmployeeDirectory.Data.Data.Services;
using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Services
{
    public class RoleService : IRoleService
    {
        ICommonServices commonServices;
        IRoleDataService roleDataService;
        public RoleService(ICommonServices commonServices, IRoleDataService roleDataService)
        {
            this.commonServices = commonServices;
            this.roleDataService = roleDataService;
        }

        public ServiceResult<RoleSummary> GetRoles()
        {
            try
            {
                List<RoleSummary> roles = roleDataService.GetRolesSummary();

                if (roles.Count == 0)
                {
                    return ServiceResult<RoleSummary>.Fail("No roles found");
                }
                return ServiceResult<RoleSummary>.Success(roles);

            }
            catch (Exception ex)
            {
                return ServiceResult<RoleSummary>.Fail("Database Issue:" + ex.Message);
            }
        }

        public ServiceResult<RoleSummary> GetRole(string id)
        {
            try
            {
                RoleSummary role = roleDataService.GetRoleSummaryById(id);

                if (role == null)
                {
                    return ServiceResult<RoleSummary>.Fail("No roles found");
                }
                return ServiceResult<RoleSummary>.Success(role);

            }
            catch (Exception ex)
            {
                return ServiceResult<RoleSummary>.Fail("Database Issue:" + ex.Message);
            }
        }
        public ServiceResult<int> Add(RoleSummary roleSummary)
        {
            try
            {
                Role role = new Role();
                string departmentId = commonServices.GetIdFromName<Department>(roleSummary.Department).Data;
                if (departmentId == null)
                {
                    departmentId = commonServices.GenerateNewId<Department>().Data;
                }

                string locationId = commonServices.GetIdFromName<Location>(roleSummary.Location).Data;
                if (locationId == null)
                {
                    locationId = commonServices.GenerateNewId<Location>().Data;
                    Location location = new Location
                    {
                        Id = locationId,
                        Name = roleSummary.Location
                    };
                    commonServices.Add(location);
                }
                role.Id = roleSummary.Id;
                role.Name = roleSummary.Name;
                role.Description = roleSummary.Description;
                role.LocationId = locationId;
                role.DepartmentId = departmentId;

                return commonServices.Add(role);

            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue:" + ex.Message);
            }
        }

        //Does a role exist with the same name and location
        public ServiceResult<bool> DoesRoleExists(string roleName, string locationName)
        {
            try
            {
                List<Role> roles = commonServices.GetAll<Role>().DataList;
                List<Location> locations = commonServices.GetAll<Location>().DataList;

                var result = roles
                    .Join(locations, role => role.LocationId, location => location.Id, (role, location) => new { Role = role, Location = location })
                    .Any(rl => rl.Role.Name == roleName && rl.Location.Name == locationName);

                return ServiceResult<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Fail("Database Issue: " + ex.Message);
            }
        }
    }
}
