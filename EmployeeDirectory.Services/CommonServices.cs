using EmployeeDirectory.Data.Services;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Services
{
    public class CommonServices : ICommonServices
    {
        IDataServiceManager dataServiceManager;
        public CommonServices(IDataServiceManager dataServiceManager)
        {
            this.dataServiceManager = dataServiceManager;
        }

        //Get All
        public ServiceResult<T> GetAll<T>() where T : new()
        {

            List<T> collection = [];
            try
            {
                collection = dataServiceManager.GetAll<T>();
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
        public ServiceResult<T> Get<T>(string id) where T : new()
        {
            try
            {
                T entity = dataServiceManager.Get<T>(id);
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
        public ServiceResult<int> Add<T>(T obj) where T : new()
        {
            try
            {
                int rowsAffected = dataServiceManager.Insert(obj);
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
        public ServiceResult<int> Update<T>(T newObj) where T : new()
        {
            try
            {
                List<T> collection = GetAll<T>().DataList;

                string id = typeof(T).GetProperty("Id").GetValue(newObj).ToString();

                T? existingEntity = dataServiceManager.Get<T>(id);

                if (existingEntity != null)
                {
                    int rowsAffected = dataServiceManager.Update(newObj);
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

        //Delete
        public ServiceResult<int> Delete<T>(string id) where T : new()
        {
            return ServiceResult<int>.Success(1);
        }

        //Get Id From Name
        public ServiceResult<string> GetIdFromName<T>(string Name)
        {
            try
            {
                string id = dataServiceManager.GetIdByName<T>(Name);
                if (id != null)
                {
                    return ServiceResult<string>.Success(id);
                }
                else
                {
                    id = GenerateNewId<T>().Data;
                    return ServiceResult<string>.Success(id);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail("Database Issue:" + ex.Message);
            }
        }

        //Generate new id
        public ServiceResult<string> GenerateNewId<T>()
        {
            string lastId = dataServiceManager.GetLastId<T>();
            string prefix = "";
            int suffixCount = 4;
            string entityName = typeof(T).Name;
            switch (entityName)
            {
                case "Employee":
                    prefix = "TEZ";
                    suffixCount = 5;

                    break;

                case "Role":
                    prefix = "RL";
                    break;

                case "Project":
                    prefix = "PR";
                    break;

                case "Manager":
                    prefix = "MR";
                    break;

                case "Department":
                    prefix = "DPT";
                    suffixCount = 3;
                    break;

                case "Location":
                    prefix = "LOC";
                    suffixCount = 3;
                    break;

                default:
                    prefix = "";
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

    }
}