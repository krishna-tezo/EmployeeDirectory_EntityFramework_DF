using EmployeeDirectory.Data.Interfaces;
using DM = EmployeeDirectory.Data.Models;
using EmployeeDirectory.Models.SummaryModels;
using EmployeeDirectory.Models.Models;
using AutoMapper;
using EmployeeDirectory.Models.Interfaces;

namespace EmployeeDirectory.Services
{
    public class RoleService(IRoleRepository roleRepository, IGenericRepository<DM.Department> departmentRepository, IGenericRepository<DM.Location> locationRepository) : IRoleService
    {

        readonly IRoleRepository roleRepository = roleRepository;
        readonly IGenericRepository<DM.Department> departmentRepository = departmentRepository;
        readonly IGenericRepository<DM.Location> locationRepository = locationRepository;

        public ServiceResult<List<RoleSummary>> GetRolesSummary()
        {
            try
            {
                List<RoleSummary> roles = roleRepository.GetRolesSummary();

                if (roles.Count == 0)
                {
                    return ServiceResult<List<RoleSummary>>.Fail("No roles found");
                }
                return ServiceResult<List<RoleSummary>>.Success(roles);

            }
            catch (Exception ex)
            {
                return ServiceResult<List<RoleSummary>>.Fail("Database Issue:" + ex.Message);
            }
        }
        public ServiceResult<RoleSummary> GetRoleSummary(string id)
        {
            try
            {
                RoleSummary role = roleRepository.GetRoleSummaryById(id);

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
        public ServiceResult<int> AddRole(RoleSummary roleSummary)
        {
            try
            {
                DM.Role role = new();
                string departmentId = GetDepartmentIdByName(roleSummary.Department.Name).Data;

                string locationId = GetLocationIdByName(roleSummary.Location.Name).Data;

                role.Id = roleSummary.Role.Id;
                role.Name = roleSummary.Role.Name;
                role.Description = roleSummary.Description;
                role.LocationId = locationId;
                role.DepartmentId = departmentId;
                int rowsAffected = roleRepository.Insert(role);

                if (rowsAffected > 0)
                {
                    return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} data has been inserted");
                }
                else
                {
                    return ServiceResult<int>.Fail("Can't add the role due to database error");
                }


            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue:" + ex.Message);
            }
        }


        public ServiceResult<List<Department>> GetAllDepartments()
        {
            try
            {
                List<DM.Department> departments;

                departments = departmentRepository.GetAll().Cast<DM.Department>().ToList();

                List<Department> departmentModels = [];
                foreach (DM.Department department in departments)
                {
                    departmentModels.Add(GetMappedObject<DM.Department, Department>(department).Data);
                }

                if (departmentModels == null || departmentModels.Count == 0)
                {
                    return ServiceResult<List<Department>>.Fail($"No department to show");
                }

                return ServiceResult<List<Department>>.Success(departmentModels);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Department>>.Fail("Database Issue: " + ex.Message);
            }
        }
        public ServiceResult<List<Location>> GetAllLocations()
        {
            try
            {
                List<DM.Location> locations;

                locations = locationRepository.GetAll().Cast<DM.Location>().ToList();

                List<Location> locationModels = [];
                foreach (DM.Location location in locations)
                {
                    locationModels.Add(GetMappedObject<DM.Location, Location>(location).Data);
                }

                if (locationModels == null || locationModels.Count == 0)
                {
                    return ServiceResult<List<Location>>.Fail($"No location to show");
                }

                return ServiceResult<List<Location>>.Success(locationModels);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Location>>.Fail("Database Issue: " + ex.Message);
            }
        }

        
        public ServiceResult<string> GenerateNewId<T>()
        {
            try
            {
                string lastId;
                string prefix;
                int suffixCount;
                switch (typeof(T).Name)
                {
                    case nameof(Role):
                        var role = roleRepository.GetLast();
                        if (role == null)
                        {
                            return ServiceResult<string>.Success("RL0001");
                        }
                        lastId = role.Id;
                        prefix = "RL";
                        suffixCount = 4;
                        break;
                    case nameof(Department):
                        var department = departmentRepository.GetLast();
                        if (department == null)
                        {
                            return ServiceResult<string>.Success("DEP001");
                        }
                        lastId = department.Id;
                        prefix = "DPT";
                        suffixCount = 3;
                        break;
                    case nameof(Location):
                        var location = locationRepository.GetLast();
                        if (location == null)
                        {
                            return ServiceResult<string>.Success("LOC001");
                        }
                        lastId = location.Id;
                        prefix = "LOC";
                        suffixCount = 3;
                        break;
                    default:
                        return ServiceResult<string>.Fail("Unsupported entity type for ID generation");

                }

                string numericPart = lastId.Substring(prefix.Length);

                if (int.TryParse(numericPart, out int numericId))
                {
                    int newNumericId = numericId + 1;
                    string newId = prefix + newNumericId.ToString($"D{suffixCount}");
                    return ServiceResult<string>.Success(newId);
                }
                else
                {
                    return ServiceResult<string>.Fail("Invalid ID format in the existing data");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail("Database Issue: " + ex.Message);
            }
        }


        public ServiceResult<string> GetLocationIdByName(string name)
        {
            try
            {
                List<DM.Location> locations = locationRepository.GetAll();
                string locationId;
                if (locations.Count > 0)
                {
                    var location = locations.FirstOrDefault(loc => loc.Name == name);

                    if (location == null)
                    {
                        locationId = GenerateNewId<Location>().Data;
                        DM.Location newLocation = new()
                        {
                            Id = locationId,
                            Name = name
                        };
                        locationRepository.Insert(newLocation);
                    }
                    else
                    {
                        locationId = location.Id;
                    }
                    return ServiceResult<string>.Success(locationId);
                }
                else
                {
                    return ServiceResult<string>.Fail("Location doesn't exist");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail(ex.Message);
            }
        }
        public ServiceResult<string> GetDepartmentIdByName(string name)
        {
            try
            {
                List<DM.Department> departments = departmentRepository.GetAll();

                if (departments.Count > 0)
                {
                    string departmentId = departments.Where(loc => loc.Name == name).First().Id;
                    return ServiceResult<string>.Success(departmentId);
                }
                else
                {
                    return ServiceResult<string>.Fail("Location doesn't exist");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail(ex.Message);
            }
        }

        //Does a role exist with the same name and location
        public ServiceResult<bool> DoesRoleExists(string roleName, string locationName)
        {
            try
            {
                List<DM.Role> roles = roleRepository.GetAll();
                List<DM.Location> locations = locationRepository.GetAll();

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

        //Mapper
        public ServiceResult<TTarget> GetMappedObject<TSrc, TTarget>(TSrc source)
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<TSrc, TTarget>());
            Mapper mapper = new Mapper(config);
            TTarget result = mapper.Map<TSrc, TTarget>(source);
            return ServiceResult<TTarget>.Success(result);

        }
    }
}