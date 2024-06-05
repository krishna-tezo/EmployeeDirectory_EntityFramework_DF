using AutoMapper;
using EmployeeDirectory.Data.Interfaces;
using EmployeeDirectory.Data.Models;
using EmployeeDirectory.Data.SummaryModels;
using EmployeeDirectory.Models;
using EmployeeDirectory.Models.Models;

namespace EmployeeDirectory.Services
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository employeeRepository;
        IGenericRepository<Project> projectRepository;
        IGenericRepository<Manager> managerRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IGenericRepository<Project> projectRepository, IGenericRepository<Manager> managerRepository)
        {
            this.employeeRepository = employeeRepository;
            this.projectRepository = projectRepository;
            this.managerRepository = managerRepository;
        }

        #region Summary
        public ServiceResult<List<EmployeeSummary>> GetEmployeeSummaries()
        {
            List<EmployeeSummary> employees = [];
            try
            {
                employees = employeeRepository.GetEmployeesSummary();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            employees = employees.Where(emp => emp.IsDeleted == false).ToList();
            if (employees != null)
            {
                return ServiceResult<List<EmployeeSummary>>.Success(employees);
            }
            else
            {
                return ServiceResult<List<EmployeeSummary>>.Fail("No Employees");
            }
        }

        public ServiceResult<EmployeeSummary> GetEmployeeSummary(string id)
        {

            EmployeeSummary employee = new EmployeeSummary();
            try
            {
                employee = employeeRepository.GetEmployeeSummaryById(id);
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

        #region CRUD
        //Read all
        public ServiceResult<List<ProjectModel>> GetAllProjects()
        {
            try
            {
                List<ProjectModel> collection = [];
                List<Project> Project = projectRepository.GetAll();
                foreach (var project in Project)
                {
                    collection.Add(GetMappedObject<Project, ProjectModel>(project).Data);
                }

                if (collection == null || collection.Count == 0)
                {
                    return ServiceResult<List<ProjectModel>>.Fail($"No Projects to show");
                }

                return ServiceResult<List<ProjectModel>>.Success(collection);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ProjectModel>>.Fail("Database Issue: " + ex.Message);
            }
        }

        //Add
        public ServiceResult<int> AddEmployee(EmployeeModel employee)
        {
            try
            {
                int rowsAffected = 0;
                Employee emp = GetMappedObject<EmployeeModel, Employee>(employee).Data;

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
        public ServiceResult<int> UpdateEmployee(EmployeeModel employee)
        {
            try
            {
                int rowsAffected = 0;
                Employee emp = GetMappedObject<EmployeeModel, Employee>(employee).Data;
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
        public ServiceResult<int> Delete(string id)
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
                string lastId = null;
                string prefix = "";
                int suffixCount = 4;

                switch (typeof(T).Name)
                {
                    case nameof(EmployeeModel):
                        {
                            Employee employee = employeeRepository.GetLast();
                            if (employee == null)
                            {
                                return ServiceResult<string>.Success("TEZ00001");
                            }
                            lastId = employee.Id;
                            prefix = "TEZ";
                            suffixCount = 5;
                            break;
                        }
                    case nameof(ProjectModel):
                        {
                            Project project = projectRepository.GetLast();
                            if (project == null)
                            {
                                return ServiceResult<string>.Success("PR0001");
                            }
                            lastId = project.Id;
                            prefix = "PR";
                            break;
                        }
                    case nameof(ManagerModel):
                        {
                            Manager manager = managerRepository.GetLast();
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

                string numericPart = lastId?.Substring(prefix.Length);

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

        #endregion

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