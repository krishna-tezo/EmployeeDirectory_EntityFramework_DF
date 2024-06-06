using EmployeeDirectory.Controllers;
using EmployeeDirectory.Core;
using EmployeeDirectory.Interfaces;
using EmployeeDirectory.Model;
using EmployeeDirectory.Models.Models;
using EmployeeDirectory.UI.Controllers;
using EmployeeDirectory.UI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;


namespace EmployeeDirectory.UI.UIServices
{
    internal class UIService : IUIService
    {

        private readonly IRoleController roleController;
        private readonly IEmployeeController employeeController;
        private readonly IValidationService validator;

        public UIService(IEmployeeController employeeController, IRoleController roleController, IValidationService validator)
        {
            this.roleController = roleController;
            this.employeeController = employeeController;
            this.validator = validator;
        }

        #region "Employee Service"

        //Add Employee
        public void AddEmployee()
        {
            Console.WriteLine("\n----Welcome to Add Employee Form----\n");
            Employee employee = GetEmployeeDetailsFromConsole(new Employee(), EmployeeFormType.Add);
            ServiceResult<int> result = employeeController.AddEmployee(employee);
            Console.WriteLine(result.Message);

        }

        //Edit Employee
        public void EditEmployee()
        {
            Employee? employee;
            Console.WriteLine("\n----Welcome To Edit Employee Form----\n");

            string? empId;
            do
            {
                Console.Write("Input Id of the Employee which you want to Edit or -1 to exit: ");
                empId = Console.ReadLine();
                if (empId!.Equals("-1"))
                {
                    break;
                }
                else if (string.IsNullOrEmpty(empId))
                {
                    Console.WriteLine("Don't leave it blank");
                }
                else
                {
                    ServiceResult<Employee> result = employeeController.GetEmployeeById(empId);

                    if (result.IsOperationSuccess)
                    {
                        employee = GetEmployeeDetailsFromConsole(result.Data, EmployeeFormType.Edit, empId);
                        Console.WriteLine(employeeController.EditEmployee(employee).Message);
                    }
                    else
                    {
                        Console.WriteLine(result.Message);
                    }
                }
            }
            while (true);
        }

        //Get Employee Details From Console
        public Employee GetEmployeeDetailsFromConsole(Employee employee, EmployeeFormType formType, string? empId = "")
        {

            Console.WriteLine("----Input Employee Details----");
            ValidationResult result;

            string? firstName;
            do
            {
                Console.Write("Enter First Name:");

                firstName = Console.ReadLine();
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Don't leave it blank");
                }
                else
                {
                    break;
                }

            }
            while (true);


            string? lastName;
            do
            {
                Console.Write("Enter Last Name:");

                lastName = Console.ReadLine();
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Don't leave it blank");
                }
                else
                {
                    break;
                }
            }
            while (true);

