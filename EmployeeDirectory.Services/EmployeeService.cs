using AutoMapper;
using EmployeeDirectory.Data.Interfaces;
using DM = EmployeeDirectory.Data.Models;
using EmployeeDirectory.Models.Interfaces;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.Models.SummaryModels;

namespace EmployeeDirectory.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository, IGenericRepository<DM.Project> projectRepository, IGenericRepository<DM.Manager> managerRepository, IMapper mapper) : IEmployeeService
    {
        readonly IEmployeeRepository employeeRepository = employeeRepository;
        readonly IGenericRepository<DM.Project> projectRepository = projectRepository;
        readonly IGenericRepository<DM.Manager> managerRepository = managerRepository;
        readonly IMapper mapper = mapper;

        #region Summary
        public ServiceResult<List<EmployeeSummary>> GetEmployeeSummaries()
        {
            try
            {
                List<EmployeeSummary> employees = employeeRepository.GetEmployeesSummary();
                return ServiceResult<List<EmployeeSummary>>.Success(employees);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<EmployeeSummary>>.Fail("Database Error:"+ex.Message);
            }
        }

        public ServiceResult<EmployeeSummary> GetEmployeeSummary(string id)
        {
            try
            {
                EmployeeSummary employee = employeeRepository.GetEmployeeSummaryById(id);
                if (employee != null)
                {
                    return ServiceResult<EmployeeSummary>.Success(employee);
                }
                else
                {
                    return ServiceResult<EmployeeSummary>.Fail("No Employee Found");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<EmployeeSummary>.Fail("Database Issue:" + ex.Message);
            }

        }
        #endregion

        public ServiceResult<List<Project>> GetAllProjects()
        {
            try
            {
                List<Project> collection = [];
                List<DM.Project> Project = projectRepository.GetAll();
                foreach (var project in Project)
                {
                    collection.Add(mapper.Map<DM.Project, Project>(project));
                }

                if (collection == null || collection.Count == 0)
                {
                    return ServiceResult<List<Project>>.Fail($"No Projects to show");
                }

                return ServiceResult<List<Project>>.Success(collection);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Project>>.Fail("Database Issue: " + ex.Message);
            }
        }

        //Add
        public ServiceResult<int> AddEmployee(Employee employee)
        {
            try
            {
                int rowsAffected = 0;
                DM.Employee emp = mapper.Map<Employee, DM.Employee>(employee);

                rowsAffected = employeeRepository.Insert(emp);

                if (rowsAffected == 0)
                {
                    return ServiceResult<int>.Fail("Database Connectivity Issue");
                }
                return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} data has been inserted");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue: " + ex.Message);
            }
        }

        //Update
        public ServiceResult<int> UpdateEmployee(Employee employee)
        {
            try
            {
                int rowsAffected = 0;
                DM.Employee emp = mapper.Map<Employee, DM.Employee>(employee);
                rowsAffected = employeeRepository.UpdateEmployee(emp);

                if (rowsAffected == 0)
                {
                    return ServiceResult<int>.Fail("Database Connectivity Issue");
                }
                return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} data has been updated");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("Database Issue: " + ex.Message);
            }
        }
        //Delete Employee
        public ServiceResult<int> DeleteEmployee(string id)
        {
            try
            {
                int rowsAffected = employeeRepository.DeleteEmployee(id);
                if (rowsAffected > 0)
                {
                    return ServiceResult<int>.Success(rowsAffected, $"{rowsAffected} row affected");
                }
                else
                {
                    return ServiceResult<int>.Fail("Couldn't delete");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail("DataBase Error" + ex.Message);
            }
        }

        //Generate New Id
        public ServiceResult<string> GenerateNewId<T>()
        {
            try
            {
                string? lastId = null;
                string prefix = "";
                int suffixCount = 4;

                switch (typeof(T).Name)
                {
                    case nameof(Employee):
                        {
                            DM.Employee employee = employeeRepository.GetLast();
                            if (employee == null)
                            {
                                return ServiceResult<string>.Success("TEZ00001");
                            }
                            lastId = employee.Id;
                            prefix = "TEZ";
                            suffixCount = 5;
                            break;
                        }
                    case nameof(Project):
                        {
                            DM.Project project = projectRepository.GetLast();
                            if (project == null)
                            {
                                return ServiceResult<string>.Success("PR0001");
                            }
                            lastId = project.Id;
                            prefix = "PR";
                            break;
                        }
                    case nameof(Manager):
                        {
                            DM.Manager manager = managerRepository.GetLast();
                            if (manager == null)
                            {
                                return ServiceResult<string>.Success("LOC001");
                            }
                            lastId = manager.Id;
                            prefix = "MR";
                            break;
                        }
                    default:
                        return ServiceResult<string>.Fail("Unsupported entity type for ID generation");
                }

                string? numericPart = lastId?.Substring(prefix.Length);

                if (int.TryParse(numericPart, out int numericId))
                {
                    int newNumericId = numericId + 1;
                    string newId = prefix + newNumericId.ToString($"D{suffixCount}");
                    return ServiceResult<string>.Success(newId);
                }
                else
                {
                    return ServiceResult<string>.Fail("Invalid Id Format");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail("Database Issue: " + ex.Message);
            }
        }
    }
}