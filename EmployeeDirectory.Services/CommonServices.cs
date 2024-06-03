using EmployeeDirectory.Data.Services;
using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Services
{
    public class CommonServices : ICommonServices
    {
        ICommonDataService commonDataService;
        public CommonServices(ICommonDataService commonDataService)
        {
            this.commonDataService = commonDataService;
        }

        //Get All
        public ServiceResult<T> GetAll<T>() where T : class
        {

            List<T> collection = [];
            try
            {
                collection = commonDataService.GetAll<T>();
                if (typeof(T).Name == "Employee")
                {
                    var propertyInfo = typeof(T).GetProperty("IsDeleted");
                    if (propertyInfo != null)
                    {
                        collection = collection.Where(emp => (bool)propertyInfo.GetValue(emp) == false).ToList();
                    }
                }

                if (collection.Count == 0)
                {
                    return ServiceResult<T>.Fail($"No {typeof(T).Name} To Show");
                }
                return ServiceResult<T>.Success(collection);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.Fail("Database Issue:" + ex.Message);
            }
        }

        //Get
        public ServiceResult<T> Get<T>(string id) where T : class
        {
            try
            {
                T entity = commonDataService.Get<T>(id);
                if (entity == null)
                {
                    return ServiceResult<T>.Fail($"{typeof(T).Name} Id {id} doesn't exist");
                }
                return ServiceResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.Fail("Database Issue:" + ex.Message);
            }
        }

        //Add
        public ServiceResult<int> Add<T>(T obj) where T : class
        {
            try
            {
                int rowsAffected = commonDataService.Insert(obj);
                if (rowsAffected == 0)
                {
                    return ServiceResult<int>.Fail("Database Connectivity Issue");
                }
                return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} data has been inserted");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue:" + ex.Message);
            }
        }

        //Update
        public ServiceResult<int> Update<T>(T newObj) where T : class
        {
            try
            {
                List<T> collection = GetAll<T>().DataList;

                string id = typeof(T).GetProperty("Id").GetValue(newObj).ToString();

                T? existingEntity = commonDataService.Get<T>(id);

                if (existingEntity != null)
                {
                    int rowsAffected = commonDataService.Update(newObj);
                    return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} employee has been updated");
                }
                else
                {
                    return ServiceResult<int>.Fail("Id Doesn't exist");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue:" + ex.Message);
            }
        }        
        public ServiceResult<string> GenerateNewId<T>()
        {
            try
            {
                string lastId;
                string entityName = typeof(T).Name;
                string prefix = "";
                int suffixCount = 4;
                switch (entityName)
                {
                    case "Employee":
                        Employee employee = commonDataService.GetLast<Employee>();
                        if (employee == null)
                        {
                            return ServiceResult<string>.Success("TEZ00001");
                        }
                        else
                        {
                            lastId = employee.Id;
                            prefix = "TEZ";
                            suffixCount = 5;
                        }
                        break;

                    case "Role":
                        Role role = commonDataService.GetLast<Role>();
                        if (role == null)
                        {
                            return ServiceResult<string>.Success("RL0001");
                        }
                        else
                        {
                            lastId = role.Id;
                            prefix = "RL";
                        }
                        break;

                    case "Project":
                        Project project = commonDataService.GetLast<Project>();
                        if (project == null)
                        {
                            return ServiceResult<string>.Success("PR0001");
                        }
                        else
                        {
                            lastId = project.Id;
                            prefix = "PR";
                        }
                        break;


                    case "Department":
                        Department department = commonDataService.GetLast<Department>();
                        if (department == null)
                        {
                            return ServiceResult<string>.Success("DEP001");
                        }
                        else
                        {
                            lastId = department.Id;
                            prefix = "DPT";
                            suffixCount = 3;

                        }
                        break;

                    case "Location":
                        Location location = commonDataService.GetLast<Location>();
                        if (location == null)
                        {
                            return ServiceResult<string>.Success("LOC001");
                        }
                        else
                        {
                            lastId = location.Id;
                            prefix = "LOC";
                            suffixCount = 3;

                        }
                        break;

                    case "Manager":
                        Manager manager = commonDataService.GetLast<Manager>();
                        if (manager == null)
                        {
                            return ServiceResult<string>.Success("MR0001");
                        }
                        else
                        {
                            lastId = manager.Id;
                            prefix = "MR";

                        }
                        break;

                    default:
                        lastId = null;
                        break;

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
                    return ServiceResult<string>.Fail("Invalid Employee Id Format");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail("DataBase Error" + ex.Message);
            }

        }
    }
}