            string? email;
            do
            {
                Console.Write("Enter email:");

                email = Console.ReadLine();


                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Don't leave it blank");
                }
                else
                {
                    result = validator.ValidateEmail(email);

                    if (result.IsValid != true)
                    {
                        Console.WriteLine(result.ErrorMessage);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            while (true);

            DateOnly dob;
            do
            {
                Console.Write("Enter Dob (mm/dd/yyyy):");
                if (DateOnly.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Date Format");

                }
            }
            while (true);

            string? mobileNumber;
            do
            {
                Console.Write("Enter Mobile No.:");

                mobileNumber = Console.ReadLine();


                if (string.IsNullOrEmpty(mobileNumber))
                {
                    Console.WriteLine("Don't leave it blank");
                }
                else
                {
                    result = validator.ValidateMobileNumber(mobileNumber);

                    if (result.IsValid != true)
                    {
                        Console.WriteLine(result.ErrorMessage);
                    }
                    else
                    {
                        break;
                    }
                }

            }
            while (true);

            DateOnly joinDate;
            do
            {
                Console.Write("Enter Join Date (mm/dd/yyyy):");

                if (DateOnly.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out joinDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Date Format");
                }
            }
            while (true);

            string? roleId = GetRoleOptions();
            string? projectId = GetProjectOptions();
            


            if (formType == EmployeeFormType.Add)
            {
                empId = employeeController.GetNewEmployeeId().Data;
            }

            employee.Id = empId;
            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Email = email;
            employee.Dob = dob;
            employee.MobileNumber = mobileNumber;
            employee.JoinDate = joinDate;
            employee.RoleId = roleId;
            employee.ProjectId = projectId;
            employee.IsDeleted = false;


            return employee;
        }

        //Get Role
        public string GetRoleOptions()
        {
            string? inputKey;
            int number = 1;
            List<Tuple<string, string, string>> options = new List<Tuple<string, string, string>>();
            Console.WriteLine("\n\n----Available Roles----\n");
            options = roleController.GetRoleNamesWithLocation().Data;

            Dictionary<string, string> optionsMap = [];
            Console.WriteLine("-----------------------------------------------------------");
            options.ForEach(option =>
            {
                optionsMap.Add(number.ToString(), option.Item1);
                string role = String.Format("|{0,-5}|{1,-30}|{2,-20}|", number, option.Item2, option.Item3);
                Console.WriteLine(role);
                number++;
            });
            Console.WriteLine("-----------------------------------------------------------");

            Console.Write("\nChoose Option:");
            inputKey = Console.ReadLine();
            if (inputKey == null || !optionsMap.ContainsKey(inputKey))
            {
                Console.WriteLine("Please Enter a valid option");
                GetRoleOptions();
            }
            return optionsMap[inputKey!];
        }

        // Get Project Options
        public string GetProjectOptions()
        {

            string? inputKey;
            int number = 1;
            List<Tuple<string, string>> options = new List<Tuple<string, string>>();
            Console.WriteLine("\n\n----Available Projects----\n");
            var result = employeeController.GetProjectNames();
            if (result.IsOperationSuccess)
            {
                options = employeeController.GetProjectNames().Data;
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Dictionary<string, Tuple<string, string>> optionsMap = [];
            options.ForEach(option =>
            {
                optionsMap.Add(number.ToString(), new Tuple<string, string>(option.Item1, option.Item2));
                Console.WriteLine(number + ". " + option.Item2);
                number++;
            });

            Console.Write("\nChoose Option:");
            inputKey = Console.ReadLine();
            if (inputKey == null || !optionsMap.ContainsKey(inputKey))
            {
                Console.WriteLine("Please Enter a valid option");
                GetRoleOptions();
            }
            return optionsMap[inputKey].Item1;
        }
        //Get Employee Role Details From Roles Data
        public string GetDepartmentsOption()
        {
            string? inputKey;
            int number = 1;
            List<string> options = new List<string>();
            Console.WriteLine("\n\n----Available Departments----\n");
            options = roleController.GetAllDepartments().Data;

            Dictionary<string, string> optionsMap = [];

            options.ForEach(option =>
            {
                optionsMap.Add(number.ToString(), option);
                Console.WriteLine(number + ". " + option);
                number++;
            });
            inputKey = Console.ReadLine();
            if (inputKey == null || !optionsMap.ContainsKey(inputKey))
            {
                Console.WriteLine("Please Enter a valid option");
            }
            return optionsMap[inputKey!];
        }


        //View Employees in Console
        public void ViewEmployees()
        {

            ServiceResult<List<EmployeeView>> result = employeeController.ViewEmployees();
            if (!result.IsOperationSuccess)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                this.ShowEmployeesDataInTabularFormat(result.Data);
            }

        }

        //View Single Employee
        public void ViewEmployee()
        {
            List<EmployeeView>? employeesToView = [];

            while (true)
            {
                Console.WriteLine("Enter the emp Id to fetch the employee or -1 to exit:");
                string? empId = Console.ReadLine();
                if (empId!.Equals("-1"))
                {
                    break;
                }
                else if (empId != null)
                {
                    ServiceResult<EmployeeView> result = employeeController.ViewEmployee(empId);
                    if (result.IsOperationSuccess)
                    {
                        employeesToView.Add(result.Data);
                        ShowEmployeesDataInTabularFormat(employeesToView);
                        break;
                    }
                    else
                    {
                        Console.WriteLine(result.Message);
                    }
                }
            }

        }

        //Delete Employee
        public void DeleteEmployee()
        {
            while (true)
            {
                Console.WriteLine("Enter Employee Id You want to delete OR -1 to exit");
                string empId = Console.ReadLine() ?? string.Empty;
                if (empId.IsNullOrEmpty())
                {
                    Console.WriteLine("Don't leave blank");
                }
                else if (empId != "-1")
                {
                    Console.WriteLine(employeeController.DeleteEmployee(empId).Message);
                    
                }
                else
                {
                    break;
                }
            }
        }

        //Representation of Data in Tabular Format
        public void ShowEmployeesDataInTabularFormat(List<EmployeeView> employees)
        {
            Console.WriteLine("\nEmployee List");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            string headers = String.Format("|{0,10}|{1,20}|{2,30}|{3,20}|{4,20}|{5,20}|{6,20}|{7,20}|", "EmpId", "Name", "Role", "Department", "Location", "Join Date", "Manager Name", "Project Name");
            Console.WriteLine(headers);
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            employees.ForEach((emp) =>
            {
                string empData = String.Format("|{0,10}|{1,20}|{2,30}|{3,20}|{4,20}|{5,20}|{6,20}|{7,20}|",
                        emp.Id, emp.Name, emp.Role, emp.Department, emp.Location, emp.JoinDate.ToString(), emp.ManagerName, emp.ProjectName);
                Console.WriteLine(empData);
            });

            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }
        #endregion



        #region "Roles Service"

        public void ViewAllRoles()
        {
            List<RoleView> roles = roleController.ViewRoles().Data;

            ShowRolesDataInTabularFormat(roles);
        }

        //Get New Role Details From Console
        public void AddRole()
        {

            Console.WriteLine("\n----Welcome to Add Role Menu----\n");
            Console.WriteLine("\n----Input Role Details----\n");
            string? department = GetDepartmentsOption();

            string? roleName;
            string? location;
            string? description;
            string roleId;
            do
            {
                Console.Write("Enter Role Name: ");
                roleName = Console.ReadLine();

                Console.Write("Enter Location: ");
                location = Console.ReadLine();

                Console.Write("Enter Description:");
                description = Console.ReadLine();

                if (string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(description))
                {
                    Console.WriteLine("Don't leave any field");
                }
                else if (roleController.DoesRoleExists(roleName, location).Data)
                {
                    Console.WriteLine("This role already exists");
                }
                else
                {
                    roleId = roleController.GenerateRoleId().Data;
                    break;
                }

            } while (true);



            RoleView role = new()
            {
                Id = roleId,
                Name = roleName,
                Location = location,
                Department = department,
                Description = description
            };

            Console.WriteLine(roleController.Add(role).Message);
            
        }
        public bool DoesRoleExist(string roleId)
        {
            List<RoleView> roles = roleController.ViewRoles().Data;
            return roles.Any(role => role.Id == roleId);
        }

        
        public void ShowRolesDataInTabularFormat(List<RoleView> roles)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
            string headers = String.Format("|{0,30}|{1,30}|{2,20}|{3,50}|", "RoleName", "Department", "Location", "Description");
            Console.WriteLine(headers);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");

            roles.ForEach(role =>
            {
                string roleData = String.Format("|{0,30}|{1,30}|{2,20}|{3,50}|", role.Name, role.Department, role.Location, role.Description);
                Console.WriteLine(roleData);
            });
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
        }
        #endregion
    }
}